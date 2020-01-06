using System;
using System.Linq;
using Memoyed.Cards.Domain;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
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
            var thrown = false;
            CardBoxId boxId;
            
            // Act
            try
            {
                boxId = new CardBoxId(id);
            }
            catch (DomainException.EmptyIdException)
            {
                thrown = true;
            }
            
            // Assert
            Assert.True(thrown);
        }

        [Fact]
        public void CardBoxIdConstructor_NonEmptyGuidPassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var thrown = false;
            CardBoxId boxId;
            
            // Act
            boxId = new CardBoxId(id);
            
            // Assert
            Assert.Equal(id, boxId.Value);
        }

        [Fact]
        public void CardBoxLevelConstructor_NegativeValuePassed_ThrowsInvalidCardBoxLevelException()
        {
            // Arrange
            var negativeLevel = -1;
            var throws = false;
            
            // Act
            try
            {
                var level = new CardBoxLevel(negativeLevel);
            }
            catch (DomainException.InvalidCardBoxLevelException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxLevelConstructor_NonNegativeValuePassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            var sampleLevel = 5;
            CardBoxLevel level;
            
            // Act
            level = new CardBoxLevel(sampleLevel);
            
            // Assert
            Assert.Equal(sampleLevel, level.Value);
        }

        [Fact]
        public void CardBoxRepeatDelayConstructor_PassNegativeValue_ThrowsInvalidRepeatDelayException()
        {
            // Arrange
            var negativeDaysUntilRepeat = -4;
            var throws = false;
            
            // Act
            try
            {
                var daysUntilRepeat = new CardBoxRepeatDelay(negativeDaysUntilRepeat);
            }
            catch (DomainException.InvalidRepeatDelayException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void
            CardBoxRepeatDelayConstructor_PassValueGreaterThanOneMonth_ThrowsInvalidRepeatDelayException()
        {
            // Arrange
            var tooBigDaysUntilRepeat = 31;
            var throws = false;
            
            // Act
            try
            {
                var daysUntilRepeat = new CardBoxRepeatDelay(tooBigDaysUntilRepeat);
            }
            catch (DomainException.InvalidRepeatDelayException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxRepeatDelayConstructor_ZeroPassed_ThrowsInvalidRepeatDelayException()
        {
            // Arrange
            var zero = 0;
            var throws = false;
            
            // Act
            try
            {
                var daysUntilRepeat = new CardBoxRepeatDelay(zero);
            }
            catch (DomainException.InvalidRepeatDelayException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxRepeatDelayConstructor_ValidValuePassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            var validValue = 14;
            CardBoxRepeatDelay repeatDelay;
            
            // Act
            repeatDelay = new CardBoxRepeatDelay(validValue);
            
            // Assert
            Assert.Equal(validValue, repeatDelay.Value);
        }

        [Fact]
        public void CardBoxConstructor_AnyDomainValuesPassed_PropertiesReturnThePassedValues()
        {
            // Arrange
            var id = new CardBoxId(Guid.NewGuid());
            var setId = new CardBoxSetId(Guid.NewGuid());
            var level = new CardBoxLevel(0);
            var repeatDelay = new CardBoxRepeatDelay(3);
            CardBox box;
            
            // Act
            box = new CardBox(id, setId, level, repeatDelay);

            // Assert
            Assert.Equal(id.Value, box.Id.Value);
            Assert.Equal(setId.Value, box.SetId.Value);
            Assert.Equal(level.Value, box.Level.Value);
            Assert.Equal(repeatDelay.Value, box.RepeatDelay.Value);
        }

        [Fact]
        public void CardBoxConstructor_AnyDomainValuesPassed_NoLearningCards()
        {
            // Arrange
            var id = new CardBoxId(Guid.NewGuid());
            var setId = new CardBoxSetId(Guid.NewGuid());
            var level = new CardBoxLevel(0);
            var repeatDelay = new CardBoxRepeatDelay(3);
            CardBox box;
            
            // Act
            box = new CardBox(id, setId, level, repeatDelay);

            // Assert
            Assert.Empty(box.LearningCards);
        }
    }
}