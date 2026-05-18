namespace InventarioVentasMVC.Models.Reportes
{
    public class ProductosStockBajoView
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Categoria { get; set; }

        public string Proveedor { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public int CantidadFaltante { get; set; }
    }
}