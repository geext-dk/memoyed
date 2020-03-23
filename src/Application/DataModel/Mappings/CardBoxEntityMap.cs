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
            builder.OwnsSingle(c => c.Id, id => id.Id);
            builder.OwnsSingle(c => c.Level, l => l.Level);
            builder.OwnsSingle(c => c.RevisionDelay, d => d.Delay);
            builder.OwnsSingle(c => c.SetId, id => id.Id);
        }
    }
}