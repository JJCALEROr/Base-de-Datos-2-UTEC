namespace InventarioVentasMVC.ViewModels
{
    public class CarritoItemViewModel
    {
        public int ProductoId { get; set; }

        public string Producto { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal
        {
            get { return Precio * Cantidad; }
        }
    }
}