using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxSets
{
    public class CardBoxSetLanguage : DomainValue<string>
    {
        public CardBoxSetLanguage(string language, DomainChecks.ValidateLanguage validateLanguage)
        {
            if (!validateLanguage(language))
            {
                throw new DomainException.InvalidLanguageException();
            }

            Value = language;
        }
    }
}