using System.ComponentModel.DataAnnotations;

namespace InventarioVentasMVC.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El username es obligatorio")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [StringLength(30)]
        public string Rol { get; set; } = "Vendedor";

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();

        public ICollection<MovimientoInventario> MovimientosInventario { get; set; } = new List<MovimientoInventario>();
    }
}
