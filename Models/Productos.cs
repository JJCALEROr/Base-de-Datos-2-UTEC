namespace InventarioVentasMVC.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }

        public int CategoriaId { get; set; }

        public int ProveedorId { get; set; }

        public string? Codigo { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public Categoria? Categoria { get; set; }

        public Proveedor? Proveedor { get; set; }

        public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();

        public ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

        public ICollection<MovimientoInventario> MovimientosInventario { get; set; } = new List<MovimientoInventario>();
    }
}