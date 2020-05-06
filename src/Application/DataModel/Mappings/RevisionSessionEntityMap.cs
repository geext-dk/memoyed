using Memoyed.Domain.Cards.RevisionSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class RevisionSessionEntityMap : IEntityTypeConfiguration<RevisionSession>
    {
        public void Configure(EntityTypeBuilder<RevisionSession> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.HasMany(s => s.SessionCards)
                .WithOne()
                .HasForeignKey(s => s.SessionId)
                .IsRequired();
            builder.Property(s => s.CardBoxSetId);
        }
    }
}