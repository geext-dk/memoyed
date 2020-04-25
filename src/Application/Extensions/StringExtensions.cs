using System.Linq;

namespace Memoyed.Application.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            return str.Trim() == ""
                ? str
                : string.Concat(
                    str.Select((c, i) => char.IsUpper(c) ? (i == 0 ? "" : "_") + char.ToLower(c) : c.ToString()));
        }
    }
}