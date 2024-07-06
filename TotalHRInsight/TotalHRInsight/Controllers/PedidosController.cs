using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Controllers
{
    public class PedidosController : Controller
    {
        private readonly TotalHRInsightDbContext _context;

        public PedidosController(TotalHRInsightDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var totalHRInsightDbContext = _context.Pedidos.Include(p => p.Estado).Include(p => p.Sucursal).Include(p => p.UsuarioCreacion);
            return View(await totalHRInsightDbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.Sucursal)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud");
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,FechaPedido,FechaEntrega,UsuarioCreacionId,IdSucursal,IdEstado,MontoTotal")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", pedido.IdSucursal);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", pedido.UsuarioCreacionId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", pedido.IdSucursal);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", pedido.UsuarioCreacionId);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,FechaPedido,FechaEntrega,UsuarioCreacionId,IdSucursal,IdEstado,MontoTotal")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
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
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "EstadoSolicitud", pedido.IdEstado);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", pedido.IdSucursal);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Nombre", pedido.UsuarioCreacionId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.Sucursal)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
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
                    dataRow.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    dataRow.Style.Border.BottomBorderColor = XLColor.Black;
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
