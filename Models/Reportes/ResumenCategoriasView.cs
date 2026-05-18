namespace InventarioVentasMVC.Models.Reportes
{
    public class ResumenCategoriasView
    {
        public int CategoriaId { get; set; }

        public string Categoria { get; set; }

        public int? TotalProductos { get; set; }

        public int? StockTotal { get; set; }

        public decimal? ValorInventario { get; set; }

        public decimal? PrecioPromedio { get; set; }
    }
}