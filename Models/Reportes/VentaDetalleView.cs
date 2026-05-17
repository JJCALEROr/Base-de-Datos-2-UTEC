namespace InventarioVentasMVC.Models.Reportes
{
    public class VentaDetalleView
    {
        public int VentaId { get; set; }

        public string NumeroVenta { get; set; }

        public DateTime Fecha { get; set; }

        public int ClienteId { get; set; }

        public string Cliente { get; set; }

        public string DUI { get; set; }

        public string Vendedor { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal IVA { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; }

        public string? Observaciones { get; set; }
    }
}