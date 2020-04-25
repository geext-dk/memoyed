using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Memoyed.Domain.Cards.Shared;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSet : AggregateRoot
    {
        private readonly List<CardBox> _cardBoxes = new List<CardBox>();
        private readonly List<RevisionSessionId> _completedSessionIds = new List<RevisionSessionId>();

        /// <summary>
        ///     Card Box Set constructor, intended for creating new instances
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
            Id = id;
            OwnerId = ownerId;
            Name = name;
            NativeLanguage = nativeLanguage;
            TargetLanguage = targetLanguage;
        }

        // ReSharper disable once UnusedMember.Local
        private CardBoxSet()
        {
        }

        /// <summary>
        ///     Id of the card box set
        /// </summary>
        public CardBoxSetId Id { get; }

        public RevisionSessionId? CurrentRevisionSessionId { get; private set; }

        public CardBoxSetName Name { get; private set; }

        public CardBoxSetOwnerId OwnerId { get; }

        /// <summary>
        ///     Language that user knows
        /// </summary>
        public CardBoxSetLanguage NativeLanguage { get; }

        /// <summary>
        ///     Language that user is learning
        /// </summary>
        public CardBoxSetLanguage TargetLanguage { get; }

        /// <summary>
        ///     Card boxes contained in the set, positioned in an increasing level order
        /// </summary>
        public IReadOnlyCollection<CardBox> CardBoxes => _cardBoxes.AsReadOnly();

        public IReadOnlyCollection<RevisionSessionId> CompletedRevisionSessionIds => _completedSessionIds.AsReadOnly();

        public void Rename(CardBoxSetName newName)
        {
            Name = newName;
        }

        public RevisionSession StartRevisionSession(UtcTime? now = null)
        {
            if (CurrentRevisionSessionId != null)
                throw new InvalidOperationException("An uncompleted revision session exists");

            now ??= new UtcTime(DateTime.UtcNow);

            var cardsReadyForSession = _cardBoxes
                .SelectMany(b => b.Cards.Select(c => new
                {
                    box = b,
                    card = c
                }).Where(bc => bc.card.CardBoxChangedDate != null &&
                               bc.card.CardBoxChangedDate.Value.AddDays(bc.box.RevisionDelay) <= now))
                .Select(bc => bc.card)
                .ToList();

            var sessionId = new RevisionSessionId(Guid.NewGuid());
            var sessionCards = cardsReadyForSession
                .Select(c => new SessionCard(sessionId, c))
                .ToList();

            var session = new RevisionSession(sessionId, Id, sessionCards);
            CurrentRevisionSessionId = session.Id;

            return session;
        }

        public void ProcessCardsFromRevisionSession(RevisionSession revisionSession,
            UtcTime? dateTime = null)
        {
            if (Id != revisionSession.CardBoxSetId)
                throw new InvalidOperationException("The revision session was created from other card box set");

            if (CurrentRevisionSessionId != revisionSession.Id)
                throw new InvalidOperationException(
                    "The revision session doesn't match with the current revision session of the set");

            if (_completedSessionIds.Contains(revisionSession.Id)) return;

            if (revisionSession.Status != RevisionSessionStatus.Completed ||
                revisionSession.SessionCards.Any(c => c.Status == SessionCardStatus.NotAnswered))
                throw new DomainException.RevisionSessionNotCompletedException();

            var splittedCardIds = revisionSession.SessionCards
                .GroupBy(c => c.Status)
                .ToDictionary(c => c.Key, x => x.Select(c => c.CardId).ToArray());

            if (splittedCardIds.TryGetValue(SessionCardStatus.AnsweredCorrectly, out var answeredCorrectlyCardIds))
                foreach (var cardId in answeredCorrectlyCardIds)
                    PromoteCard(cardId, dateTime);

            if (splittedCardIds.TryGetValue(SessionCardStatus.AnsweredWrong, out var answeredWrongCardIds))
                foreach (var cardId in answeredWrongCardIds)
                    DemoteCard(cardId, dateTime);

            _completedSessionIds.Add(revisionSession.Id);
            CurrentRevisionSessionId = null;
        }

        /// <summary>
        ///     Adds a card box to the set
        /// </summary>
        /// <param name="cardBox">A card box to add</param>
        /// <exception cref="DomainException.CardBoxSetIdMismatchException">
        ///     Throws if the card box's set id doesn't
        ///     match with the set's id
        /// </exception>
        /// <exception cref="DomainException.CardBoxAlreadyInSetException">
        ///     Throws if a card box with the same id is
        ///     already in the set
        /// </exception>
        /// <exception cref="DomainException.CardBoxLevelAlreadyExistException">
        ///     Throws if a card box with the same level
        ///     is already in the set
        /// </exception>
        /// <exception cref="DomainException.DecreasingRevisionDelayException">
        ///     Throws if there exists a card box in the
        ///     set with level lesser than the added card box but its revision delay is greater than the added card box has
        /// </exception>
        public void AddCardBox(CardBox cardBox)
        {
            if (cardBox.SetId != Id) throw new DomainException.CardBoxSetIdMismatchException();

            if (_cardBoxes.Count > 0)
            {
                if (_cardBoxes.Any(c => c.Id == cardBox.Id)) throw new DomainException.CardBoxAlreadyInSetException();

                if (_cardBoxes.Any(c => c.Level == cardBox.Level))
                    throw new DomainException.CardBoxLevelAlreadyExistException();

                if (_cardBoxes.Any(c => c.Level < cardBox.Level
                                        && c.RevisionDelay > cardBox.RevisionDelay))
                    throw new DomainException.DecreasingRevisionDelayException();
            }

            _cardBoxes.Add(cardBox);
        }

        /// <summary>
        ///     Removes the card box from the set. If it doesn't contained in the set, nothing happens.
        /// </summary>
        /// <param name="cardBoxId">An id of the cardBox to remove</param>
        public void RemoveCardBox(CardBoxId cardBoxId)
        {
            var existing = CardBoxes.FirstOrDefault(b => b.Id == cardBoxId);
            if (existing == null) throw new DomainException.CardBoxNotFoundInSetException();

            _cardBoxes.Remove(existing);
        }

        /// <summary>
        ///     Add new card to the set. It is placed in a box with a lowest level.
        /// </summary>
        /// <param name="card">A card to add</param>
        /// <param name="now">Current time in UTC</param>
        /// <exception cref="DomainException.CardAlreadyInSetException">
        ///     Throws if a card with the same id
        ///     already exists in the set
        /// </exception>
        public void AddNewCard(Card card, UtcTime? now = null)
        {
            now ??= new UtcTime(DateTime.UtcNow);
            EnsureAtLeastOneBoxExists();

            var box = GetBoxContainingCard(card.Id);
            if (box != null) throw new DomainException.CardAlreadyInSetException();

            box = GetMinimalLevelBox();
            card.ChangeCardBoxId(box!.Id, now);
            box.AddCard(card);
        }

        public void RemoveCard(CardId id)
        {
            var box = GetBoxContainingCard(id);
            if (box == null) throw new DomainException.CardNotInSetException();

            box.RemoveCard(id);
        }

        private void PromoteCard(CardId cardId, UtcTime? now = null)
        {
            var box = GetBoxContainingCard(cardId);
            if (box == null) throw new DomainException.CardNotInSetException();

            var nextLevelBox = GetNextLevelBox(box.Level);
            if (nextLevelBox == null) return;

            var card = box.Cards.Single(c => c.Id == cardId);
            box.RemoveCard(card.Id);
            card.ChangeCardBoxId(nextLevelBox.Id, now ?? new UtcTime(DateTime.UtcNow));
            nextLevelBox.AddCard(card);
        }

        private void DemoteCard(CardId cardId, UtcTime? now = null)
        {
            var box = GetBoxContainingCard(cardId);
            if (box == null) throw new DomainException.CardNotInSetException();

            var prevLevelBox = GetPreviousLevelBox(box.Level);
            if (prevLevelBox == null) return;

            var card = box.Cards.Single(c => c.Id == cardId);
            box.RemoveCard(card.Id);
            card.ChangeCardBoxId(prevLevelBox.Id, now ?? new UtcTime(DateTime.UtcNow));
            prevLevelBox.AddCard(card);
        }

        private CardBox? GetBoxContainingCard(CardId cardId)
        {
            return CardBoxes.FirstOrDefault(b => b.Cards.Any(c => c.Id == cardId));
        }

        private CardBox? GetMinimalLevelBox()
        {
            return CardBoxes.Count > 0
                ? CardBoxes.Aggregate((prev, next) => prev.Level < next.Level ? prev : next)
                : null;
        }

        private CardBox? GetNextLevelBox(CardBoxLevel level)
        {
            var nextLevelBoxIndex = _cardBoxes.FindIndex(b => b.Level > level);

            return nextLevelBoxIndex != -1 ? _cardBoxes[nextLevelBoxIndex] : null;
        }

        private CardBox? GetPreviousLevelBox(CardBoxLevel level)
        {
            var previousLevelBoxIndex = _cardBoxes.FindLastIndex(b => b.Level < level);

            return previousLevelBoxIndex != -1 ? _cardBoxes[previousLevelBoxIndex] : null;
        }

        private void EnsureAtLeastOneBoxExists()
        {
            if (_cardBoxes.Count == 0) throw new DomainException.NoBoxesInSetException();
        }
    }
}