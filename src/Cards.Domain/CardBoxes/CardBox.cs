using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBox
    {
        public CardBox(CardBoxId id, CardBoxSetId setId, CardBoxLevel level, CardBoxRevisionDelay revisionDelay)
        {
            Id = id;
            SetId = setId;
            Level = level;
            RevisionDelay = revisionDelay;
        }
        
        private readonly List<LearningCard> _learningCards = new List<LearningCard>();
        public IEnumerable<LearningCard> LearningCards => _learningCards.AsEnumerable();
        
        /// <summary>
        /// Id of the card box
        /// </summary>
        public CardBoxId Id { get; }
        
        /// <summary>
        /// Id of the card box set which owns the card box
        /// </summary>
        public CardBoxSetId SetId { get; }
        
        /// <summary>
        /// Level of the card box
        /// </summary>
        public CardBoxLevel Level { get; }
        
        /// <summary>
        /// Days until revision of the contained cards
        /// Counts starts after the card is moved to the box
        /// </summary>
        public CardBoxRevisionDelay RevisionDelay { get; }
        
        
        internal void AddCard(LearningCard card)
        {
            if (card.CardBoxId != Id)
            {
                throw new DomainException.CardBoxIdMismatchException();
            }
            
            _learningCards.Add(card);
        }

        internal void RemoveCard(LearningCard card)
        {
            if (card.CardBoxId != Id)
            {
                throw new DomainException.CardBoxIdMismatchException();
            }

            _learningCards.Remove(card);
        }

    }
}