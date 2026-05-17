namespace InventarioVentasMVC.Models.Reportes
{
    public class DashboardViewModel
    {
        public int VentasHoy { get; set; }

        public decimal IngresoHoy { get; set; }

        public int ProductosStockBajo { get; set; }

        public int TotalProductos { get; set; }

        public int TotalClientes { get; set; }
    }
}
