using System;

namespace InventarioVentasMVC.Models
{
    public class MovimientoInventario
    {
        public int MovimientoInventarioId { get; set; }
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; } = string.Empty; // Entrada, Salida, Ajuste
        public int Cantidad { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
        public string? Referencia { get; set; } // Número de venta o compra
        public DateTime Fecha { get; set; }

        // Relaciones
        public Producto Producto { get; set; }
        public Usuario Usuario { get; set; }
    }
}
