namespace InventarioVentasMVC.ViewModels
{
    public class CompraViewModel
    {
        public int ProveedorId { get; set; }

        public int UsuarioId { get; set; }

        public string Observaciones { get; set; }

        public List<CarritoCompraItemViewModel> Carrito { get; set; }
            = new List<CarritoCompraItemViewModel>();
    }
}