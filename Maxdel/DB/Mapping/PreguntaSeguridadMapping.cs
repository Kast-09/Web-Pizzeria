using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class PreguntaSeguridadMapping : IEntityTypeConfiguration<PreguntaSeguridad>
    {
        public void Configure(EntityTypeBuilder<PreguntaSeguridad> builder)
        {
            builder.ToTable("PreguntaSeguridad", "dbo");
            builder.HasKey(o => o.Id);
        }
    }
}
