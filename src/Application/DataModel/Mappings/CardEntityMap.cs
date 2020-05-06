using Memoyed.Application.Extensions;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class CardEntityMap : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.HasOne<CardBox>()
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.CardBoxId)
                .IsRequired(false);
            
            builder.OwnsSingle(c => c.TargetLanguageWord, word => word.Value);
            builder.OwnsSingle(c => c.NativeLanguageWord, word => word.Value);
            builder.OwnsSingle(c => c.Comment, comment => comment.Value);
        }
    }
}