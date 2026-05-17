namespace InventarioVentasMVC.ViewModels
{
    public class CarritoCompraItemViewModel
    {
        public int ProductoId { get; set; }

        public string Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal Subtotal
        {
            get { return Cantidad * PrecioCompra; }
        }
    }
}
