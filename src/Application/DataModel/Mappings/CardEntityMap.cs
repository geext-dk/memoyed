using Memoyed.Application.Extensions;
using Memoyed.Domain.Cards.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class CardEntityMap : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey("DbId");

            builder.OwnsSingle(c => c.Id, id => id.Value);
            builder.OwnsSingle(c => c.TargetLanguageWord, id => id.Value);
            builder.OwnsSingle(c => c.NativeLanguageWord, id => id.Value);
            builder.OwnsSingle(c => c.Comment, id => id.Value);
            builder.OwnsSingle(c => c.CardBoxId, id => id.Value);
            builder.OwnsSingle(c => c.CardBoxChangedDate, id => id.Value);
        }
    }
}