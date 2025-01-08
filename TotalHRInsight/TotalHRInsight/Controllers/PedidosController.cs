using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TotalHRInsight.DAL;
using TotalHRInsight.Models;
using TotalHRInsight.Models.Pedidos;

namespace TotalHRInsight.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class PedidosController : Controller
	{
		private readonly TotalHRInsightDbContext _context;
		private readonly TotalHRInsightDbContext _context2;

		public PedidosController(TotalHRInsightDbContext context, TotalHRInsightDbContext context2)
		{
			_context = context;
			_context2 = context2;
		}

		// GET: Pedidos
		public IActionResult Index()
		{
			ViewData["sucursales"] = new SelectList(_context.Sucursales
										.Where(w => w.NombreSucursal != "Centro de Produccion"), "IdSucursal", "NombreSucursal");

			return View();
		}

		[HttpGet]
		public async Task<JsonResult> FiltrarPedidos(int idSucursal, DateTime? fechaInicio, DateTime? fechaFinal)
		{
			try
			{
				List<Pedido> pedidos = new List<Pedido>();

				if (idSucursal == null && !fechaInicio.HasValue && !fechaFinal.HasValue)
				{
					pedidos = await _context.Pedidos
					.Include(a => a.Estado)
					.Include(a => a.Sucursal)
					.Include(a => a.UsuarioCreacion)
					.OrderBy(o => o.FechaPedido)
					.Where(w => w.Sucursal.NombreSucursal != "Centro de Produccion")
					.ToListAsync();
				}
				else
				{

				pedidos = await _context.Pedidos
					.Include(a => a.Estado)
					.Include(a => a.Sucursal)
					.Include(a => a.UsuarioCreacion)
					.OrderBy(o => o.FechaPedido)
					.Where(w => w.Sucursal.IdSucursal.Equals(idSucursal) &&
					(!fechaInicio.HasValue || w.FechaPedido.Date >= fechaInicio.Value.Date) &&
					(!fechaFinal.HasValue || w.FechaPedido.Date <= fechaFinal.Value.Date))
					.ToListAsync();
				}

				return Json(pedidos);

			}
			catch (Exception ex)
			{
				return Json(new { error = ex.Message });
			}

		}

		// GET: Pedidos
		public async Task<IActionResult> Devolucion()
		{
			var pedidos = await _context.Pedidos.Include(p => p.Estado)
										.Include(p => p.Sucursal)
										.Include(p => p.UsuarioCreacion)
										.Where(w => w.Sucursal.NombreSucursal == "Centro de Produccion")
										.ToListAsync();

			return View(pedidos);
		}


		// GET: Pedidos/Details/5
		public async Task<IActionResult> Details(int? IdPedido)
		{

			if (IdPedido == null)
			{
				return NotFound();
			}

			var pedido = await _context.Pedidos
				.Include(p => p.Estado)
				.Include(p => p.Sucursal)
				.Include(p => p.UsuarioCreacion)
				.Include(p => p.PedidosProductos)
					.ThenInclude(pp => pp.Producto)
				.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

			if (pedido == null)
			{
				return NotFound();
			}

			var pedidoViewModel = new PedidoViewModel
			{
				Pedido = pedido,
				PedidosProductos = (List<PedidosProductos>)pedido.PedidosProductos
			};

			return View(pedidoViewModel);
		}

		// GET: Pedidos/Create
		public async Task<IActionResult> CreateAsync()
		{
			ViewData["Inventario"] = await _context2.Inventario
				.Include(i => i.Producto)
				.Include(i => i.Sucursal)
				.Include(i => i.UsuarioCreacion)
				.Include(i => i.UsuarioModificacion)
				.Where(c => c.Sucursal.NombreSucursal == "Centro de Produccion")
				.ToListAsync();
			ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
			ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
			ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CrearPedido pedido, string ProductosJson)
		{
			if (!ModelState.IsValid)
			{
				var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
				return BadRequest(string.Join(", ", errores));
			}

			var estadoPendiente = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoSolicitud == "En proceso");

			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var productosArray = JArray.Parse(ProductosJson);
				var productos = productosArray.Select(item => new ListaProducto
				{
					IdProducto = item["IdProducto"].Value<int>(),
					NombreProducto = item["NombreProducto"].Value<string>(),
					PrecioUnitario = item["PrecioUnitario"].Value<double>(),
					CantidadSelecciona = item["CantidadSeleccionada"].Value<int>()
				}).ToList();

				var nuevoPedido = new Pedido
				{
					FechaEntrega = pedido.FechaEntrega,
					FechaPedido = DateTime.Now,
					UsuarioCreacionId = User.FindFirstValue(ClaimTypes.NameIdentifier),
					IdSucursal = pedido.IdSucursal,
					IdEstado = estadoPendiente.IdEstado,
					MontoTotal = pedido.MontoTotal
				};

				_context.Pedidos.Add(nuevoPedido);
				await _context.SaveChangesAsync();

				bool errorEnProductos = false;
				string mensajeError = "";

				foreach (var producto in productos)
				{
					if (!await _context.Productos.AnyAsync(p => p.IdProducto == producto.IdProducto))
					{
						errorEnProductos = true;
						mensajeError = $"El producto con ID {producto.IdProducto} no existe.";
						break;
					}

					var detallePedido = new PedidosProductos
					{
						ProductosID = producto.IdProducto,
						PedidoID = nuevoPedido.IdPedido,
						Cantidad = producto.CantidadSelecciona,
					};
					_context.PedidosProductos.Add(detallePedido);

					// Restar la cantidad de la sucursal principal
					var productoSucursalPrincipal = await _context.Inventario
						.FirstOrDefaultAsync(ps => ps.ProductoId == producto.IdProducto && ps.SucursalId == 2);

					if (productoSucursalPrincipal != null)
					{
						productoSucursalPrincipal.CantidadDisponible -= producto.CantidadSelecciona;
						if (productoSucursalPrincipal.CantidadDisponible < 0)
						{
							errorEnProductos = true;
							mensajeError = $"No hay suficiente stock del producto {producto.NombreProducto} en la sucursal principal.";
							break;
						}
						_context.Inventario.Update(productoSucursalPrincipal);
					}
					else
					{
						errorEnProductos = true;
						mensajeError = $"No se encontró el producto {producto.NombreProducto} en la sucursal principal.";
						break;
					}
				}

				if (errorEnProductos)
				{
					// Eliminar el pedido si hubo un error con los productos
					_context.Pedidos.Remove(nuevoPedido);
					var registros = _context.PedidosProductos.Where(x => x.PedidoID == nuevoPedido.IdPedido);
					_context.RemoveRange(registros);
					await _context.SaveChangesAsync();
					await transaction.RollbackAsync();
					return BadRequest(mensajeError);
				}

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();
				return Ok(new { message = "Pedido creado exitosamente" });
			}
			catch (DbUpdateException ex)
			{
				await transaction.RollbackAsync();
				var errorMessage = ObtenerMensajeDeError(ex);
				return StatusCode(500, $"Error al guardar en la base de datos: {errorMessage}");
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, $"Error al crear el pedido: {ex.Message}");
			}
		}

		private string ObtenerMensajeDeError(Exception ex)
		{
			var mensaje = ex.Message;
			var innerException = ex.InnerException;
			var depth = 0;
			while (innerException != null && depth < 5)
			{
				mensaje += $" Inner exception: {innerException.Message}";
				innerException = innerException.InnerException;
				depth++;
			}
			return mensaje;
		}

		[HttpPost]
		public IActionResult ActualizarCantidad(int idProducto, int cantidad, bool esAgregar)
		{
			try
			{
				var inventario = _context.Inventario.Find(idProducto);
				if (inventario != null)
				{
					if (esAgregar)
					{
						inventario.CantidadDisponible -= cantidad;
					}
					else
					{
						inventario.CantidadDisponible += cantidad;
					}
					_context.SaveChanges();
					return Ok();
				}
				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// GET: Pedidos/Edit/5
		public async Task<IActionResult> Edit(int? IdPedido)
		{
			if (IdPedido == null)
			{
				return NotFound();
			}
			var pedido = await _context.Pedidos
				.Include(p => p.UsuarioCreacion)
				.Include(p => p.Sucursal)
				.Include(p => p.Estado)
				.Include(p => p.PedidosProductos)
					.ThenInclude(pp => pp.Producto)
				.FirstOrDefaultAsync(m => m.IdPedido == IdPedido);

			if (pedido == null)
			{
				return NotFound();
			}
			ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
			return View(pedido);
		}

		// POST: Pedidos/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int IdPedido, [Bind("IdPedido,FechaEntrega,IdEstado")] Pedido pedidoActualizado)
		{
			if (IdPedido != pedidoActualizado.IdPedido)
			{
				return NotFound();
			}
			ModelState.Remove("UsuarioCreacionId");

			if (ModelState.IsValid)
			{
				try
				{
					// Obtener el pedido original de la base de datos
					var pedidoOriginal = await _context.Pedidos
						.Include(p => p.UsuarioCreacion)
						.Include(p => p.Sucursal)
						.Include(p => p.Estado)
						.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

					if (pedidoOriginal == null)
					{
						return NotFound();
					}

					// Actualizar solo los campos permitidos
					pedidoOriginal.FechaEntrega = pedidoActualizado.FechaEntrega;
					pedidoOriginal.IdEstado = pedidoActualizado.IdEstado;

					// Marcar solo el pedido como modificado
					_context.Entry(pedidoOriginal).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PedidoExists(pedidoActualizado.IdPedido))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}

			// Si llegamos aquí, algo falló, volvemos a cargar los datos necesarios
			var pedido = await _context.Pedidos
				.Include(p => p.UsuarioCreacion)
				.Include(p => p.Sucursal)
				.Include(p => p.Estado)
				.Include(p => p.PedidosProductos)
					.ThenInclude(pp => pp.Producto)
				.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

			ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
			return View(pedido);
		}



		// GET: Pedidos/Delete/5
		public async Task<IActionResult> Delete(int? IdPedido)
		{
			if (IdPedido == null)
			{
				return NotFound();
			}

			var pedido = await _context.Pedidos
				.Include(p => p.Estado)
				.Include(p => p.Sucursal)
				.Include(p => p.UsuarioCreacion)
				.Include(p => p.PedidosProductos)
					.ThenInclude(pp => pp.Producto)
				.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

			if (pedido == null)
			{
				return NotFound();
			}

			var pedidoViewModel = new PedidoViewModel
			{
				Pedido = pedido,
				PedidosProductos = (List<PedidosProductos>)pedido.PedidosProductos
			};

			return View(pedidoViewModel);
		}

		// POST: Pedidos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int IdPedido)
		{
			var pedido = await _context.Pedidos
				.Include(p => p.PedidosProductos)
				.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

			if (pedido != null)
			{
				// Eliminar todos los PedidosProductos asociados
				_context.PedidosProductos.RemoveRange(pedido.PedidosProductos);

				// Eliminar el pedido
				_context.Pedidos.Remove(pedido);

				// Guardar los cambios en la base de datos
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		private bool PedidoExists(int IdPedido)
		{
			return _context.Pedidos.Any(e => e.IdPedido == IdPedido);
		}


		public async Task<IActionResult> ExportToExcel(int idSucursal, DateTime? fechaInicio, DateTime? fechaFinal)
		{
			try
			{
				var pedidos = await ObtenerPedidos(idSucursal, fechaInicio, fechaFinal);

				using (var workbook = new XLWorkbook())
				{
					var worksheet = ConfigurarHojaExcel(workbook);
					ConfigurarEncabezado(worksheet);
					AgregarDatosPedidos(worksheet, pedidos);
					AplicarEstilosTabla(worksheet, pedidos.Count);

					return GenerarArchivoExcel(workbook);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
				return BadRequest($"Error al generar Excel: {ex.Message}");
			}
		}

		private async Task<List<Pedido>> ObtenerPedidos(int idSucursal, DateTime? fechaInicio, DateTime? fechaFinal)
		{
			return await _context.Pedidos
				.Include(a => a.Estado)
				.Include(a => a.Sucursal)
				.Include(a => a.UsuarioCreacion)
				.OrderBy(o => o.FechaPedido)
				.Where(w => w.Sucursal.IdSucursal.Equals(idSucursal) &&
					(!fechaInicio.HasValue || w.FechaPedido.Date >= fechaInicio.Value.Date) &&
					(!fechaFinal.HasValue || w.FechaPedido.Date <= fechaFinal.Value.Date))
				.ToListAsync();
		}

		private IXLWorksheet ConfigurarHojaExcel(XLWorkbook workbook)
		{
			var worksheet = workbook.Worksheets.Add("Pedidos");
			worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

			// Ajustes iniciales
			worksheet.Row(1).Height = 60;
			worksheet.Columns().AdjustToContents();

			return worksheet;
		}

		private void ConfigurarEncabezado(IXLWorksheet worksheet)
		{
			// Título principal
			var titulo = worksheet.Cell("A1");
			titulo.Value = "Reporte de Pedidos";
			titulo.Style
				.Font.SetBold(true)
				.Font.SetFontSize(16)
				.Fill.SetBackgroundColor(XLColor.FromHtml("#4472C4"))
				.Font.SetFontColor(XLColor.White)
				.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

			// Cabeceras
			var headers = new[] { "N° Pedido", "Fecha Pedido", "Fecha Entrega", "Usuario", "Sucursal", "Estado", "Monto Total" };
			for (int i = 0; i < headers.Length; i++)
			{
				var cell = worksheet.Cell(3, i + 1);
				cell.Value = headers[i];
				cell.Style
					.Font.SetBold(true)
					.Font.SetFontSize(12)
					.Fill.SetBackgroundColor(XLColor.FromHtml("#D9E1F2"))
					.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
			}
		}

		private void AgregarDatosPedidos(IXLWorksheet worksheet, List<Pedido> pedidos)
		{
			int rowIdx = 4;
			foreach (var pedido in pedidos)
			{
				worksheet.Cell(rowIdx, 1).Value = pedido.IdPedido;
				worksheet.Cell(rowIdx, 2).Value = pedido.FechaPedido.ToString("dd/MM/yyyy");
				worksheet.Cell(rowIdx, 3).Value = pedido.FechaEntrega.ToString("dd/MM/yyyy");
				worksheet.Cell(rowIdx, 4).Value = pedido.UsuarioCreacion.Nombre;
				worksheet.Cell(rowIdx, 5).Value = pedido.Sucursal.NombreSucursal;
				worksheet.Cell(rowIdx, 6).Value = pedido.Estado.EstadoSolicitud;
				worksheet.Cell(rowIdx, 7).SetValue(pedido.MontoTotal)
					.Style.NumberFormat.SetFormat("#,##0.00");

				rowIdx++;
			}
		}

		private void AplicarEstilosTabla(IXLWorksheet worksheet, int totalRows)
		{
			var rango = worksheet.Range(3, 1, totalRows + 3, 7);
			var tabla = rango.CreateTable();
			tabla.Theme = XLTableTheme.TableStyleMedium2;

			worksheet.Columns().AdjustToContents();

			// Alternar colores de filas
			for (int row = 4; row <= totalRows + 3; row++)
			{
				if (row % 2 == 0)
				{
					worksheet.Row(row).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F2F2F2"));
				}
			}
		}

		private FileResult GenerarArchivoExcel(XLWorkbook workbook)
		{
			using (var stream = new MemoryStream())
			{
				workbook.SaveAs(stream);
				var content = stream.ToArray();
				string fileName = $"Reporte_Pedidos_{DateTime.Now:yyyyMMdd}.xlsx";

				return new FileContentResult(content,
					"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
				{
					FileDownloadName = fileName
				};
			}
		}

		public async Task<IActionResult> ExportPedido(int idSucursal)
		{
			var pedido = await ObtenerPedidoDetallado(idSucursal);
			if (pedido == null) return NotFound();

			try
			{
				using var workbook = new XLWorkbook();
				var worksheet = ConfigurarHojaDetalle(workbook, pedido.IdPedido);
				AgregarInformacionPedido(worksheet, pedido);
				AgregarDetalleProductos(worksheet, pedido.PedidosProductos);

				return GenerarArchivoPedido(workbook, pedido.IdPedido);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error al exportar pedido: {ex.Message}");
			}
		}

		private async Task<Pedido> ObtenerPedidoDetallado(int idSucursal)
		{
			return await _context.Pedidos
				.Include(p => p.Estado)
				.Include(p => p.Sucursal)
				.Include(p => p.UsuarioCreacion)
				.Include(p => p.PedidosProductos)
					.ThenInclude(pp => pp.Producto)
				.FirstOrDefaultAsync(p => p.IdPedido == idSucursal);
		}

		private IXLWorksheet ConfigurarHojaDetalle(XLWorkbook workbook, int idPedido)
		{
			var worksheet = workbook.Worksheets.Add($"Pedido_{idPedido}");
			worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

			var titleCell = worksheet.Cell("A2");
			titleCell.Value = $"Detalle del Pedido #{idPedido}";
			titleCell.Style
				.Font.SetBold(true)
				.Font.SetFontSize(16)
				.Fill.SetBackgroundColor(XLColor.FromHtml("#4472C4"))
				.Font.SetFontColor(XLColor.White)
				.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

			worksheet.Range("A2:G2").Merge();

			return worksheet;
		}

		private void AgregarInformacionPedido(IXLWorksheet worksheet, Pedido pedido)
		{
			var infoFields = new Dictionary<string, string>
   {
	   {"Fecha Pedido", pedido.FechaPedido.ToString("dd/MM/yyyy")},
	   {"Fecha Entrega", pedido.FechaEntrega.ToString("dd/MM/yyyy")},
	   {"Usuario", pedido.UsuarioCreacion.Nombre},
	   {"Sucursal", pedido.Sucursal.NombreSucursal},
	   {"Estado", pedido.Estado.EstadoSolicitud}
   };

			int row = 3;
			foreach (var field in infoFields)
			{
				worksheet.Cell(row, 1).Value = $"{field.Key}:";
				worksheet.Cell(row, 2).Value = field.Value;
				worksheet.Cell(row, 1).Style.Font.SetBold(true);
				row++;
			}

			worksheet.Cell(row, 1).Value = "Monto Total:";
			worksheet.Cell(row, 2).Value = pedido.MontoTotal;
			worksheet.Cell(row, 2).Style.NumberFormat.SetFormat("₡ #,##0.00");
			worksheet.Cell(row, 1).Style.Font.SetBold(true);
		}

		private void AgregarDetalleProductos(IXLWorksheet worksheet, ICollection<PedidosProductos> productos)
		{
			var headers = new[] { "Código", "Producto", "Precio Unitario", "Cantidad" };
			var headerRow = worksheet.Row(10);

			for (int i = 0; i < headers.Length; i++)
			{
				var cell = headerRow.Cell(i + 1);
				cell.Value = headers[i];
				cell.Style
					.Font.SetBold(true)
					.Font.SetFontSize(12)
					.Fill.SetBackgroundColor(XLColor.FromHtml("#4472C4"))
					.Font.SetFontColor(XLColor.White)
					.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
			}

			int rowIdx = 11;
			foreach (var producto in productos)
			{
				var row = worksheet.Row(rowIdx);
				row.Cell(1).Value = producto.ProductosID;
				row.Cell(2).Value = producto.Producto.NombreProducto;
				row.Cell(3).SetValue(producto.Producto.PrecioUnitario)
					.Style.NumberFormat.SetFormat("₡ #,##0.00");
				row.Cell(4).Value = producto.Cantidad;

				if (rowIdx % 2 == 0)
				{
					worksheet.Range(rowIdx, 1, rowIdx, 4)
						.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F2F2F2"));
				}

				rowIdx++;
			}

			worksheet.Columns().AdjustToContents();
		}

		private FileResult GenerarArchivoPedido(XLWorkbook workbook, int idPedido)
		{
			using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			var content = stream.ToArray();

			return new FileContentResult(content,
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileDownloadName = $"Pedido_{idPedido}_{DateTime.Now:yyyyMMdd}.xlsx"
			};
		}




	}
}