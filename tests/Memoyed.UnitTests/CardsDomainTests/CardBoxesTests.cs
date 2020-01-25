using System;
using System.Collections.Generic;
using Memoyed.Cards.Domain;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.Shared;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class CardBoxesTests
    {
        [Fact]
        public void CardBoxIdConstructor_EmptyGuidPassed_ThrowsEmptyIdException()
        {
            // Arrange
            var id = Guid.Empty;
            
            // Act && Assert
            Assert.Throws<DomainException.EmptyIdException>(
                () => new CardBoxId(id));
        }

        [Fact]
        public void CardBoxIdConstructor_NonEmptyGuidPassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            var id = Guid.NewGuid();
            
            // Act
            var boxId = new CardBoxId(id);
            
            // Assert
            Assert.Equal(id, boxId.Value);
        }

        [Fact]
        public void CardBoxLevelConstructor_NegativeValuePassed_ThrowsInvalidCardBoxLevelException()
        {
            // Arrange
            const int negativeLevel = -1;
            
            // Act && Assert
            Assert.Throws<DomainException.InvalidCardBoxLevelException>(
                () => new CardBoxLevel(negativeLevel));
        }

        [Fact]
        public void CardBoxLevelConstructor_NonNegativeValuePassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            const int sampleLevel = 5;
            
            // Act
            var level = new CardBoxLevel(sampleLevel);
            
            // Assert
            Assert.Equal(sampleLevel, level.Value);
        }

        [Fact]
        public void CardBoxRevisionDelayConstructor_PassNegativeValue_ThrowsInvalidRevisionDelayException()
        {
            // Arrange
            var negativeDaysUntilRevision = -4;
            
            // Act && Assert
            Assert.Throws<DomainException.InvalidRevisionDelayException>(
                () => new CardBoxRevisionDelay(negativeDaysUntilRevision));
        }

        [Fact]
        public void CardBoxRevisionDelayConstructor_PassValueGreaterThan30_ThrowsInvalidRevisionDelayException()
        {
            // Arrange
            const int tooBigDaysUntilRevision = 31;
            
            // Act && Assert
            Assert.Throws<DomainException.InvalidRevisionDelayException>(
                () => new CardBoxRevisionDelay(tooBigDaysUntilRevision));
        }

        [Fact]
        public void CardBoxRevisionDelayConstructor_ZeroPassed_ThrowsInvalidRevisionDelayException()
        {
            // Arrange
            const int zero = 0;
            
            // Act && Assert
            Assert.Throws<DomainException.InvalidRevisionDelayException>(
                () => new CardBoxRevisionDelay(zero));
        }

        [Fact]
        public void CardBoxRevisionDelayConstructor_ValidValuePassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            const int validValue = 14;
            
            // Act
            var revisionDelay = new CardBoxRevisionDelay(validValue);
            
            // Assert
            Assert.Equal(validValue, revisionDelay.Value);
        }

        [Fact]
        public void CardBoxConstructor_AnyDomainValuesPassed_PropertiesReturnThePassedValues()
        {
            // Arrange
            var id = new CardBoxId(Guid.NewGuid());
            var setId = new CardBoxSetId(Guid.NewGuid());
            var level = new CardBoxLevel(0);
            var revisionDelay = new CardBoxRevisionDelay(3);
            
            // Act
            var box = new CardBox(id, setId, level, revisionDelay);

            // Assert
            Assert.Equal(id.Value, box.Id.Value);
            Assert.Equal(setId.Value, box.SetId.Value);
            Assert.Equal(level.Value, box.Level.Value);
            Assert.Equal(revisionDelay.Value, box.RevisionDelay.Value);
        }

        [Fact]
        public void CardBoxConstructor_AnyDomainValuesPassed_NoLearningCards()
        {
            // Arrange
            var id = new CardBoxId(Guid.NewGuid());
            var setId = new CardBoxSetId(Guid.NewGuid());
            var level = new CardBoxLevel(0);
            var revisionDelay = new CardBoxRevisionDelay(3);
            
            // Act
            var box = new CardBox(id, setId, level, revisionDelay);

            // Assert
            Assert.Empty(box.LearningCards);
        }

        [Fact]
        public void CardBoxCreateSnapshot_CalledOnAnyObject_ReturnsSnapshotOfTheInstance()
        {
            // Arrange
            var box = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxLevel(0),
                new CardBoxRevisionDelay(3));
            
            var set = new CardBoxSet(
                box.SetId,
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            set.AddCardBox(box);

            var card = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            set.AddNewCard(card, new UtcTime(DateTime.UtcNow));

            // Act
            var snapshot = box.CreateSnapshot();
            
            // Assert
            Assert.Equal(box.Id.Value, snapshot.Id);
            Assert.Equal(box.SetId.Value, snapshot.SetId);
            Assert.Equal(box.Level.Value, snapshot.Level);
            Assert.Equal(box.RevisionDelay.Value, snapshot.RevisionDelay);
            Assert.Single(snapshot.LearningCards);
            Assert.Collection(snapshot.LearningCards,
                c => Assert.Equal(card.Id, c.Id));
        }

        private class TestBoxSnapshot : ICardBoxSnapshot
        {
            public Guid Id { get; set; }
            public Guid SetId { get; set; }
            public int Level { get; set; }
            public int RevisionDelay { get; set; }
            public IEnumerable<ILearningCardSnapshot> LearningCards { get; set; }
        }

        private class TestCardSnapshot : ILearningCardSnapshot
        {
            public Guid Id { get; set; }
            public Guid? CardBoxId { get; set; }
            public string NativeLanguageWord { get; set; }
            public string TargetLanguageWord { get; set; }
            public string Comment { get; set; }
            public DateTime? CardBoxChangedDate { get; set; }
        }

        [Fact]
        public void CardBoxFromSnapshot_ValidSnapshotPassed_ReturnsRestoredObject()
        {
            // Arrange
            var cardSnapshot = new TestCardSnapshot
            {
                Id = Guid.NewGuid(),
                Comment = null,
                CardBoxId = null,
                NativeLanguageWord = "Russian",
                TargetLanguageWord = "Norwegian",
                CardBoxChangedDate = null
            };

            var snapshot = new TestBoxSnapshot
            {
                Id = Guid.NewGuid(),
                SetId = Guid.NewGuid(),
                Level = 0,
                RevisionDelay = 15,
                LearningCards = new List<TestCardSnapshot>
                {
                    cardSnapshot
                }
            };
            
            // Act
            var box = CardBox.FromSnapshot(snapshot);
            
            // Assert
            Assert.Equal(snapshot.Id, box.Id.Value);
            Assert.Equal(snapshot.SetId, box.SetId.Value);
            Assert.Equal(snapshot.Level, box.Level.Value);
            Assert.Equal(snapshot.RevisionDelay, box.RevisionDelay.Value);
            Assert.Collection(box.LearningCards,
                c => Assert.Equal(cardSnapshot.Id, c.Id.Value));
            
        }

        [Fact]
        public void CardBoxFromSnapshot_JustCreatedSnapshotPassed_ReturnsObjectEqualToOriginal()
        {
            // Arrange
            var box = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxLevel(0),
                new CardBoxRevisionDelay(3));
            
            var set = new CardBoxSet(
                box.SetId,
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            set.AddCardBox(box);

            var card = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            set.AddNewCard(card, new UtcTime(DateTime.UtcNow));

            var snapshot = box.CreateSnapshot();
            
            // Act
            var fromSnapshot = CardBox.FromSnapshot(snapshot);
            
            // Assert
            Assert.Equal(box.Id, fromSnapshot.Id);
            Assert.Equal(box.SetId, fromSnapshot.SetId);
            Assert.Equal(box.Level, fromSnapshot.Level);
            Assert.Equal(box.RevisionDelay, fromSnapshot.RevisionDelay);
            Assert.Collection(box.LearningCards, c =>
                Assert.Equal(card.Id, c.Id));
        }
    }
}