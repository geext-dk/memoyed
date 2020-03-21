using System;
using System.Data;
using Dapper;

namespace Memoyed.Cards.ApplicationServices.Extensions
{
    public static class SqlMapperExtensions
    {
        public class GuidSqlHandler : SqlMapper.TypeHandler<Guid>
        {
            public override void SetValue(IDbDataParameter parameter, Guid value)
            {
                parameter.Value = value;
            }

            public override Guid Parse(object value)
            {
                return Guid.Parse((string) value);
            }
        }
        
    }
}