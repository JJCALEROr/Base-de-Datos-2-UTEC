using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioVentasMVC.Models
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroVenta { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal IVA { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Completada";

        [StringLength(500)]
        public string Observaciones { get; set; }

        // =========================
        // RELACIONES (NAVIGATION)
        // =========================

        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
    }
}
