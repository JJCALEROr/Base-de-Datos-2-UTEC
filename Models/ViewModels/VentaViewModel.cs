using InventarioVentasMVC.Models;

namespace InventarioVentasMVC.ViewModels
{
    public class VentaViewModel
    {
        public Venta Venta { get; set; } = new Venta();

        public int ClienteId { get; set; }

        public int UsuarioId { get; set; }

        public decimal Descuento { get; set; }

        public string? Observaciones { get; set; }

        public List<CarritoItemViewModel> Carrito { get; set; }
            = new List<CarritoItemViewModel>();
    }
}
