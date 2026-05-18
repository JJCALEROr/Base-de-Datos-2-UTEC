using InventarioVentasMVC.Models;
using InventarioVentasMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;



namespace InventarioVentasMVC.Controllers
{
    [Authorize(Roles = "Admin,Vendedor")]
    public class VentasController : Controller
    {
        private readonly InventarioContext _context;

        // Carrito temporal
        private static List<CarritoItemViewModel> carrito =
            new List<CarritoItemViewModel>();

        public VentasController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            ViewData["ClienteId"] = new SelectList(
                await _context.Clientes.ToListAsync(),
                "ClienteId",
                "Nombre"
            );

            ViewData["UsuarioId"] = new SelectList(
                await _context.Usuarios.ToListAsync(),
                "UsuarioId",
                "Username"
            );

            ViewData["Productos"] = await _context.Productos
                .Where(p => p.Activo && p.Stock > 0)
                .ToListAsync();

            var vm = new VentaViewModel
            {
                Carrito = carrito
            };

            return View(vm);
        }

        // POST: Agregar producto al carrito
        [HttpPost]
        public async Task<IActionResult> AgregarProducto(int productoId, int cantidad)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.ProductoId == productoId);

            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (cantidad <= 0)
            {
                TempData["Error"] = "Cantidad inválida.";
                return RedirectToAction(nameof(Index));
            }

            // Buscar si ya existe en carrito
            var itemExistente = carrito
                .FirstOrDefault(p => p.ProductoId == productoId);

            int cantidadActual = itemExistente != null
                ? itemExistente.Cantidad
                : 0;

            // Validar stock total
            if (producto.Stock < (cantidad + cantidadActual))
            {
                TempData["Error"] = "Stock insuficiente.";
                return RedirectToAction(nameof(Index));
            }

            // Agregar o actualizar carrito
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItemViewModel
                {
                    ProductoId = producto.ProductoId,
                    Producto = producto.Nombre,
                    Precio = producto.PrecioVenta,
                    Cantidad = cantidad
                });
            }

            TempData["Success"] = "Producto agregado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Eliminar producto del carrito
        public IActionResult EliminarProducto(int productoId)
        {
            var item = carrito
                .FirstOrDefault(p => p.ProductoId == productoId);

            if (item != null)
            {
                carrito.Remove(item);

                TempData["Success"] = "Producto eliminado del carrito.";
            }
            else
            {
                TempData["Error"] = "Producto no encontrado en el carrito.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarVenta(
    int clienteId,
    int usuarioId,
    decimal descuento = 0,
    string observaciones = ""
)
        {
            if (carrito.Count == 0)
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Convertir carrito a JSON
                var detalle = carrito.Select(c => new
                {
                    ProductoId = c.ProductoId,
                    Cantidad = c.Cantidad,
                    PrecioUnitario = c.Precio,
                    Descuento = 0
                });

                string detalleJson = JsonSerializer.Serialize(detalle);

                // Parámetros OUTPUT
                var ventaIdParam = new SqlParameter
                {
                    ParameterName = "@VentaId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                var numeroVentaParam = new SqlParameter
                {
                    ParameterName = "@NumeroVenta",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 20,
                    Direction = ParameterDirection.Output
                };

                var mensajeParam = new SqlParameter
                {
                    ParameterName = "@Mensaje",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200,
                    Direction = ParameterDirection.Output
                };

                // Ejecutar SP
                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_RegistrarVenta
                @ClienteId,
                @UsuarioId,
                @Descuento,
                @Observaciones,
                @Detalle,
                @VentaId OUTPUT,
                @NumeroVenta OUTPUT,
                @Mensaje OUTPUT",

                    new SqlParameter("@ClienteId", clienteId),
                    new SqlParameter("@UsuarioId", usuarioId),
                    new SqlParameter("@Descuento", descuento),
                    new SqlParameter("@Observaciones", observaciones ?? ""),
                    new SqlParameter("@Detalle", detalleJson),

                    ventaIdParam,
                    numeroVentaParam,
                    mensajeParam
                );

                string mensaje = mensajeParam.Value?.ToString() ?? "";

                if (mensaje.StartsWith("OK"))
                {
                    carrito.Clear();

                    TempData["Success"] =
                        $"Venta registrada correctamente. " +
                        $"Número: {numeroVentaParam.Value}";
                }
                else
                {
                    TempData["Error"] = mensaje;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> FinalizarVenta(VentaViewModel vm)
        {
            if (carrito.Count == 0)
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Convertir carrito a JSON
                var detalle = carrito.Select(c => new
                {
                    ProductoId = c.ProductoId,
                    Cantidad = c.Cantidad,
                    PrecioUnitario = c.Precio,
                    Descuento = 0
                });

                string detalleJson = JsonSerializer.Serialize(detalle);

                // Parámetros OUTPUT
                var ventaIdParam = new SqlParameter
                {
                    ParameterName = "@VentaId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                var numeroVentaParam = new SqlParameter
                {
                    ParameterName = "@NumeroVenta",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 20,
                    Direction = ParameterDirection.Output
                };

                var mensajeParam = new SqlParameter
                {
                    ParameterName = "@Mensaje",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200,
                    Direction = ParameterDirection.Output
                };

                // Ejecutar SP
                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_RegistrarVenta
                @ClienteId,
                @UsuarioId,
                @Descuento,
                @Observaciones,
                @Detalle,
                @VentaId OUTPUT,
                @NumeroVenta OUTPUT,
                @Mensaje OUTPUT",

                    new SqlParameter("@ClienteId", vm.ClienteId),
                    new SqlParameter("@UsuarioId", vm.UsuarioId),
                    new SqlParameter("@Descuento", vm.Descuento),
                    new SqlParameter("@Observaciones",
                        vm.Observaciones ?? (object)DBNull.Value),
                    new SqlParameter("@Detalle", detalleJson),

                    ventaIdParam,
                    numeroVentaParam,
                    mensajeParam
                );

                string mensaje = mensajeParam.Value?.ToString() ?? "";

                if (mensaje.StartsWith("ERROR"))
                {
                    TempData["Error"] = mensaje;
                    return RedirectToAction(nameof(Index));
                }

                carrito.Clear();

                TempData["Success"] =
                    $"Venta registrada correctamente. " +
                    $"Número: {numeroVentaParam.Value}";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Historial()
        {
            var ventas = await _context.VentasDetalleView
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            return View(ventas);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var detalle = await _context.DetalleVentaCompletoView
                .Where(d => d.NumeroVenta ==
                    _context.Ventas
                        .Where(v => v.VentaId == id)
                        .Select(v => v.NumeroVenta)
                        .FirstOrDefault())
                .ToListAsync();

            return View(detalle);
        }

        public async Task<IActionResult> GenerarPDF(int id)
        {
            var detalle = await _context.DetalleVentaCompletoView
                .Where(v => v.NumeroVenta ==
                    _context.Ventas
                        .Where(x => x.VentaId == id)
                        .Select(x => x.NumeroVenta)
                        .FirstOrDefault())
                .ToListAsync();

            return new ViewAsPdf("FacturaPDF", detalle)
            {
                FileName = $"Venta_{id}.pdf"
            };
        }
    }
}