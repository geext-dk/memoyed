using Memoyed.Application.Extensions;
using Memoyed.Domain.Cards.CardBoxSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class CardBoxSetEntityMap : IEntityTypeConfiguration<CardBoxSet>
    {
        public void Configure(EntityTypeBuilder<CardBoxSet> builder)
        {
            builder.Property<int>("DbId");
            builder.HasKey("DbId");

            builder.OwnsSingle(c => c.Id, id => id.Value);
            builder.OwnsSingle(c => c.CurrentRevisionSessionId, id => id.Value);
            builder.OwnsSingle(c => c.Name, n => n.Value);
            builder.OwnsSingle(c => c.NativeLanguage, l => l.Value);
            builder.OwnsSingle(c => c.TargetLanguage, l => l.Value);
            builder.OwnsMany(s => s.CompletedRevisionSessionIds,
                b => b.ToTable("card_box_sets_completed_revision_session_ids"));
        }
    }
}