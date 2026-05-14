namespace PruebaReportes.Models
{
    //Vista que muestra los productos que tienen un stock menor a 10 unidades
    public class ProductoBajoStock
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreProveedor { get; set; }
        public string UnidadMedida { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int UnidadesFaltantes { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Estado { get; set; }

    }
}
