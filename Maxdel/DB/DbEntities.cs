using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using Maxdel.DB;
using Maxdel.DB.Mapping;

namespace Maxdel.DB
{
    public class DbEntities : DbContext
    {
        public DbSet<Productos> Productos { get; set; }
        public DbSet<DetallePedido> detallePedidos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<TamañoPrecio> tamañoPrecios { get; set; }
        public DbSet<Tipo> tipos { get; set; }
        public DbSet<Direcciones> direcciones { get; set; }
        public DbSet<Estado> estado { get; set; }
        public DbSet<NroTracking> nroTrackings { get; set; }
        public DbSet<Boleta> boletas { get; set; }
        public DbEntities() { }

        public DbEntities(DbContextOptions<DbEntities> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductosMapping());
            modelBuilder.ApplyConfiguration(new DetallePedidoMapping());
            modelBuilder.ApplyConfiguration(new PedidoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new EstadoMapping());
            modelBuilder.ApplyConfiguration(new TamañoPrecioMapping());
            modelBuilder.ApplyConfiguration(new TipoMapping());
            modelBuilder.ApplyConfiguration(new DireccionesMapping());
            modelBuilder.ApplyConfiguration(new NroTrackingMapping());
            modelBuilder.ApplyConfiguration(new BoletaMapping());
        }
    }
}
