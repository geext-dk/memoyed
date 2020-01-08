using System;
using System.Collections.Generic;
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
            
            // Act && Assert
            Assert.Throws<DomainException.EmptyIdException>(
                () => new CardBoxSetId(emptyId));
        }

        [Fact]
        public void CardBoxSetIdConstructor_NonEmptyGuidPassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            var nonEmptyId = Guid.NewGuid();
            
            // Act
            var id = new CardBoxSetId(nonEmptyId);
            
            // Assert
            Assert.Equal(nonEmptyId, id.Value);
        }

        [Fact]
        public void CardBoxSetLanguageConstructor_NonValidLanguagePassed_ThrowsInvalidLanguageException()
        {
            // Arrange
            static bool TestValidator(string _) => false;
            const string testLanguageString = "Any";

            // Act && Assert
            Assert.Throws<DomainException.InvalidLanguageException>(
                () => new CardBoxSetLanguage(testLanguageString, TestValidator));
        }

        [Fact]
        public void CardBoxSetLanguageConstructor_ValidLanguagePassed_PropertyReturnsThePassedValue()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            const string testLanguageString = "Any";
            
            // Act
            var language = new CardBoxSetLanguage(testLanguageString, TestValidator);
            
            // Assert
            Assert.Equal(testLanguageString, language.Value);
        }

        [Fact]
        public void CardBoxSetConstructor_ValidObjectsPassed_InitializesProperly()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var id = new CardBoxSetId(Guid.NewGuid());
            var nativeLanguage = new CardBoxSetLanguage("Russian", TestValidator);
            var targetLanguage = new CardBoxSetLanguage("Norwegian", TestValidator);
            
            // Act
            var set = new CardBoxSet(id, nativeLanguage, targetLanguage);
            
            // Assert
            Assert.Equal(id.Value, set.Id.Value);
            Assert.Equal(nativeLanguage.Value, set.NativeLanguage.Value);
            Assert.Equal(targetLanguage.Value, set.TargetLanguage.Value);
            Assert.Empty(set.CardBoxes);
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxesWithIncreasingRevisionDelay_NoExceptionsThrown()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(2));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(4));
            var thirdCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(3), new CardBoxRevisionDelay(7));
            
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
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(2));
            
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
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                new CardBoxSetId(Guid.NewGuid()), new CardBoxLevel(0), new CardBoxRevisionDelay(2));
            
            // Act && Assert
            Assert.Throws<DomainException.CardBoxSetIdMismatchException>(
                () => set.AddCardBox(cardBox));
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddSameCardBoxAgain_ThrowsCardBoxAlreadyInSetException()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(2));
            
            set.AddCardBox(cardBox);
            
            // Act && Assert
            Assert.Throws<DomainException.CardBoxAlreadyInSetException>(
                () => set.AddCardBox(cardBox));
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxesWithDecreasingRevisionDelay_ThrowsDecreasingRevisionDelayException()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(2));

            set.AddCardBox(firstCardBox);
            
            // Act && Assert
            Assert.Throws<DomainException.DecreasingRevisionDelayException>(
                () => set.AddCardBox(secondCardBox));
        }

        [Fact]
        public void CardBoxSetAddCardBox_AddCardBoxesWithSameLevel_ThrowsCardBoxLevelAlreadyExistsException()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(2));

            set.AddCardBox(firstCardBox);
            
            // Act && Assert
            Assert.Throws<DomainException.CardBoxLevelAlreadyExistException>(
                () => set.AddCardBox(secondCardBox));
        }

        [Fact]
        public void CardBoxSetRemoveCardBox_ContainedCardBoxPassed_RemovesCardBoxFromEnumerable()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));

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
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(7));

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
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            set.AddCardBox(cardBox);
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act
            set.AddNewCard(learningCard);

            // Assert
            Assert.Contains(set.CardBoxes, b =>
                b.LearningCards.Any(c =>
                    c.Id == learningCard.Id));
        }

        [Fact]
        public void CardBoxSetAddNewCard_CardNotContainedInBoxesPassed_AddsToLowestLevelBox()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(7));
            set.AddCardBox(firstCardBox);
            set.AddCardBox(secondCardBox);
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act
            set.AddNewCard(learningCard);

            // Assert
            Assert.Contains(firstCardBox.LearningCards, c => c.Id == learningCard.Id);
            Assert.Equal(firstCardBox.Id, learningCard.CardBoxId);
        }

        [Fact]
        public void CardBoxSetAddNewCard_CardAlreadyContainedInBoxPassed_ThrowsLearningCardAlreadyInSetException()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
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
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));
            
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
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(7));
            set.AddCardBox(firstCardBox);
            set.AddCardBox(secondCardBox);
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            set.AddNewCard(learningCard);
            var firstCardBoxChangeDate = learningCard.CardBoxChangedDate;
            
            // Act
            set.PromoteCardToNextLevel(learningCard);
            
            // Assert
            Assert.Contains(secondCardBox.LearningCards, c => c.Id == learningCard.Id);
            Assert.Equal(secondCardBox.Id, learningCard.CardBoxId);
            Assert.NotEqual(firstCardBoxChangeDate, learningCard.CardBoxChangedDate);
        }

        [Fact]
        public void CardBoxPromoteCardToNextLevel_NoNextLevelBox_NothingHappens()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            set.AddCardBox(cardBox);
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            set.AddNewCard(learningCard);
            var cardBoxChangeDate = learningCard.CardBoxChangedDate;
            
            // Act
            set.PromoteCardToNextLevel(learningCard);
            
            // Assert
            Assert.Contains(cardBox.LearningCards, c => c.Id == learningCard.Id);
            Assert.Equal(cardBoxChangeDate, learningCard.CardBoxChangedDate);
            Assert.Equal(cardBox.Id, learningCard.CardBoxId);
        }

        [Fact]
        public void CardBoxPromoteCardToNextLevel_NoSuchCardInSet_ThrowsLearningCardNotInSetException()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var cardBoxId = new CardBoxId(Guid.NewGuid());
            set.AddCardBox(new CardBox(cardBoxId,
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4)));
            
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            
            // Act && Assert
            Assert.Throws<DomainException.LearningCardNotInSetException>(
                () => set.PromoteCardToNextLevel(learningCard));
        }

        [Fact]
        public void CardBoxSetAddCardBox_PassCardBoxInAscendingLevelOrder_SetContainsBoxesInAscendingOrder()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(7));
            
            // Act
            set.AddCardBox(firstCardBox);
            set.AddCardBox(secondCardBox);
            
            // Assert
            Assert.Collection(set.CardBoxes, 
                b=> Assert.Equal(firstCardBox.Id, b.Id),
                b => Assert.Equal(secondCardBox.Id, b.Id));
        }

        [Fact]
        public void CardBoxSetAddCardBox_PassCardBoxInDescendingLevelOrder_SetContainsBoxesInAscendingOrder()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(7));
            
            // Act
            set.AddCardBox(secondCardBox);
            set.AddCardBox(firstCardBox);
            
            // Assert
            Assert.Collection(set.CardBoxes, 
                b=> Assert.Equal(firstCardBox.Id, b.Id),
                b => Assert.Equal(secondCardBox.Id, b.Id));
        }
        
        [Fact]
        public void CardBoxSetAddCardBox_PassCardBoxInVariousOrder_SetContainsBoxesInAscendingOrder()
        {
            // Arrange
            static bool TestValidator(string _) => true;
            var set = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), 
                new CardBoxSetLanguage("Russian", TestValidator),
                new CardBoxSetLanguage("Norwegian", TestValidator));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(0), new CardBoxRevisionDelay(4));
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(7));
            var thirdCardBox = new CardBox(new CardBoxId(Guid.NewGuid()),
                set.Id, new CardBoxLevel(2), new CardBoxRevisionDelay(10));
            
            // Act
            set.AddCardBox(thirdCardBox);
            set.AddCardBox(firstCardBox);
            set.AddCardBox(secondCardBox);
            
            // Assert
            Assert.Collection(set.CardBoxes, 
                b=> Assert.Equal(firstCardBox.Id, b.Id),
                b => Assert.Equal(secondCardBox.Id, b.Id),
                b => Assert.Equal(thirdCardBox.Id, b.Id));
        }

        [Fact]
        public void CardBoxSetCreateSnapshot_CalledOnAnyObject_ReturnsSnapshotOfTheInstance()
        {
            // Arrange
            var set = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            
            var box = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                set.Id,
                new CardBoxLevel(0),
                new CardBoxRevisionDelay(7));
            
            var card = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));

            set.AddCardBox(box);
            set.AddNewCard(card);
            
            // Act
            var snapshot = set.CreateSnapshot();
            
            // Assert
            Assert.Equal(set.Id.Value, snapshot.Id);
            Assert.Equal(set.NativeLanguage.Value, snapshot.NativeLanguage);
            Assert.Equal(set.TargetLanguage.Value, snapshot.TargetLanguage);
            Assert.Collection(set.CardBoxes,
                b =>
                {
                    Assert.Equal(box.Id, b.Id);
                    Assert.Collection(box.LearningCards,
                        c => Assert.Equal(card.Id, c.Id));
                });
        }

        private class TestSnapshot : ICardBoxSetSnapshot
        {
            public Guid Id { get; set; }
            public string NativeLanguage { get; set; }
            public string TargetLanguage { get; set; }
            public IEnumerable<ICardBoxSnapshot> CardBoxes { get; set; }
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
        public void CardBoxSetFromSnapshot_ValidObjectPassed_ReturnsRestoredObject()
        {
            // Arrange
            var cardSnapshot = new TestCardSnapshot
            {
                Comment = null,
                Id = Guid.NewGuid(),
                CardBoxId = null,
                NativeLanguageWord = "Привет",
                TargetLanguageWord = "Hei",
                CardBoxChangedDate = null
            };
            
            var boxSnapshot = new TestBoxSnapshot
            {
                Id = Guid.NewGuid(),
                Level = 0,
                RevisionDelay = 7,
                SetId = Guid.NewGuid(),
                LearningCards = new List<TestCardSnapshot>
                {
                    cardSnapshot
                }
            };

            var snapshot = new TestSnapshot
            {
                Id = boxSnapshot.SetId,
                NativeLanguage = "Russian",
                TargetLanguage = "Norwegian",
                CardBoxes = new List<TestBoxSnapshot>
                {
                    boxSnapshot
                }
            };
            
            // Act
            var set = CardBoxSet.FromSnapshot(snapshot);
            
            // Assert
            Assert.Equal(snapshot.Id, set.Id.Value);
            Assert.Equal(snapshot.NativeLanguage, set.NativeLanguage.Value);
            Assert.Equal(snapshot.TargetLanguage, set.TargetLanguage.Value);
            Assert.Collection(set.CardBoxes,
                b =>
                {
                    Assert.Equal(boxSnapshot.Id, b.Id.Value);
                    Assert.Collection(b.LearningCards, c =>
                            Assert.Equal(cardSnapshot.Id, c.Id.Value));
                });
        }

        [Fact]
        public void CardBoxSetFromSnapshot_JustCreatedSnapshotPassed_ReturnsObjectEqualToOriginal()
        {
            // Arrange
            var set = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            
            var box = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                set.Id,
                new CardBoxLevel(0),
                new CardBoxRevisionDelay(7));
            
            var card = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));

            set.AddCardBox(box);
            set.AddNewCard(card);

            var snapshot = set.CreateSnapshot();
            
            // Act
            var fromSnapshot = CardBoxSet.FromSnapshot(snapshot);
            
            // Assert
            Assert.Equal(set.Id, fromSnapshot.Id);
            Assert.Equal(set.NativeLanguage, fromSnapshot.NativeLanguage);
            Assert.Equal(set.TargetLanguage, fromSnapshot.TargetLanguage);
            Assert.Collection(set.CardBoxes,
                b => Assert.Equal(box.Id, b.Id));
        }
    }
}