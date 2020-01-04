using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBox
    {
        public CardBox(CardBoxId id, CardBoxSetId setId, CardBoxLevel level, CardBoxRepeatDelay repeatDelay)
        {
            Id = id;
            SetId = setId;
            Level = level;
            RepeatDelay = repeatDelay;
        }
        
        private readonly List<LearningCard> _learningCards = new List<LearningCard>();
        public IEnumerable<LearningCard> LearningCards => _learningCards.AsEnumerable();
        
        public CardBoxId Id { get; }
        public CardBoxSetId SetId { get; }
        public CardBoxLevel Level { get; }
        public CardBoxRepeatDelay RepeatDelay { get; }
        
        
        public void AddCard(LearningCard card)
        {
            _learningCards.Add(card);
        }

        public void RemoveCard(LearningCard card)
        {
            _learningCards.Remove(card);
        }

    }
}