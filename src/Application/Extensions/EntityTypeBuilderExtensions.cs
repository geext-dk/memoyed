using System;
using System.Linq.Expressions;
using System.Reflection;
using Memoyed.DomainFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void OwnsSingle<T, TValue, TInner>(this EntityTypeBuilder<T> builder,
            Expression<Func<T, TValue>> propertyAccess,
            Expression<Func<TValue, TInner>> valueAccess)
            where T : class
            where TValue : DomainValue<TInner>
        {
            if (!(propertyAccess.Body is MemberExpression memberExpression) ||
                memberExpression.Member.MemberType != MemberTypes.Property)
                throw new InvalidOperationException("The given expression is not a member expression " +
                                                    "or the member is not a property");

            builder.OwnsOne(propertyAccess,
                opt => opt.Property(valueAccess).HasColumnName(memberExpression.Member.Name.ToSnakeCase()));
        }
    }
}