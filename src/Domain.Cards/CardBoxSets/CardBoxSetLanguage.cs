using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
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