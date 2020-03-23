using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class CardComment : DomainValue
    {
        public CardComment(string? comment)
        {
            Comment = comment?.Trim();
        }
        
        public string? Comment { get; }
    }
}