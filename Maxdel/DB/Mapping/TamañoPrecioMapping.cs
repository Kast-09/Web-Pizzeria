using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class TamañoPrecioMapping : IEntityTypeConfiguration<TamañoPrecio>
    {
        public void Configure(EntityTypeBuilder<TamañoPrecio> builder)
        {
            builder.ToTable("TamañoPrecio", "dbo");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Productos)
                .WithMany()
                .HasForeignKey(o => o.IdProducto);
        }
    }
}
