using InventarioVentasMVC.Models;
using InventarioVentasMVC.Models.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



namespace InventarioVentasMVC.Controllers
{
    [Authorize(Roles = "Admin,Bodeguero")]
    public class DashboardController : Controller
    {

        private readonly InventarioContext _context;


        public DashboardController(InventarioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboard = new();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_DashboardHoy";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dashboard.VentasHoy =
                                reader["VentasHoy"] != DBNull.Value
                                ? Convert.ToInt32(reader["VentasHoy"])
                                : 0;

                            dashboard.IngresoHoy =
                                reader["IngresoHoy"] != DBNull.Value
                                ? Convert.ToDecimal(reader["IngresoHoy"])
                                : 0;

                            dashboard.ProductosStockBajo =
                                reader["ProductosStockBajo"] != DBNull.Value
                                ? Convert.ToInt32(reader["ProductosStockBajo"])
                                : 0;

                            dashboard.TotalProductos =
                                reader["TotalProductos"] != DBNull.Value
                                ? Convert.ToInt32(reader["TotalProductos"])
                                : 0;

                            dashboard.TotalClientes =
                                reader["TotalClientes"] != DBNull.Value
                                ? Convert.ToInt32(reader["TotalClientes"])
                                : 0;
                        }
                    }
                }
            }

            return View(dashboard);
        }
    }
}
