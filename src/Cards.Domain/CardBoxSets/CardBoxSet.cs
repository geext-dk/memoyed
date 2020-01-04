using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.LearningCards;

namespace Memoyed.Cards.Domain.CardBoxSets
{
    public class CardBoxSet
    {
        public CardBoxSet(CardBoxSetId id, CardBoxSetLanguage nativeLanguage, CardBoxSetLanguage targetLanguage)
        {
            Id = id;
            NativeLanguage = nativeLanguage;
            TargetLanguage = targetLanguage;
        }

        public CardBoxSetId Id { get; }
        public CardBoxSetLanguage NativeLanguage { get; }
        public CardBoxSetLanguage TargetLanguage { get; }
        private readonly List<CardBox> _cardBoxes = new List<CardBox>();
        public IEnumerable<CardBox> CardBoxes => _cardBoxes.AsEnumerable();

        public void AddCardBox(CardBox cardBox)
        {
            if (cardBox.SetId != Id)
            {
                throw new DomainException.CardBoxSetIdMismatchException();
            }

            if (_cardBoxes.Any(c => c.Id == cardBox.Id))
            {
                throw new DomainException.CardBoxAlreadyInSetException();
            }

            if (_cardBoxes.Any(c => c.Level == cardBox.Level))
            {
                throw new DomainException.CardBoxLevelAlreadyExistException();
            }

            if (_cardBoxes.Any(c => c.Level < cardBox.Level
                                    && c.RepeatDelay > cardBox.RepeatDelay))
            {
                throw new DomainException.DecreasingRepeatDelayException();
            }

            _cardBoxes.Add(cardBox);
        }

        public void RemoveCardBox(CardBox cardBox)
        {
            _cardBoxes.RemoveAll(b => b.Id == cardBox.Id);
        }

        public void AddNewCard(LearningCard card)
        {
            EnsureAtLeastOneBoxExists();
            
            var box = GetBoxContainingCard(card);
            if (box != null)
            {
                throw new DomainException.LearningCardAlreadyInSetException();
            }

            box = GetMinimalLevelBox();
            box.AddCard(card);
        }

        public void PromoteCardToNextLevel(LearningCard card)
        {
            var box = GetBoxContainingCard(card);
            if (box == null)
            {
                throw new DomainException.LearningCardNotInSetException();
            }

            var nextLevelBox = GetNextLevelBox(box);
            if (nextLevelBox == null)
            {
                return;
            }
            
            box.RemoveCard(card);
            nextLevelBox.AddCard(card);
        }

        private CardBox GetBoxContainingCard(LearningCard card)
        {
            return _cardBoxes.FirstOrDefault(b => b.LearningCards.Any(c => c.Id == card.Id));
        }

        private CardBox GetMinimalLevelBox() => _cardBoxes.Aggregate((prev, next) =>
            prev.Level < next.Level ? prev : next);

        private CardBox GetNextLevelBox(CardBox box)
        {
            var nextLevelBoxes = _cardBoxes
                .Where(b => b.Level > box.Level)
                .ToList();

            if (nextLevelBoxes.Count == 0)
            {
                return null;
            }
            
            return nextLevelBoxes
                .Aggregate((prev, next) => prev.Level < next.Level ? prev : next);
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