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

        public IActionResult ExportarExcel() { 
            DataTable dt_ProductosBajoStock = new DataTable();

            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter()) {
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM vw_ProductosBajoStock", conexion);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt_ProductosBajoStock);
                }
            }

            using (var libro = new XLWorkbook())
            {
                dt_ProductosBajoStock.TableName = "ProductosBajoStock";
                var hoja = libro.Worksheets.Add(dt_ProductosBajoStock);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {//MemoryStream para guardar el archivo Excel en memoria antes de enviarlo al cliente
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("ReporteBajoStockProductos_", DateTime.Now.ToString(), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }

        }

    } 


}