using System;
using Memoyed.Domain.Cards;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.CardBoxSets;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class CardBoxesTests
    {
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
        public void CardBoxRevisionDelayConstructor_ZeroPassed_ThrowsInvalidRevisionDelayException()
        {
            // Arrange
            const int zero = 0;

            // Act && Assert
            Assert.Throws<DomainException.InvalidRevisionDelayException>(
                () => new CardBoxRevisionDelay(zero));
        }
    }
}