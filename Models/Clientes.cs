using System;
using System.Collections.Generic;

namespace InventarioVentasMVC.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relación con Ventas
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
