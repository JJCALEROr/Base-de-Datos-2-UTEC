using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PruebaReportes.Controllers
{
    public class ReportesController : Controller
    {
        public readonly string _connectionString;

        public ReportesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GestorInventario");
        }

        // ============================================================
        // VISTA 1: Productos con stock bajo el mínimo
        // ============================================================
        public IActionResult vw_ProductosBajoStock()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ProductosBajoStock", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ProductosBajoStock";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ProductosBajoStock_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 2: Resumen de ventas por mes y año
        // ============================================================
        public IActionResult vw_VentasPorMes()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_VentasPorMes", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "VentasPorMes";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_VentasPorMes_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 3: Productos más vendidos
        // ============================================================
        public IActionResult vw_ProductosMasVendidos()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ProductosMasVendidos", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ProductosMasVendidos";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ProductosMasVendidos_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 4: Rentabilidad por producto
        // ============================================================
        public IActionResult vw_RentabilidadProducto()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_RentabilidadProducto", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "RentabilidadProducto";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_RentabilidadProducto_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 5: Compras por proveedor y período
        // ============================================================
        public IActionResult vw_ComprasPorProveedor()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ComprasPorProveedor", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ComprasPorProveedor";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ComprasPorProveedor_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 6: Movimientos de inventario con detalle
        // ============================================================
        public IActionResult vw_MovimientosInventario()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_MovimientosInventario", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "MovimientosInventario";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_MovimientosInventario_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 7: Productos próximos a caducar
        // ============================================================
        public IActionResult vw_ProductosProximosCaducar()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ProductosProximosCaducar", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ProductosProximosCaducar";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ProductosProximosCaducar_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 8: Actividad de usuarios
        // ============================================================
        public IActionResult vw_ActividadUsuarios()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ActividadUsuarios", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ActividadUsuarios";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ActividadUsuarios_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 9: Valorización del inventario por categoría
        // ============================================================
        public IActionResult vw_ValorizacionInventario()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ValorizacionInventario", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ValorizacionInventario";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ValorizacionInventario_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        // ============================================================
        // VISTA 10: Comparativo compras vs ventas por producto
        // ============================================================
        public IActionResult vw_ComparativoCompraVenta()
        {
            DataTable dt = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ComparativoCompraVenta", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt.TableName = "ComparativoCompraVenta";
                var hoja = libro.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("vw_ComparativoCompraVenta_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }
    }
}