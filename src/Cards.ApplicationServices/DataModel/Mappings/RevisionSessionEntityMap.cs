using Memoyed.Cards.Domain.RevisionSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Cards.ApplicationServices.DataModel.Mappings
{
    public class RevisionSessionEntityMap : IEntityTypeConfiguration<RevisionSession>
    {
        public void Configure(EntityTypeBuilder<RevisionSession> builder)
        {
            builder.HasKey("DbId");
        }
    }
}