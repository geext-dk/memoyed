using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.Cards;
using Memoyed.Cards.Domain.RevisionSessions;
using Memoyed.Cards.Domain.RevisionSessions.SessionCards;
using Memoyed.Cards.Domain.Shared;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxSets
{
    public class CardBoxSet : AggregateRoot
    {
        /// <summary>
        /// Card Box Set constructor, intended for creating new instances
        /// </summary>
        /// <param name="id">Id of the card box set</param>
        /// <param name="ownerId"></param>
        /// <param name="name"></param>
        /// <param name="nativeLanguage">Language which user knows</param>
        /// <param name="targetLanguage">Language which user is learning</param>
        public CardBoxSet(CardBoxSetId id, CardBoxSetOwnerId ownerId, CardBoxSetName name,
            CardBoxSetLanguage nativeLanguage,
            CardBoxSetLanguage targetLanguage)
        {
            _id = id;
            Name = name;
            NativeLanguage = nativeLanguage;
            TargetLanguage = targetLanguage;
        }

        private CardBoxSet()
        {
        }

        /// <summary>
        /// Id of the card box set
        /// </summary>
        public CardBoxSetId Id => _id;

        private readonly CardBoxSetId _id;
        
        public CardBoxSetName Name { get; private set; }
        
        public CardBoxSetOwnerId OwnerId { get; }

        /// <summary>
        /// Language that user knows
        /// </summary>
        public CardBoxSetLanguage NativeLanguage { get; private set; }
        
        /// <summary>
        /// Language that user is learning
        /// </summary>
        public CardBoxSetLanguage TargetLanguage { get; private set; }
        
        /// <summary>
        /// Card boxes contained in the set, positioned in an increasing level order
        /// </summary>
        public IEnumerable<CardBox> CardBoxes => _cardBoxes.AsReadOnly();
        private readonly List<CardBox> _cardBoxes = new List<CardBox>();

        public ReadOnlyCollection<RevisionSessionId> CompletedRevisionSessionIds => _completedSessionIds.AsReadOnly();
        private readonly List<RevisionSessionId> _completedSessionIds = new List<RevisionSessionId>();

        public void Rename(CardBoxSetName newName)
        {
            Name = newName;
        }

        public RevisionSession StartRevisionSession(UtcTime now = null)
        {
            now ??= new UtcTime(DateTime.UtcNow);

            var cardsReadyForSession = _cardBoxes
                .SelectMany(b => b.Cards.Select(c => new
                {
                    box = b,
                    card = c
                }).Where(bc => bc.card.CardBoxChangedDate != null
                               && bc.card.CardBoxChangedDate.Value.AddDays(bc.box.RevisionDelay) <= now))
                .Select(bc => bc.card)
                .ToList();

            var sessionId = new RevisionSessionId(Guid.NewGuid());
            var sessionCards = cardsReadyForSession
                .Select(c => new SessionCard(sessionId, c))
                .ToList();
            
            return new RevisionSession(sessionId, Id, sessionCards);
        }

        public void ProcessCardsFromRevisionSession(RevisionSessionId revisionSessionId,
            IEnumerable<CardId> answeredCorrectlyCardIds,
            IEnumerable<CardId> answeredWrongCardIds,
            UtcTime dateTime)
        {
            if (_completedSessionIds.Contains(revisionSessionId))
            {
                return;
            }

            foreach (var cardId in answeredCorrectlyCardIds)
            {
                PromoteCard(cardId, dateTime);
            }

            foreach (var cardId in answeredWrongCardIds)
            {
                DemoteCard(cardId, dateTime);
            }
            
            _completedSessionIds.Add(revisionSessionId);
        }

        /// <summary>
        /// Adds a card box to the set
        /// </summary>
        /// <param name="cardBox">A card box to add</param>
        /// <exception cref="DomainException.CardBoxSetIdMismatchException">Throws if the card box's set id doesn't
        /// match with the set's id</exception>
        /// <exception cref="DomainException.CardBoxAlreadyInSetException">Throws if a card box with the same id is
        /// already in the set</exception>
        /// <exception cref="DomainException.CardBoxLevelAlreadyExistException">Throws if a card box with the same level
        /// is already in the set</exception>
        /// <exception cref="DomainException.DecreasingRevisionDelayException">Throws if there exists a card box in the
        /// set with level lesser than the added card box but its revision delay is greater than the added card box has
        /// </exception>
        public void AddCardBox(CardBox cardBox)
        {
            if (cardBox.SetId != Id)
            {
                throw new DomainException.CardBoxSetIdMismatchException();
            }
            
            if (_cardBoxes.Count == 0)
            {
                _cardBoxes.Add(cardBox);
            }
            else
            {
                if (_cardBoxes.Any(c => c.Id == cardBox.Id))
                {
                    throw new DomainException.CardBoxAlreadyInSetException();
                }

                if (_cardBoxes.Any(c => c.Level == cardBox.Level))
                {
                    throw new DomainException.CardBoxLevelAlreadyExistException();
                }

                if (_cardBoxes.Any(c => c.Level < cardBox.Level
                                        && c.RevisionDelay > cardBox.RevisionDelay))
                {
                    throw new DomainException.DecreasingRevisionDelayException();
                }

                var minimalHigherLevelBox = _cardBoxes.FirstOrDefault(b => b.Level > cardBox.Level);
                if (minimalHigherLevelBox == null)
                {
                    _cardBoxes.Add(cardBox);
                }
                else
                {
                    var index = _cardBoxes.IndexOf(minimalHigherLevelBox);
                    _cardBoxes.Insert(index, cardBox);
                }
            }
        }

        /// <summary>
        /// Removes the card box from the set. If it doesn't contained in the set, nothing happens.
        /// </summary>
        /// <param name="cardBox">A card box to remove</param>
        public void RemoveCardBox(CardBox cardBox)
        {
            _cardBoxes.RemoveAll(b => b.Id == cardBox.Id);
        }

        /// <summary>
        /// Add new card to the set. It is placed in a box with a lowest level.
        /// </summary>
        /// <param name="card">A card to add</param>
        /// <param name="now">Current time in UTC</param>
        /// <exception cref="DomainException.CardAlreadyInSetException">Throws if a card with the same id
        /// already exists in the set</exception>
        public void AddNewCard(Card card, UtcTime now)
        {
            EnsureAtLeastOneBoxExists();
            
            var box = GetBoxContainingCard(card.Id);
            if (box != null)
            {
                throw new DomainException.CardAlreadyInSetException();
            }

            box = GetMinimalLevelBox();
            card.ChangeCardBoxId(box.Id, now);
            box.AddCard(card);
        }

        public void RemoveCard(CardId id)
        {
            var box = GetBoxContainingCard(id);
            box?.RemoveCard(id);
        }

        /// <summary>
        /// Moves the card to a box with a greater level. The card will be placed in a box which level is minimal
        /// among boxes with greater level than the card is contained in before the operation.
        /// </summary>
        /// <param name="cardId">An Id of the card to promote</param>
        /// <param name="now">current Time</param>
        /// <exception cref="DomainException.CardNotInSetException">Throws if the given card doesn't exist in
        /// the set </exception>
        private void PromoteCard(CardId cardId, UtcTime? now = null)
        {
            var box = GetBoxContainingCard(cardId);
            if (box == null)
            {
                throw new DomainException.CardNotInSetException();
            }

            var nextLevelBox = GetNextLevelBox(box.Level);
            if (nextLevelBox == null)
            {
                return;
            }

            var card = box.Cards.Single(c => c.Id == cardId);
            box.RemoveCard(card.Id);
            card.ChangeCardBoxId(nextLevelBox.Id, now ?? new UtcTime(DateTime.UtcNow));
            nextLevelBox.AddCard(card);
        }

        /// <summary>
        /// Moves the card to the box with the lowest level
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="now"></param>
        /// <exception cref="DomainException.CardNotInSetException"></exception>
        private void DemoteCard(CardId cardId, UtcTime now = null)
        {
            var box = GetBoxContainingCard(cardId);
            if (box == null)
            {
                throw new DomainException.CardNotInSetException();
            }

            var prevLevelBox = GetPreviousLevelBox(box.Level);
            if (prevLevelBox == null)
            {
                return;
            }

            var card = box.Cards.Single(c => c.Id == cardId);
            box.RemoveCard(card.Id);
            card.ChangeCardBoxId(prevLevelBox.Id, now ?? new UtcTime(DateTime.UtcNow));
            prevLevelBox.AddCard(card);
        }

        private CardBox GetBoxContainingCard(CardId cardId)
        {
            return _cardBoxes.FirstOrDefault(b => b.Cards.Any(c => c.Id == cardId));
        }

        private CardBox GetMinimalLevelBox() => _cardBoxes.Aggregate((prev, next) =>
            prev.Level < next.Level ? prev : next);

        private CardBox GetNextLevelBox(CardBoxLevel level)
        {
            var nextLevelBoxIndex = _cardBoxes.FindIndex(b => b.Level > level);

            return nextLevelBoxIndex != -1 ? _cardBoxes[nextLevelBoxIndex] : null;
        }

        private CardBox GetPreviousLevelBox(CardBoxLevel level)
        {
            var previousLevelBoxIndex = _cardBoxes.FindLastIndex(b => b.Level < level);

            return previousLevelBoxIndex != -1 ? _cardBoxes[previousLevelBoxIndex] : null;
        }

        private void EnsureAtLeastOneBoxExists()
        {
            if (_cardBoxes.Count == 0)
            {
                throw new DomainException.NoBoxesInSetException();
            }
        }
    }
}