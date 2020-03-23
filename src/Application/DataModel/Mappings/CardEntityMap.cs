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

            builder.OwnsSingle(c => c.Id, id => id.Id);
            builder.OwnsSingle(c => c.TargetLanguageWord, word => word.Word);
            builder.OwnsSingle(c => c.NativeLanguageWord, word => word.Word);
            builder.OwnsSingle(c => c.Comment, comment => comment.Comment);
            builder.OwnsSingle(c => c.CardBoxId, id => id.Id);
            builder.OwnsSingle(c => c.CardBoxChangedDate, date => date.Time);
        }
    }
}