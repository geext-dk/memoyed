using System;
using Memoyed.Cards.Domain;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.Shared;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class LearningCardTests
    {
        [Fact]
        public void LearningCardIdConstructor_EmptyGuidPassed_ThrowsEmptyIdException()
        {
            // Arrange
            var emptyGuid = Guid.Empty;
            LearningCardId id = null;

            // Act
            try
            {
                id = new LearningCardId(emptyGuid);
            }
            catch (DomainException.EmptyIdException)
            {
            }

            // Assert
            Assert.Null(id);
        }

        [Fact]
        public void LearningCardIdConstructor_NonEmptyGuidPassed_ConstructsSuccessfully()
        {
            // Arrange
            var nonEmptyGuid = Guid.NewGuid();
            LearningCardId id;

            // Act
            id = new LearningCardId(nonEmptyGuid);

            // Assert
            Assert.Equal(nonEmptyGuid, id.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void LearningCardWordConstructor_PassNull_ThrowsEmptyWordDomainException(string emptyString)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException.EmptyWordException>(
                () => new LearningCardWord(emptyString));
        }

        [Theory]
        [InlineData("t")]
        [InlineData("test")]
        public void LearningCardWordConstructor_PassNonEmptyString_CreatesSuccessfully(string testString)
        {
            // Act
            var word = new LearningCardWord(testString);

            // Assert
            Assert.Equal(testString, word.Value);
        }

        [Fact]
        public void LearningCardWordConstructor_PassNonTrimmedString_CreatesWithTrimmed()
        {
            // Arrange
            const string nonTrimmed = "     test     ";

            // Act
            var word = new LearningCardWord(nonTrimmed);

            // Assert
            Assert.Equal(nonTrimmed.Trim(), word.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("TestString")]
        public void LearningCardCommentConstructor_PassAnyTrimmedString_CreatesSuccessfully(string testString)
        {
            // Act
            var word = new LearningCardComment(testString);

            // Assert
            Assert.Equal(testString, word.Value);
        }

        [Fact]
        public void LearningCardCommentConstructor_PassNonTrimmedString_CreatesWithTrimmed()
        {
            // Arrange
            const string testString = "   test   ";

            // Act
            var comment = new LearningCardComment(testString);

            // Assert
            Assert.Equal(testString.Trim(), comment.Value);
        }

        [Fact]
        public void LearningCard_CreateWithAnyArgs_InitializedProperly()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            
            // Act
            var card = new LearningCard(id, nativeLanguageWord, 
                targetLanguageWord, comment);
            
            // Assert
            Assert.Equal(id, card.Id);
            Assert.Equal(nativeLanguageWord, card.NativeLanguageWord);
            Assert.Equal(targetLanguageWord, card.TargetLanguageWord);
            Assert.Equal(comment, card.Comment);
            Assert.Null(card.CardBoxChangedDate);
            Assert.Null(card.CardBoxId);
        }

        [Fact]
        public void LearningCardChangeTargetLanguageWord_PassAnyWord_PropertyReturnsNewValue()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            var card = new LearningCard(id,
                nativeLanguageWord, targetLanguageWord, comment);

            var newTargetLanguageWord = new LearningCardWord("Hello");
            
            // Act
            card.ChangeTargetLanguageWord(newTargetLanguageWord);
            
            // Assert
            Assert.Equal(newTargetLanguageWord, card.TargetLanguageWord);
        }
        
        [Fact]
        public void LearningCardChangeNativeLanguageWord_PassAnyWord_PropertyReturnsNewValue()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            var card = new LearningCard(id, nativeLanguageWord,
                targetLanguageWord, comment);

            var newNativeLanguageWord = new LearningCardWord("Здравствуйте");
            
            // Act
            card.ChangeNativeLanguageWord(newNativeLanguageWord);
            
            // Assert
            Assert.Equal(newNativeLanguageWord, card.NativeLanguageWord);
        }
        
        [Fact]
        public void LearningCardChangeComment_PassAnyWord_PropertyReturnsNewValue()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            var card = new LearningCard(id,
                nativeLanguageWord, targetLanguageWord, comment);

            var newComment = new LearningCardComment("It is a greeting");
            
            // Act
            card.ChangeComment(newComment);
            
            // Assert
            Assert.Equal(newComment, card.Comment);
        }

        [Fact]
        public void LearningCardCreateSnapshot_CalledOnAnyObject_ReturnsSnapshotOfTheInstance()
        {
            // Arrange
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment("Test"));
            
            // Let's change cardBoxSetId of the card
            var box = new CardBox(new CardBoxId(Guid.NewGuid()),
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxLevel(0),
                new CardBoxRevisionDelay(7));
            
            var set = new CardBoxSet(
                box.SetId,
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            set.AddCardBox(box);
            set.AddNewCard(learningCard, new UtcTime(DateTime.UtcNow));
            
            // Act
            var snapshot = learningCard.CreateSnapshot();
            
            // Assert
            Assert.Equal(learningCard.Id.Value, snapshot.Id);
            Assert.Equal(learningCard.Comment.Value, snapshot.Comment);
            Assert.Equal(learningCard.CardBoxId.Value, snapshot.CardBoxId);
            Assert.Equal(learningCard.NativeLanguageWord.Value, snapshot.NativeLanguageWord);
            Assert.Equal(learningCard.TargetLanguageWord.Value, snapshot.TargetLanguageWord);
            Assert.Equal(learningCard.CardBoxChangedDate, snapshot.CardBoxChangedDate);
        }
        
        private class TestSnapshot : ILearningCardSnapshot
        {
            public Guid Id { get; set; }
            public Guid? CardBoxId { get; set; }
            public string NativeLanguageWord { get; set; }
            public string TargetLanguageWord { get; set; }
            public string Comment { get; set; }
            public DateTime? CardBoxChangedDate { get; set; }
        }

        [Fact]
        public void LearningCardFromSnapshot_ValidSnapshotPassed_ReturnsRestoredObject()
        {
            // Arrange
            var snapshot = new TestSnapshot
            {
                Id = Guid.NewGuid(),
                CardBoxId = Guid.NewGuid(),
                NativeLanguageWord = "Привет",
                TargetLanguageWord = "Hei",
                Comment = "Test",
                CardBoxChangedDate = DateTime.UtcNow
            };
            
            // Act
            var learningCard = LearningCard.FromSnapshot(snapshot);
            
            // Assert
            Assert.Equal(snapshot.Id, learningCard.Id.Value);
            Assert.Equal(snapshot.CardBoxId, learningCard.CardBoxId.Value);
            Assert.Equal(snapshot.NativeLanguageWord, learningCard.NativeLanguageWord.Value);
            Assert.Equal(snapshot.TargetLanguageWord, learningCard.TargetLanguageWord.Value);
            Assert.Equal(snapshot.Comment, learningCard.Comment.Value);
            Assert.Equal(snapshot.CardBoxChangedDate, learningCard.CardBoxChangedDate);
        }

        [Fact]
        public void LearningCardFromSnapshot_JustCreatedSnapshotPassed_ReturnsObjectEqualToOriginal()
        {
            // Arrange
            var learningCard = new LearningCard(
                new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment("Test"));
            
            // Let's change cardBoxSetId of the card
            var box = new CardBox(new CardBoxId(Guid.NewGuid()),
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxLevel(0),
                new CardBoxRevisionDelay(7));
            
            var set = new CardBoxSet(
                box.SetId,
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            set.AddCardBox(box);
            set.AddNewCard(learningCard, new UtcTime(DateTime.UtcNow));

            var snapshot = learningCard.CreateSnapshot();
            
            // Act
            var fromSnapshot = LearningCard.FromSnapshot(snapshot);
            
            // Assert
            Assert.Equal(learningCard.Id, fromSnapshot.Id);
            Assert.Equal(learningCard.CardBoxId, fromSnapshot.CardBoxId);
            Assert.Equal(learningCard.NativeLanguageWord, fromSnapshot.NativeLanguageWord);
            Assert.Equal(learningCard.TargetLanguageWord, fromSnapshot.TargetLanguageWord);
            Assert.Equal(learningCard.Comment, fromSnapshot.Comment);
            Assert.Equal(learningCard.CardBoxChangedDate, fromSnapshot.CardBoxChangedDate);
        }
    }
}