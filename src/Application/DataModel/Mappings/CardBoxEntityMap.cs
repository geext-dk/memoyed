using Memoyed.Application.Extensions;
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
            builder.OwnsSingle(c => c.Id, id => id.Value);
            builder.OwnsSingle(c => c.Level, l => l.Value);
            builder.OwnsSingle(c => c.RevisionDelay, d => d.Value);
            builder.OwnsSingle(c => c.SetId, id => id.Value);
        }
    }
}