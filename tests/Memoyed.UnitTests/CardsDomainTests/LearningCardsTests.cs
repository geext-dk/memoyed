using System;
using Memoyed.Domain.Cards;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class LearningCardTests
    {
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
    }
}