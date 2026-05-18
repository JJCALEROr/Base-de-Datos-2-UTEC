using InventarioVentasMVC.Models.Reportes;

namespace InventarioVentasMVC.ViewModels
{
    public class HomeDashboardViewModel
    {
        // KPIs
        public decimal VentasHoy { get; set; }

        public int TotalVentasHoy { get; set; }

        public int ProductosStockBajo { get; set; }

        public int TotalProductos { get; set; }

        // TABLAS
        public List<ProductosStockBajoView> StockBajo { get; set; }
            = new();

        public List<TopProductosVendidosView> TopProductos { get; set; }
            = new();

        public List<ResumenCategoriasView> ResumenCategorias { get; set; }
            = new();
    }
}