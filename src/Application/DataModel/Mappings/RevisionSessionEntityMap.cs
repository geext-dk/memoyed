using Memoyed.Domain.Cards.RevisionSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class RevisionSessionEntityMap : IEntityTypeConfiguration<RevisionSession>
    {
        public void Configure(EntityTypeBuilder<RevisionSession> builder)
        {
            builder.HasKey("DbId");
        }
    }
}