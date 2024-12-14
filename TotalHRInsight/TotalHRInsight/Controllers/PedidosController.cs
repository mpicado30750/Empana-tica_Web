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
		public async Task<IActionResult> Index()
		{
            var pedidos = await _context.Pedidos.Include(p => p.Estado)
                                        .Include(p => p.Sucursal)
                                        .Include(p => p.UsuarioCreacion)
                                        .Where(w => w.Sucursal.NombreSucursal != "Centro de Produccion")
                                        .ToListAsync();

            return View(pedidos);
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


        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                Console.WriteLine("Inicio del método ExportToExcel");

                var pedidos = await _context.Pedidos
                    .Include(p => p.Estado)
                    .Include(p => p.Sucursal)
                    .Include(p => p.UsuarioCreacion)
                    .ToListAsync();
                Console.WriteLine($"Cantidad de pedidos obtenidos: {pedidos.Count}");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Pedidos");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                    Console.WriteLine("Orientación de página establecida a paisaje");

                    //// Agregar las imágenes y ajustar tamaño
                    //var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    //var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");

                    //Console.WriteLine($"Ruta de imagen 1: {imagePath1}");
                    //Console.WriteLine($"Ruta de imagen 2: {imagePath2}");

                    //if (System.IO.File.Exists(imagePath1))
                    //{
                    //    var picture1 = worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                    //    Console.WriteLine("Imagen 1 agregada al Excel");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Imagen 1 no encontrada");
                    //}

                    //if (System.IO.File.Exists(imagePath2))
                    //{
                    //    var picture2 = worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);
                    //    Console.WriteLine("Imagen 2 agregada al Excel");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Imagen 2 no encontrada");
                    //}

                    // Ajustar las celdas para las imágenes
                    worksheet.Row(1).Height = 60;
                    worksheet.Column(1).Width = 12;
                    worksheet.Column(7).Width = 12;
                    Console.WriteLine("Celdas ajustadas para las imágenes");

                    // Título
                    var titleCell = worksheet.Cell("A3");
                    titleCell.Value = "Informe de Pedidos";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    Console.WriteLine("Título del informe configurado");

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
                    Console.WriteLine("Cabeceras de la tabla configuradas");

                    // Datos
                    int rowIdx = 6;
                    foreach (var pedido in pedidos)
                    {
                        var dataRow = worksheet.Row(rowIdx);
                        dataRow.Cell(1).Value = pedido.IdPedido;
                        dataRow.Cell(2).Value = pedido.FechaPedido.ToString("yyyy-MM-dd");
                        dataRow.Cell(3).Value = pedido.FechaEntrega.ToString("yyyy-MM-dd");
                        dataRow.Cell(4).Value = pedido.UsuarioCreacion.Nombre;
                        dataRow.Cell(5).Value = pedido.Sucursal.NombreSucursal;
                        dataRow.Cell(6).Value = pedido.Estado.EstadoSolicitud;
                        dataRow.Cell(7).Value = pedido.MontoTotal;
                        rowIdx++;
                    }
                    Console.WriteLine("Datos de pedidos agregados al Excel");

                    // Establecer el estilo de tabla para los datos
                    var tableRange = worksheet.Range("A5:G" + rowIdx);
                    var table = tableRange.CreateTable();
                    table.Theme = XLTableTheme.TableStyleMedium2;
                    Console.WriteLine("Estilo de tabla establecido");

                    // Ajustar el ancho de las columnas después de agregar los datos
                    worksheet.Columns().AdjustToContents();
                    Console.WriteLine("Columnas ajustadas al contenido");

                    // Guardar el archivo Excel en un MemoryStream y devolver como FileResult
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        Console.WriteLine("Archivo Excel guardado en memoria");
                        string fileName = $"Pedido_{DateTime.Now:ddMMyyyy}.xlsx";
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                return Content($"Ocurrió un error al generar el archivo Excel: {ex.Message}\n{ex.StackTrace}");
            }
        }


        public async Task<IActionResult> ExportPedido(int IdPedido)
        {
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

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"Pedido_{IdPedido}");
                    worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;

                    //// Agregar imágenes
                    //var imagePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Empana-tica_Logo.png");
                    //var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pyme.png");
                    //if (System.IO.File.Exists(imagePath1))
                    //    worksheet.AddPicture(imagePath1).MoveTo(worksheet.Cell("A1")).Scale(0.15);
                    //if (System.IO.File.Exists(imagePath2))
                    //    worksheet.AddPicture(imagePath2).MoveTo(worksheet.Cell("G1")).Scale(0.1);

                    //// Ajustar celdas para las imágenes
                    //worksheet.Row(1).Height = 60;
                    //worksheet.Column(1).Width = 12;
                    //worksheet.Column(7).Width = 12;

                    // Título
                    var titleCell = worksheet.Cell("A2");
                    titleCell.Value = $"Detalles del Pedido #{IdPedido}";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 16;
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                    titleCell.Style.Font.FontColor = XLColor.White;
                    worksheet.Range("A2:G2").Merge();

                    // Información del pedido
                    worksheet.Cell("A3").Value = "Fecha Pedido:";
                    worksheet.Cell("B3").Value = pedido.FechaPedido.ToString("yyyy-MM-dd");
                    worksheet.Cell("A4").Value = "Fecha Entrega:";
                    worksheet.Cell("B4").Value = pedido.FechaEntrega.ToString("yyyy-MM-dd");
                    worksheet.Cell("A5").Value = "Usuario Creación:";
                    worksheet.Cell("B5").Value = pedido.UsuarioCreacion.Nombre;
                    worksheet.Cell("A6").Value = "Sucursal:";
                    worksheet.Cell("B6").Value = pedido.Sucursal.NombreSucursal;
                    worksheet.Cell("A7").Value = "Estado:";
                    worksheet.Cell("B7").Value = pedido.Estado.EstadoSolicitud;
                    worksheet.Cell("A8").Value = "Monto Total:";
                    worksheet.Cell("B8").Value = pedido.MontoTotal;
                    worksheet.Cell("B8").Style.NumberFormat.Format = "₡ #,##0.00";

                    // Cabeceras de la tabla de productos
                    var headerRow = worksheet.Row(10);
                    headerRow.Cell(1).Value = "IdProducto";
                    headerRow.Cell(2).Value = "NombreProducto";
                    headerRow.Cell(3).Value = "PrecioUnitario";
                    headerRow.Cell(4).Value = "Cantidad";
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Font.FontSize = 12;
                    headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRow.Style.Font.FontColor = XLColor.White;
                    worksheet.Range("A10:D10").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");

                    // Datos de los productos
                    int rowIdx = 11;
                    foreach (var pedidoProducto in pedido.PedidosProductos)
                    {
                        var dataRow = worksheet.Row(rowIdx);
                        dataRow.Cell(1).Value = pedidoProducto.ProductosID;
                        dataRow.Cell(2).Value = pedidoProducto.Producto.NombreProducto;
                        dataRow.Cell(3).Value = pedidoProducto.Producto.PrecioUnitario;
                        dataRow.Cell(3).Style.NumberFormat.Format = "₡ #,##0.00";
                        dataRow.Cell(4).Value = pedidoProducto.Cantidad;
                        rowIdx++;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        string fileName = $"Pedidos_{DateTime.Now:ddMMyyyy}.xlsx";
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Pedido_{IdPedido}.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error al exportar el pedido: {ex.Message}");
            }
        }




    }
}