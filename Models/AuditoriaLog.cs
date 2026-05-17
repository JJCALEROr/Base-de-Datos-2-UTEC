using System;

namespace InventarioVentasMVC.Models
{
    public class AuditoriaLog
    {
        public int AuditoriaLogId { get; set; }
        public string Entidad { get; set; } = string.Empty; // Ej: Producto, Cliente, Venta
        public string Accion { get; set; } = string.Empty; // Insert, Update, Delete
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? Detalles { get; set; } // Información adicional del cambio
    }
}
