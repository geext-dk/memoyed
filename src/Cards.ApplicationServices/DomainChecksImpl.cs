using Memoyed.Cards.Domain;

namespace Memoyed.Cards.ApplicationServices
{
    public static class DomainChecksImpl
    {
        public static readonly DomainChecks.ValidateLanguage ValidateLanguage = language =>
        {
            return language switch
            {
                "Russian" => true,
                "English" => true,
                "Norwegian" => true,
                _ => false
            };
        };
    }
}