namespace InventarioVentasMVC.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relación con Productos
        public ICollection<Producto> Productos { get; set; } 
    }
}

