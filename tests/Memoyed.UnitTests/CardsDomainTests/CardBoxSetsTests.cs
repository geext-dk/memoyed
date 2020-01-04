using System;
using System.Linq;
using Memoyed.Cards.Domain;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class CardBoxSetsTests
    {
        [Fact]
        public void CardBoxSetIdConstructor_EmptyGuidPassed_ThrowsEmptyIdException()
        {
            // Arrange
            var emptyId = Guid.Empty;
            var throws = false;
            
            // Act
            try
            {
                var id = new CardBoxSetId(emptyId);
            }
            catch (DomainException.EmptyIdException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxSetIdConstructor_NonEmptyGuidPassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            var nonEmptyId = Guid.NewGuid();
            CardBoxSetId id;
            
            // Act
            id = new CardBoxSetId(nonEmptyId);
            
            // Assert
            Assert.Equal(nonEmptyId, id.Value);
        }

        [Fact]
        public void CardBoxSetLanguageConstructor_NonValidLanguagePassed_ThrowsInvalidLanguageException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => false;
            var testLanguageString = "Any";
            CardBoxSetLanguage language;
            var throws = false;

            // Act
            try
            {
                language = new CardBoxSetLanguage(testLanguageString, testValidator);
            }
            catch (DomainException.InvalidLanguageException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxSetLanguageConstructor_ValidLanguagePassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var testLanguageString = "Any";
            CardBoxSetLanguage language;
            
            // Act
            language = new CardBoxSetLanguage(testLanguageString, testValidator);
            
            // Assert
            Assert.Equal(testLanguageString, language.Value);
        }

        [Fact]
        public void CardBoxSetConstructor_ValidObjectsPassed_InitializesProperly()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var id = new CardBoxSetId(Guid.NewGuid());
            var nativeLanguage = new CardBoxSetLanguage("Russian", testValidator);
            var targetLangauge = new CardBoxSetLanguage("Norwegian", testValidator);
            CardBoxSet set;
            
            // Act
            set = new CardBoxSet(id, nativeLanguage, targetLangauge);
            
            // Assert
            Assert.Equal(id.Value, set.Id.Value);
            Assert.Equal(nativeLanguage.Value, set.NativeLanguage.Value);
            Assert.Equal(targetLangauge.Value, set.TargetLanguage.Value);
            Assert.Empty(set.CardBoxes);
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxesWithIncreasingRepeatDelay_NoExceptionsThrown()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(2));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRepeatDelay(4));
            var thirdCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(3), new CardBoxRepeatDelay(7));
            
            // Act
            set.AddCardBox(firstCardBox);
            set.AddCardBox(secondCardBox);
            set.AddCardBox(thirdCardBox);
            
            // Assert
            Assert.Collection(set.CardBoxes,
                first => Assert.Equal(firstCardBox.Id.Value, first.Id.Value),
                second => Assert.Equal(secondCardBox.Id.Value, second.Id.Value),
                third => Assert.Equal(thirdCardBox.Id.Value, third.Id.Value));
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxWithSameSetId_AddsSuccessfully()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(2));
            
            // Act
            set.AddCardBox(cardBox);
            
            // Assert
            Assert.Single(set.CardBoxes);
            var cardBoxFromSet = set.CardBoxes.Single();
            Assert.Equal(cardBoxFromSet.Id.Value, cardBox.Id.Value);
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxWithDifferentSetId_ThrowsInvalidSetIdException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                new CardBoxSetId(Guid.NewGuid()), new CardBoxLevel(0), new CardBoxRepeatDelay(2));
            var throws = false;
            
            // Act
            try
            {
                set.AddCardBox(cardBox);
            }
            catch (DomainException.CardBoxSetIdMismatchException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddSameCardBoxAgain_ThrowsCardBoxAlreadyInSetException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(2));
            var throws = false;
            
            set.AddCardBox(cardBox);
            
            // Act
            try
            {
                set.AddCardBox(cardBox);
            }
            catch (DomainException.CardBoxAlreadyInSetException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxesWithDecreasingRepeatDelay_ThrowsDecreasingRepeatDelayException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRepeatDelay(2));

            var throws = false;
            set.AddCardBox(firstCardBox);
            
            // Act
            try
            {
                set.AddCardBox(secondCardBox);
            }
            catch (DomainException.DecreasingRepeatDelayException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxesWithSameLevel_ThrowsCardBoxLevelAlreadyExistsException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(2));

            var throws = false;
            set.AddCardBox(firstCardBox);
            
            // Act
            try
            {
                set.AddCardBox(secondCardBox);
            }
            catch (DomainException.CardBoxLevelAlreadyExistException)
            {
                throws = true;
            }
            
            // Assert
            Assert.True(throws);
        }

        [Fact]
        public void CardBoxSetRemoveCardBox_ContainedCardBoxPassed_RemovesCardBoxFromEnumerable()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4));

            set.AddCardBox(cardBox);
            
            // Act
            set.RemoveCardBox(cardBox);
            
            // Assert
            Assert.Empty(set.CardBoxes);
        }

        [Fact]
        public void CardBoxSetRemoveCardBox_NotContainedCardBoxPassed_EnumerableDoesntChange()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRepeatDelay(7));

            set.AddCardBox(firstCardBox);
            
            // Act
            set.RemoveCardBox(secondCardBox);
            
            // Assert
            Assert.Single(set.CardBoxes);
            Assert.Equal(firstCardBox.Id, set.CardBoxes.Single().Id);
        }

        [Fact]
        public void CardBoxSetAddNewCard_CardNotContainedInBoxesPassed_AddsSuccessfully()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4));
            set.AddCardBox(cardBox);
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act
            set.AddNewCard(learningCard);

            // Assert
            Assert.True(set.CardBoxes.Any(b =>
                b.LearningCards.Any(c =>
                    c.Id == learningCard.Id)));
        }

        [Fact]
        public void CardBoxSetAddNewCard_CardNotContainedInBoxesPassed_AddsToLowestLevelBox()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            set.AddCardBox(new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4)));
            set.AddCardBox(new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRepeatDelay(7)));
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act
            set.AddNewCard(learningCard);

            // Assert
            var box = set.CardBoxes.Aggregate((prev, next) => prev.Level < next.Level ? prev : next);
            Assert.True(box.LearningCards.Any(c => c.Id == learningCard.Id));
        }

        [Fact]
        public void CardBoxSetAddNewCard_CardAlreadyContainedInBoxPassed_ThrowsLearningCardAlreadyInSetException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4));
            set.AddCardBox(cardBox);
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));

            set.AddNewCard(learningCard);
            
            // Act && Assert
            Assert.Throws<DomainException.LearningCardAlreadyInSetException>(
                () => set.AddNewCard(learningCard));
        }

        [Fact]
        public void CardBoxSetAddNewCard_SetDoesntContainAnyBoxes_ThrowsNoBoxesInSetException()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act && Assert
            Assert.Throws<DomainException.NoBoxesInSetException>(
                () => set.AddNewCard(learningCard));
        }

        [Fact]
        public void CardBoxPromoteCardToNextLevel_AllConditionsMet_SuccessfullyMovesToNextLevel()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            set.AddCardBox(new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4)));
            set.AddCardBox(new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRepeatDelay(7)));
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            set.AddNewCard(learningCard);
            
            // Act
            set.PromoteCardToNextLevel(learningCard);
            
            // Assert
            var box = set.CardBoxes.First(b => b.Level == 1);
            Assert.True(box.LearningCards.Any(c => c.Id == learningCard.Id));
        }

        [Fact]
        public void CardBoxPromoteCardToNextLevel_NoNextLevelBox_NothingHappens()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            set.AddCardBox(new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4)));
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            set.AddNewCard(learningCard);
            
            // Act
            set.PromoteCardToNextLevel(learningCard);
            
            // Assert
            var box = set.CardBoxes.First(b => b.Level == 0);
            Assert.True(box.LearningCards.Any(c => c.Id == learningCard.Id));
        }

        [Fact]
        public void CardBoxPromoteCardToNextLevel_NoSuchCardInSet_ThrowsLearningCardNotInSetExceptiion()
        {
            // Arrange
            DomainChecks.ValidateLanguage testValidator = _ => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", testValidator),
                new CardBoxSetLanguage("Norwegian", testValidator));

            set.AddCardBox(new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRepeatDelay(4)));
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act && Assert
            Assert.Throws<DomainException.LearningCardNotInSetException>(
                () => set.PromoteCardToNextLevel(learningCard));
        }
    }
}