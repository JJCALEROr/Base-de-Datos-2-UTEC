using System;
using System.Collections.Generic;

namespace InventarioVentasMVC.Models
{
    public class Compra
    {
        public int CompraId { get; set; }
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        // Relaciones
        public Proveedor Proveedor { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();
    }
}
