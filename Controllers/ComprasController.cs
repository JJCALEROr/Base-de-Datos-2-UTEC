using InventarioVentasMVC.Models;
using InventarioVentasMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace InventarioVentasMVC.Controllers
{
    public class ComprasController : Controller
    {
        private readonly InventarioContext _context;

        private static List<CarritoCompraItemViewModel> carrito =
            new List<CarritoCompraItemViewModel>();

        public ComprasController(InventarioContext context)
        {
            _context = context;
        }

        // =====================================================
        // INDEX
        // =====================================================

        public async Task<IActionResult> Index()
        {
            ViewData["ProveedorId"] = new SelectList(
                await _context.Proveedores
                    .Where(p => p.Activo)
                    .ToListAsync(),
                "ProveedorId",
                "RazonSocial"
            );

            ViewData["UsuarioId"] = new SelectList(
                await _context.Usuarios
                    .Where(u => u.Activo)
                    .ToListAsync(),
                "UsuarioId",
                "Username"
            );

            ViewData["Productos"] = await _context.Productos
                .Where(p => p.Activo)
                .ToListAsync();

            var vm = new CompraViewModel
            {
                Carrito = carrito
            };

            return View(vm);
        }

        // =====================================================
        // PRODUCTOS POR PROVEEDOR
        // =====================================================

        [HttpGet]
        public async Task<JsonResult> ObtenerProductosPorProveedor(int proveedorId)
        {
            var productos = await _context.Productos
                .Where(p =>
                    p.Activo &&
                    p.ProveedorId == proveedorId)
                .Select(p => new
                {
                    productoId = p.ProductoId,
                    nombre = p.Nombre,
                    stock = p.Stock,
                    precioCompra = p.PrecioCompra
                })
                .ToListAsync();

            return Json(productos);
        }

        // =====================================================
        // AGREGAR PRODUCTO
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> AgregarProducto(
            int productoId,
            int cantidad,
            decimal precioCompra)
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

            if (precioCompra <= 0)
            {
                TempData["Error"] = "Precio inválido.";
                return RedirectToAction(nameof(Index));
            }

            var itemExistente = carrito
                .FirstOrDefault(p => p.ProductoId == productoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
                itemExistente.PrecioCompra = precioCompra;
            }
            else
            {
                carrito.Add(new CarritoCompraItemViewModel
                {
                    ProductoId = producto.ProductoId,
                    Producto = producto.Nombre,
                    Cantidad = cantidad,
                    PrecioCompra = precioCompra
                });
            }

            TempData["Success"] = "Producto agregado.";

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // ELIMINAR PRODUCTO
        // =====================================================

        public IActionResult EliminarProducto(int productoId)
        {
            var item = carrito
                .FirstOrDefault(p => p.ProductoId == productoId);

            if (item != null)
            {
                carrito.Remove(item);
            }

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // GUARDAR COMPRA
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> GuardarCompra(
            CompraViewModel vm)
        {
            if (!carrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var detalle = carrito.Select(c => new
                {
                    ProductoId = c.ProductoId,
                    Cantidad = c.Cantidad,
                    PrecioUnitario = c.PrecioCompra
                });

                string detalleJson =
                    JsonSerializer.Serialize(detalle);

                var proveedorParam =
                    new SqlParameter("@ProveedorId", vm.ProveedorId);

                var usuarioParam =
                    new SqlParameter("@UsuarioId", vm.UsuarioId);

                var observacionesParam =
                    new SqlParameter(
                        "@Observaciones",
                        vm.Observaciones ?? (object)DBNull.Value
                    );

                var detalleParam =
                    new SqlParameter("@Detalle", detalleJson);

                var compraIdParam =
                    new SqlParameter("@CompraId",
                        System.Data.SqlDbType.Int)
                    {
                        Direction =
                            System.Data.ParameterDirection.Output
                    };

                var mensajeParam =
                    new SqlParameter("@Mensaje",
                        System.Data.SqlDbType.NVarChar, 200)
                    {
                        Direction =
                            System.Data.ParameterDirection.Output
                    };

                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_RegistrarCompra
                        @ProveedorId,
                        @UsuarioId,
                        @Observaciones,
                        @Detalle,
                        @CompraId OUTPUT,
                        @Mensaje OUTPUT",
                    proveedorParam,
                    usuarioParam,
                    observacionesParam,
                    detalleParam,
                    compraIdParam,
                    mensajeParam
                );

                string mensaje =
                    mensajeParam.Value?.ToString() ?? "";

                if (mensaje.StartsWith("OK"))
                {
                    carrito.Clear();

                    TempData["Success"] = mensaje;

                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = mensaje;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}