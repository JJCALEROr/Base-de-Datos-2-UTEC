using ClosedXML.Excel;
using InventarioVentasMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InventarioVentasMVC.Controllers
{
    public class ReportesController : Controller
    {
        private readonly InventarioContext _context;

        public ReportesController(InventarioContext context)
        {
            _context = context;
        }

        // ── Método auxiliar: ejecuta una vista SQL y devuelve un DataTable ──
        private DataTable EjecutarVista(string nombreVista)
        {
            var dt = new DataTable();
            var conexion = _context.Database.GetDbConnection();

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = $"SELECT * FROM {nombreVista}";
                comando.CommandType = CommandType.Text;

                using var reader = comando.ExecuteReader();
                dt.Load(reader);
            }
            finally
            {
                // EF gestiona el ciclo de vida de la conexión;
                // solo la cerramos si nosotros la abrimos.
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return dt;
        }

        // ── Método auxiliar: convierte un DataTable en un archivo .xlsx ──
        private FileContentResult GenerarExcel(DataTable dt, string nombreVista)
        {
            dt.TableName = nombreVista;

            using var libro = new XLWorkbook();
            var hoja = libro.Worksheets.Add(dt);
            hoja.ColumnsUsed().AdjustToContents();

            using var memoria = new MemoryStream();
            libro.SaveAs(memoria);

            var nombreArchivo = $"{nombreVista}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(
                memoria.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nombreArchivo
            );
        }

        // ============================================================
        // VISTA 1: Productos con stock bajo el mínimo
        // ============================================================
        public IActionResult vw_ProductosBajoStock()
        {
            var dt = EjecutarVista("vw_ProductosBajoStock");
            return GenerarExcel(dt, "ProductosBajoStock");
        }

        // ============================================================
        // VISTA 2: Resumen de ventas por mes y año
        // ============================================================
        public IActionResult vw_VentasPorMes()
        {
            var dt = EjecutarVista("vw_VentasPorMes");
            return GenerarExcel(dt, "VentasPorMes");
        }

        // ============================================================
        // VISTA 3: Productos más vendidos
        // ============================================================
        public IActionResult vw_ProductosMasVendidos()
        {
            var dt = EjecutarVista("vw_ProductosMasVendidos");
            return GenerarExcel(dt, "ProductosMasVendidos");
        }

        // ============================================================
        // VISTA 4: Rentabilidad por producto
        // ============================================================
        public IActionResult vw_RentabilidadProducto()
        {
            var dt = EjecutarVista("vw_RentabilidadProducto");
            return GenerarExcel(dt, "RentabilidadProducto");
        }

        // ============================================================
        // VISTA 5: Compras por proveedor y período
        // ============================================================
        public IActionResult vw_ComprasPorProveedor()
        {
            var dt = EjecutarVista("vw_ComprasPorProveedor");
            return GenerarExcel(dt, "ComprasPorProveedor");
        }

        // ============================================================
        // VISTA 6: Movimientos de inventario con detalle
        // ============================================================
        public IActionResult vw_MovimientosInventario()
        {
            var dt = EjecutarVista("vw_MovimientosInventario");
            return GenerarExcel(dt, "MovimientosInventario");
        }

        // ============================================================
        // VISTA 7: Productos sin movimiento de inventario
        // ============================================================
        public IActionResult vw_ProductosSinMovimiento()
        {
            var dt = EjecutarVista("vw_ProductosSinMovimiento");
            return GenerarExcel(dt, "ProductosSinMovimiento");
        }

        // ============================================================
        // VISTA 8: Actividad de usuarios
        // ============================================================
        public IActionResult vw_ActividadUsuarios()
        {
            var dt = EjecutarVista("vw_ActividadUsuarios");
            return GenerarExcel(dt, "ActividadUsuarios");
        }

        // ============================================================
        // VISTA 9: Valorización del inventario por categoría
        // ============================================================
        public IActionResult vw_ValorizacionInventario()
        {
            var dt = EjecutarVista("vw_ValorizacionInventario");
            return GenerarExcel(dt, "ValorizacionInventario");
        }

        // ============================================================
        // VISTA 10: Comparativo compras vs ventas por producto
        // ============================================================
        public IActionResult vw_ComparativoCompraVenta()
        {
            var dt = EjecutarVista("vw_ComparativoCompraVenta");
            return GenerarExcel(dt, "ComparativoCompraVenta");
        }

        // ============================================================
        // TABLA: Usuarios
        // ============================================================
        public IActionResult ExportarUsuarios()
        {
            var dt = EjecutarVista("Usuarios");
            return GenerarExcel(dt, "Usuarios");
        }

        // ============================================================
        // TABLA: Productos
        // ============================================================
        public IActionResult ExportarProductos()
        {
            var dt = EjecutarVista("Productos");
            return GenerarExcel(dt, "Productos");
        }

    }
}

