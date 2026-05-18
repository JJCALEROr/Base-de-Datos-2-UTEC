using InventarioVentasMVC.Models;
using InventarioVentasMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InventarioVentasMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly InventarioContext _context;

        public HomeController(InventarioContext context)
        {
            _context = context;
        }

        // =====================================================
        // DASHBOARD
        // =====================================================

        public async Task<IActionResult> Index()
        {
            // =========================================
            // DASHBOARD ADMIN
            // =========================================

            if (User.IsInRole("Admin"))
            {
                var hoy = DateTime.Today;

                // =========================================
                // VENTAS DEL DÍA
                // =========================================

                var ventasHoy = await _context.VentasPorDiaView
                    .Where(v => v.Dia == hoy)
                    .FirstOrDefaultAsync();

                // =========================================
                // STOCK BAJO
                // =========================================

                var stockBajo = await _context.ProductosStockBajoView
                    .OrderBy(p => p.Stock)
                    .Take(5)
                    .ToListAsync();

                // =========================================
                // TOP PRODUCTOS
                // =========================================

                var topProductos = await _context.TopProductosVendidosView
                    .OrderByDescending(p => p.TotalUnidades)
                    .Take(5)
                    .ToListAsync();

                // =========================================
                // RESUMEN CATEGORÍAS
                // =========================================

                var resumenCategorias = await _context.ResumenCategoriasView
                    .OrderByDescending(c => c.ValorInventario)
                    .ToListAsync();

                // =========================================
                // TOTAL PRODUCTOS
                // =========================================

                var totalProductos = await _context.Productos
                    .CountAsync();

                // =========================================
                // VIEWMODEL
                // =========================================

                var vm = new HomeDashboardViewModel
                {
                    VentasHoy = ventasHoy?.MontoTotal ?? 0,

                    TotalVentasHoy = ventasHoy?.TotalVentas ?? 0,

                    ProductosStockBajo = stockBajo.Count,

                    TotalProductos = totalProductos,

                    StockBajo = stockBajo,

                    TopProductos = topProductos,

                    ResumenCategorias = resumenCategorias
                };

                return View(vm);
            }

            // =========================================
            // DASHBOARD VENDEDOR
            // =========================================

            if (User.IsInRole("Vendedor"))
            {
                ViewBag.Mensaje =
                    "Bienvenido al módulo de ventas.";

                return View("DashboardVendedor");
            }

            // =========================================
            // DASHBOARD BODEGUERO
            // =========================================

            if (User.IsInRole("Bodeguero"))
            {
                var stockBajo = await _context.ProductosStockBajoView
                    .OrderBy(p => p.Stock)
                    .Take(10)
                    .ToListAsync();

                return View("DashboardBodeguero", stockBajo);
            }

            // =========================================
            // FALLBACK
            // =========================================

            return View();
        }

        // =====================================================
        // PRIVACY
        // =====================================================

        public IActionResult Privacy()
        {
            return View();
        }

        // =====================================================
        // ERROR
        // =====================================================

        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]

        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}