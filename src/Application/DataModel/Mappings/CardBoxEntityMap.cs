using Memoyed.Domain.Cards.CardBoxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class CardBoxEntityMap : IEntityTypeConfiguration<CardBox>
    {
        public void Configure(EntityTypeBuilder<CardBox> builder)
        {
            builder.HasKey("DbId");
            builder.OwnsOne(c => c.Id,
                a => a.Property(b => b.Value).HasColumnName("Id"));
            builder.OwnsOne(c => c.Level,
                a => a.Property(b => b.Value).HasColumnName("Level"));
            builder.OwnsOne(c => c.RevisionDelay,
                a => a.Property(b => b.Value).HasColumnName("RevisionDelay"));
            builder.OwnsOne(c => c.SetId,
                a => a.Property(b => b.Value).HasColumnName("SetId"));
        }
    }
}