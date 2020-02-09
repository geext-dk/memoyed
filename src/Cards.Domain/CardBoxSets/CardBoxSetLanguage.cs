using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxSets
{
    public class CardBoxSetLanguage : DomainValue<string>
    {
        public CardBoxSetLanguage(string value, DomainChecks.ValidateLanguage validateLanguage)
        {
            if (!validateLanguage(value))
            {
                throw new DomainException.InvalidLanguageException();
            }

            Value = value;
        }

        private CardBoxSetLanguage()
        {
        }
    }
}