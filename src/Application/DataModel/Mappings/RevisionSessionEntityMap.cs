using Memoyed.Application.Extensions;
using Memoyed.Domain.Cards.RevisionSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class RevisionSessionEntityMap : IEntityTypeConfiguration<RevisionSession>
    {
        public void Configure(EntityTypeBuilder<RevisionSession> builder)
        {
            builder.Property<int>("DbId");
            builder.HasKey("DbId");

            builder.OwnsSingle(c => c.Id, id => id.Value);
            builder.OwnsSingle(c => c.CardBoxSetId, id => id.Value);
        }
    }
}