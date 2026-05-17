using System.ComponentModel.DataAnnotations;

namespace InventarioVentasMVC.Models
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria")]
        [StringLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El NIT es obligatorio")]
        [StringLength(20)]
        public string NIT { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Correo inválido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(250)]
        public string? Direccion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
