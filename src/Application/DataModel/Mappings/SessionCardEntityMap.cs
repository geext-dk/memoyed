using Memoyed.Application.Extensions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class SessionCardEntityMap : IEntityTypeConfiguration<SessionCard>
    {
        public void Configure(EntityTypeBuilder<SessionCard> builder)
        {
            builder.Property<int>("DbId");
            builder.HasKey("DbId");
            
            builder.OwnsSingle(c => c.SessionId, id => id.Value);
            builder.OwnsSingle(c => c.CardId, id => id.Value);
            builder.OwnsSingle(c => c.NativeLanguageWord, w => w.Value);
            builder.OwnsSingle(c => c.TargetLanguageWord, w => w.Value);
        }
    }
}