using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class BoletaMapping : IEntityTypeConfiguration<Boleta>
    {
        public void Configure(EntityTypeBuilder<Boleta> builder)
        {
            builder.ToTable("Boleta", "dbo");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Usuario)
                .WithMany(o => o.Boletas)
                .HasForeignKey(o => o.IdUsuario);
        }
    }
}
