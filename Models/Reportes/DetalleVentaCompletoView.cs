namespace InventarioVentasMVC.Models.Reportes
{
    public class DetalleVentaCompletoView
    {
        public int DetalleId { get; set; }

        public string NumeroVenta { get; set; }

        public DateTime Fecha { get; set; }

        public string Cliente { get; set; }

        public string CodigoProducto { get; set; }

        public string Producto { get; set; }

        public string Categoria { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal Subtotal { get; set; }

        public string Estado { get; set; }
    }
}