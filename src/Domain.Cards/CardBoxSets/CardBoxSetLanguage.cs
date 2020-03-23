using System;
using System.Buffers.Text;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetLanguage : DomainValue
    {
        public CardBoxSetLanguage(string language, DomainChecks.ValidateLanguage validateLanguage)
        {
            if (!validateLanguage(language))
            {
                throw new DomainException.InvalidLanguageException();
            }

            Language = language;
        }
        
        public string Language { get; }

        // ReSharper disable once UnusedMember.Local
        private CardBoxSetLanguage()
        {
            Language = null!;
        }
    }
}