using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class UsuarioMapping: IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario", "dbo");
            builder.HasKey(x => x.Id);

            builder.HasOne(o => o.Tipo)
                .WithMany()
                .HasForeignKey(o => o.IdTipo);

            builder.HasOne(o => o.PreguntaSeguridad)
                .WithMany()
                .HasForeignKey(o => o.IdPreguntaSeguridad);
        }
    }
}
