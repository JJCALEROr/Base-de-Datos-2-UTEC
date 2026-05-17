using InventarioVentasMVC.Models.Reportes;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentasMVC.Models
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options)
            : base(options)
        {
        }

        // TABLAS
        public DbSet<Producto> Productos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Venta> Ventas { get; set; }

        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        public DbSet<Compra> Compras { get; set; }

        public DbSet<DetalleCompra> DetalleCompras { get; set; }

        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }

        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

        // VISTAS SQL
        public DbSet<VentaDetalleView> VentasDetalleView { get; set; }

        public DbSet<DetalleVentaCompletoView> DetalleVentaCompletoView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VentaDetalleView>()
                .HasNoKey()
                .ToView("vw_VentasDetalle");

            modelBuilder.Entity<DetalleVentaCompletoView>()
                .HasNoKey()
                .ToView("vw_DetalleVentasCompleto");
        }
    }
}