namespace InventarioVentasMVC.Models.Reportes
{
    public class TopProductosVendidosView
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; }

        public string Producto { get; set; }

        public string Categoria { get; set; }

        public int? TotalUnidades { get; set; }

        public decimal? TotalIngresos { get; set; }

        public int? NumeroVentas { get; set; }
    }
}