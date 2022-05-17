using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using Maxdel.DB;
using Maxdel.DB.Mapping;

namespace Maxdel.DB
{
    public class DbEntities : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Tamaño> tamaños { get; set; }
        public DbSet<Precio> precios { get; set; }
        public DbSet<DetallePedido> detallePedidos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Usuario> usuarios { get; set; }

        public DbEntities(DbContextOptions<DbEntities> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductoMapping());
            modelBuilder.ApplyConfiguration(new TamañoMapping());
            modelBuilder.ApplyConfiguration(new PrecioMapping());
            modelBuilder.ApplyConfiguration(new DetallePedidoMapping());
            modelBuilder.ApplyConfiguration(new PedidoMapping());
            modelBuilder.ApplyConfiguration(new ClienteMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new EstadoMapping());
        }
    }
}
