using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxes
{
    public class CardBox : Entity
    {
        private readonly List<Card> _cards = new List<Card>();

        /// <summary>
        ///     Card Box constructor, used for creating new card boxes
        /// </summary>
        /// <param name="id">Id of the card box</param>
        /// <param name="setId">Id of the card box set the card box belongs to</param>
        /// <param name="level">Level of the card box</param>
        /// <param name="revisionDelay">Days until revision of cards in the card box</param>
        public CardBox(Guid id, Guid setId, CardBoxLevel level, CardBoxRevisionDelay revisionDelay)
        {
            Id = id;
            SetId = setId;
            Level = level;
            RevisionDelay = revisionDelay;
        }

        private CardBox()
        {
        }

        /// <summary>
        ///     Enumerable of the learning cards contained in the card box
        /// </summary>
        public IEnumerable<Card> Cards => _cards.AsEnumerable();

        /// <summary>
        ///     Id of the card box
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        ///     Id of the card box set which owns the card box
        /// </summary>
        public Guid SetId { get; }

        /// <summary>
        ///     Level of the card box
        /// </summary>
        public CardBoxLevel Level { get; }

        /// <summary>
        ///     Days until revision of the contained cards
        ///     Counts starts after the card is moved to the box
        /// </summary>
        public CardBoxRevisionDelay RevisionDelay { get; }

        internal void AddCard(Card card)
        {
            if (card.CardBoxId != Id) throw new DomainException.CardBoxIdMismatchException();

            _cards.Add(card);
        }

        internal void RemoveCard(Guid cardId)
        {
            _cards.RemoveAll(c => c.Id == cardId);
        }
    }
}