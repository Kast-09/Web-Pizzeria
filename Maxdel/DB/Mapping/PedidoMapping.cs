using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class PedidoMapping: IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido", "dbo");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Cliente)
                .WithMany()
                .HasForeignKey(o => o.IdCliente);

            builder.HasOne(o => o.EstadoFK)
                .WithMany()
                .HasForeignKey(o => o.Estado);
        }
    }
}
