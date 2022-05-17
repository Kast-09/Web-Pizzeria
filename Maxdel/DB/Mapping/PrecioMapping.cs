using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class PrecioMapping: IEntityTypeConfiguration<Precio>
    {
        public void Configure(EntityTypeBuilder<Precio> builder)
        {
            builder.ToTable("Precio", "dbo");
            builder.HasKey(x => x.Id);

            builder.HasOne(o => o.Producto)
                .WithMany()
                .HasForeignKey(o => o.IdProducto);

            builder.HasOne(o => o.Tamaño)
                .WithMany()
                .HasForeignKey(o => o.IdTamaño);
        }
    }
}
