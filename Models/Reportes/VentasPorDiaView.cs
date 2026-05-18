namespace InventarioVentasMVC.Models.Reportes
{
    public class VentasPorDiaView
    {
        public DateTime Dia { get; set; }

        public int? TotalVentas { get; set; }

        public decimal? MontoTotal { get; set; }

        public decimal? TotalDescuentos { get; set; }

        public decimal? PromedioVenta { get; set; }
    }
}