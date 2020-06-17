using Memoyed.Application.EntityFramework.Extensions;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.EntityFramework.Mappings
{
    public class SessionCardEntityMap : IEntityTypeConfiguration<SessionCard>
    {
        public void Configure(EntityTypeBuilder<SessionCard> builder)
        {
            builder.HasKey(c => new {c.SessionId, c.CardId});
            builder.Property(c => c.SessionId).ValueGeneratedNever();
            builder.Property(c => c.CardId).ValueGeneratedNever();
            
            builder.HasOne<RevisionSession>()
                .WithMany(s => s.SessionCards)
                .HasForeignKey(c => c.SessionId);

            builder.OwnsSingle(c => c.NativeLanguageWord, w => w.Value);
            builder.OwnsSingle(c => c.TargetLanguageWord, w => w.Value);
        }
    }
}