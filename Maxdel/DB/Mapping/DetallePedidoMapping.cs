using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class DetallePedidoMapping: IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.ToTable("DetallePedido", "dbo");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Pedido)
                .WithMany()
                .HasForeignKey(o => o.IdPedido);

            builder.HasOne(o => o.Producto)
                .WithMany()
                .HasForeignKey(o => o.IdProducto);

            builder.HasOne(o => o.Tamaño)
                .WithMany()
                .HasForeignKey(o => o.IdTamaño);

            builder.HasOne(o => o.Precio)
                .WithMany()
                .HasForeignKey(o => o.IdPrecio);
        }
    }
}
