using System;

namespace InventarioVentasMVC.Models
{
    public class DetalleCompra
    {
        public int DetalleId { get; set; }

        public int CompraId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        // Relaciones

        public Compra Compra { get; set; }

        public Producto Producto { get; set; }
    }
}