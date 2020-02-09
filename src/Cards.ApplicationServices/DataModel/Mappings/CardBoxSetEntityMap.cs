using Memoyed.Cards.Domain.CardBoxSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Cards.ApplicationServices.DataModel.Mappings
{
    public class CardBoxSetEntityMap : IEntityTypeConfiguration<CardBoxSet>
    {
        public void Configure(EntityTypeBuilder<CardBoxSet> builder)
        {
            builder.HasKey("DbId");
            builder.OwnsOne(c => c.Id,
                a => a.Property(s => s.Value).HasColumnName("Id"));
            builder.OwnsOne(c => c.Name,
                a => a.Property(s => s.Value).HasColumnName("Name"));
            builder.OwnsOne(c => c.NativeLanguage,
                a => a.Property(s => s.Value).HasColumnName("NativeLanguage"));
            builder.OwnsOne(c => c.TargetLanguage,
                a => a.Property(s => s.Value).HasColumnName("TargetLanguage"));
            builder.OwnsMany(s => s.CompletedRevisionSessionIds);
        }
    }
}