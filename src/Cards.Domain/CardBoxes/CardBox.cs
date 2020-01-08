using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBox : ISnapshotable<ICardBoxSnapshot>
    {
        private readonly List<LearningCard> _learningCards = new List<LearningCard>();
        
        /// <summary>
        /// Card Box constructor, used for creating new card boxes
        /// </summary>
        /// <param name="id">Id of the card box</param>
        /// <param name="setId">Id of the card box set the card box belongs to</param>
        /// <param name="level">Level of the card box</param>
        /// <param name="revisionDelay">Days until revision of cards in the card box</param>
        public CardBox(CardBoxId id, CardBoxSetId setId, CardBoxLevel level, CardBoxRevisionDelay revisionDelay)
        {
            Id = id;
            SetId = setId;
            Level = level;
            RevisionDelay = revisionDelay;
        }

        private CardBox(ICardBoxSnapshot snapshot) : this(
            new CardBoxId(snapshot.Id),
            new CardBoxSetId(snapshot.SetId),
            new CardBoxLevel(snapshot.Level),
            new CardBoxRevisionDelay(snapshot.RevisionDelay))
        {
            _learningCards = snapshot.LearningCards
                .Select(LearningCard.FromSnapshot)
                .ToList();
        }

        /// <summary>
        /// Enumerable of the learning cards contained in the card box
        /// </summary>
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

        public ICardBoxSnapshot CreateSnapshot() => new Snapshot(this);
        public static CardBox FromSnapshot(ICardBoxSnapshot snapshot) => new CardBox(snapshot);

        private class Snapshot : ICardBoxSnapshot
        {
            private readonly CardBox _cardBox;
            public Snapshot(CardBox cardBox)
            {
                _cardBox = cardBox;
            }

            public Guid Id => _cardBox.Id.Value;
            public Guid SetId => _cardBox.SetId.Value;
            public int Level => _cardBox.Level.Value;
            public int RevisionDelay => _cardBox.RevisionDelay.Value;
            public IEnumerable<ILearningCardSnapshot> LearningCards => _cardBox.LearningCards
                .Select(c => c.CreateSnapshot());
        }
    }
}