﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
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
		public async Task<IActionResult> Index()
		{
            var pedidos = await _context.Pedidos.Include(p => p.Estado)
                                        .Include(p => p.Sucursal)
                                        .Include(p => p.UsuarioCreacion)
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
            ViewData["Inventario"] = await _context2.Inventario.Include(i => i.Producto).Include(i => i.Sucursal).Include(i => i.UsuarioCreacion).Include(i => i.UsuarioModificacion).ToListAsync(); ;
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

            var estadoPendiente = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoSolicitud == "En Progreso");

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
                }

                if (errorEnProductos)
                {
                    // Eliminar el pedido si hubo un error con los productos
                    _context.Pedidos.Remove(nuevoPedido);
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
       

        public async Task<IActionResult> ExportToExcel()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.Sucursal)
                .Include(p => p.UsuarioCreacion)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Pedidos");

                // Agregar las imágenes y ajustar tamaño (como en el ejemplo anterior)
                var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");
                var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);

                // Ajustar las celdas para las imágenes (como en el ejemplo anterior)
                worksheet.Row(1).Height = 60;
                worksheet.Column(1).Width = 12;
                worksheet.Column(7).Width = 12;

                // Título
                var titleCell = worksheet.Cell("A3");
                titleCell.Value = "Informe de Pedidos";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4"); // Color de fondo azul de Excel
                titleCell.Style.Font.FontColor = XLColor.White;


                // Cabeceras de la tabla
                var headerRow = worksheet.Row(5);
                headerRow.Cell(1).Value = "IdPedido";
                headerRow.Cell(2).Value = "FechaPedido";
                headerRow.Cell(3).Value = "FechaEntrega";
                headerRow.Cell(4).Value = "UsuarioCreacion";
                headerRow.Cell(5).Value = "Sucursal";
                headerRow.Cell(6).Value = "Estado";
                headerRow.Cell(7).Value = "MontoTotal";
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Font.FontSize = 12;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Style.Font.FontColor = XLColor.White;


                // Datos
                int rowIdx = 6;
                foreach (var pedido in pedidos)
                {
                    var dataRow = worksheet.Row(rowIdx);
                    dataRow.Cell(1).Value = pedido.IdPedido;
                    dataRow.Cell(2).Value = pedido.FechaPedido.ToString("yyyy-MM-dd");
                    dataRow.Cell(3).Value = pedido.FechaEntrega.ToString("yyyy-MM-dd");
                    dataRow.Cell(4).Value = pedido.UsuarioCreacion.Nombre; // Ajusta según la propiedad correcta
                    dataRow.Cell(5).Value = pedido.Sucursal.NombreSucursal;
                    dataRow.Cell(6).Value = pedido.Estado.EstadoSolicitud;
                    dataRow.Cell(7).Value = pedido.MontoTotal;
                 
                    rowIdx++;
                }

                // Establecer el estilo de tabla para los datos
                var tableRange = worksheet.Range("A5:G" + rowIdx);
                var table = tableRange.CreateTable();

                // Establecer estilo de tabla (opcional): Aquí puedes ajustar según tus preferencias
                table.Theme = XLTableTheme.TableStyleMedium2; // Ejemplo de estilo de tabla

                // Ajustar el ancho de las columnas después de agregar los datos
                worksheet.Columns().AdjustToContents();

                // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos.xlsx");
                }
            }
        }

    }
}