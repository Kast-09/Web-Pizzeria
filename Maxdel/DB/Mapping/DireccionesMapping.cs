using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class DireccionesMapping : IEntityTypeConfiguration<Direcciones>
    {
        public void Configure(EntityTypeBuilder<Direcciones> builder)
        {
            builder.ToTable("Direcciones", "dbo");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Usuario)
                .WithMany(o => o.Direcciones)
                .HasForeignKey(o => o.IdUsuario);
        }
    }
}
