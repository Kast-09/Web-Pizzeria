using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class TamañoMapping: IEntityTypeConfiguration<Tamaño>
    {
        public void Configure(EntityTypeBuilder<Tamaño> builder)
        {
            builder.ToTable("Tamaño", "dbo");
            builder.HasKey(t => t.Id);

            builder.HasOne(o => o.Producto)
                .WithMany()
                .HasForeignKey(o => o.IdProducto);
        }
    }
}
