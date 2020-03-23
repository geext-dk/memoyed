using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class CardWord : DomainValue
    {
        public CardWord(string? word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new DomainException.EmptyWordException();
            }

            Word = word;
        }
        
        public string Word { get; }
    }
}