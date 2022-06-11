using Maxdel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxdel.DB.Mapping
{
    public class NroTrackingMapping : IEntityTypeConfiguration<NroTracking>
    {
        public void Configure(EntityTypeBuilder<NroTracking> builder)
        {
            builder.ToTable("NroTracking", "dbo");
            builder.HasKey(o => o.Id);
        }
    }
}
