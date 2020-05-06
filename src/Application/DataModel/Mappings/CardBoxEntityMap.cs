using Memoyed.Application.Extensions;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.CardBoxSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class CardBoxEntityMap : IEntityTypeConfiguration<CardBox>
    {
        public void Configure(EntityTypeBuilder<CardBox> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.HasOne<CardBoxSet>()
                .WithMany()
                .HasForeignKey(b => b.SetId)
                .IsRequired();
            builder.HasMany(c => c.Cards)
                .WithOne()
                .HasForeignKey(c => c.CardBoxId);

            builder.OwnsSingle(c => c.Level, l => l.Value);
            builder.OwnsSingle(c => c.RevisionDelay, d => d.Value);
        }
    }
}