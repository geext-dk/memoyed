using Memoyed.Application.Extensions;
using Memoyed.Domain.Users.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class UserEntityMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property<int>("DbId");
            builder.HasKey("DbId");

            builder.OwnsSingle(u => u.Id, id => id.Id);
        }
    }
}