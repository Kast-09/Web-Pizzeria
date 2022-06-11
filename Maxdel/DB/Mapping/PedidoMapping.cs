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

            builder.HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(o => o.IdUsuario);

            builder.HasOne(o => o.EstadoFK)
                .WithMany()
                .HasForeignKey(o => o.Estado);

            builder.HasOne(o => o.boleta)
                .WithMany()
                .HasForeignKey(o => o.IdBoleta);
        }
    }
}
