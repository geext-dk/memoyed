using System;
using Memoyed.Cards.Domain;
using Memoyed.Cards.Domain.LearningCards;
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
            // Arrange
            LearningCardWord word;
            var throws = false;

            // Act
            try
            {
                word = new LearningCardWord(emptyString);
            }
            catch (DomainException.EmptyWordException)
            {
                throws = true;
            }

            // Assert
            Assert.True(throws);
        }

        [Theory]
        [InlineData("t")]
        [InlineData("test")]
        public void LearningCardWordConstructor_PassNonEmptyString_CreatesSuccessfully(string testString)
        {
            // Arrange
            LearningCardWord word;

            // Act
            word = new LearningCardWord(testString);

            // Assert
            Assert.Equal(testString, word.Value);
        }

        [Fact]
        public void LearningCardWordConstructor_PassNonTrimmedString_CreatesWithTrimmed()
        {
            // Arrange
            var nonTrimmed = "     test     ";
            LearningCardWord word;

            // Act
            word = new LearningCardWord(nonTrimmed);

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
            // Arrange
            LearningCardComment word;

            // Act
            word = new LearningCardComment(testString);

            // Assert
            Assert.Equal(testString, word.Value);
        }

        [Fact]
        public void LearningCardCommentConstructor_PassNonTrimmedString_CreatesWithTrimmed()
        {
            // Arrange
            LearningCardComment comment;
            var testString = "   test   ";

            // Act
            comment = new LearningCardComment(testString);

            // Assert
            Assert.Equal(testString.Trim(), comment.Value);
        }

        [Fact]
        public void LearningCard_CreateWithAnyArgs_PropertiesReturnPassedValues()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            LearningCard card;
            
            // Act
            card = new LearningCard(id, nativeLanguageWord, targetLanguageWord, comment);
            
            // Assert
            Assert.Equal(id.Value, card.Id.Value);
            Assert.Equal(nativeLanguageWord.Value, card.NativeLanguageWord.Value);
            Assert.Equal(targetLanguageWord.Value, card.TargetLanguageWord.Value);
            Assert.Equal(comment.Value, card.Comment.Value);
        }

        [Fact]
        public void LearningCardChangeTargetLanguageWord_PassAnyWord_PropertyReturnsNewValue()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            var card = new LearningCard(id, nativeLanguageWord, targetLanguageWord, comment);

            var newTargetLanguageWord = new LearningCardWord("Hello");
            
            // Act
            card.ChangeTargetLanguageWord(newTargetLanguageWord);
            
            // Assert
            Assert.Equal(newTargetLanguageWord.Value, card.TargetLanguageWord.Value);
        }
        
        [Fact]
        public void LearningCardChangeNativeLanguageWord_PassAnyWord_PropertyReturnsNewValue()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            var card = new LearningCard(id, nativeLanguageWord, targetLanguageWord, comment);

            var newNativeLanguageWord = new LearningCardWord("Здравствуйте");
            
            // Act
            card.ChangeNativeLanguageWord(newNativeLanguageWord);
            
            // Assert
            Assert.Equal(newNativeLanguageWord.Value, card.NativeLanguageWord.Value);
        }
        
        [Fact]
        public void LearningCardChangeComment_PassAnyWord_PropertyReturnsNewValue()
        {
            // Arrange
            var id = new LearningCardId(Guid.NewGuid());
            var nativeLanguageWord = new LearningCardWord("Привет");
            var targetLanguageWord = new LearningCardWord("Hei");
            var comment = new LearningCardComment("testComment");
            var card = new LearningCard(id, nativeLanguageWord, targetLanguageWord, comment);

            var newComment = new LearningCardComment("It is a greeting");
            
            // Act
            card.ChangeComment(newComment);
            
            // Assert
            Assert.Equal(newComment.Value, card.Comment.Value);
        }
    }
}