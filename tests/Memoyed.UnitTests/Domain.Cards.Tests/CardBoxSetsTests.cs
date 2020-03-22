using System;
using System.Linq;
using Memoyed.Domain.Cards;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.Shared;
using Xunit;

namespace Memoyed.UnitTests.Domain.Cards.Tests
{
    public class CardBoxSetsTests
    {
        #region Constructor

        [Fact]
        public void Constructor_CreateCardBoxSet_SuccessfullyAssignsArguments()
        {
            // Arrange
            var id = new CardBoxSetId(Guid.NewGuid());
            var ownerId = new CardBoxSetOwnerId(Guid.NewGuid());
            var name = new CardBoxSetName("Test Name");
            var nativeLanguage = new CardBoxSetLanguage("Russian", _ => true);
            var targetLanguage = new CardBoxSetLanguage("Finnish", _ => true);
            
            // Act
            var cardBoxSet = new CardBoxSet(id, ownerId, name, nativeLanguage, targetLanguage);
            
            // Assert
            Assert.Equal(id, cardBoxSet.Id);
            Assert.Equal(ownerId, cardBoxSet.OwnerId);
            Assert.Equal(name, cardBoxSet.Name);
            Assert.Equal(nativeLanguage, cardBoxSet.NativeLanguage);
            Assert.Equal(targetLanguage, cardBoxSet.TargetLanguage);
            Assert.Empty(cardBoxSet.CardBoxes);
            Assert.Empty(cardBoxSet.CompletedRevisionSessionIds);
        }
        
        #endregion Constructor
        
        #region Rename
        
        [Fact]
        public void Rename_PassValidName_CardBoxSetChangesName()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"), new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var newName = new CardBoxSetName("New Name");
            
            // Act
            cardBoxSet.Rename(newName);
            
            // Assert
            Assert.Equal(newName, cardBoxSet.Name);
        }

        #endregion Rename
        
        #region StartRevisionSession

        [Fact]
        public void StartRevisionSession_CardReadyForRevision_ReturnsSessionWithTheCard()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            
            var newCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(newCardBox);
            
            var card = new Card(new CardId(Guid.NewGuid()), new CardWord("Привет"), new CardWord("Moi"));
            
            cardBoxSet.AddNewCard(card, new UtcTime(new DateTime(2020, 2, 16)));
            
            // Act
            var revision = cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 20)));
            
            // Assert
            Assert.Equal(RevisionSessionStatus.Started, revision.Status);
            Assert.Single(revision.SessionCards);
            var sessionCard = revision.SessionCards.First();
            Assert.Equal(card.Id, sessionCard.CardId);
            Assert.Equal(card.TargetLanguageWord, sessionCard.TargetLanguageWord);
            Assert.Equal(card.NativeLanguageWord, sessionCard.NativeLanguageWord);
        }

        [Fact]
        public void StartRevisionSession_CardNotReadyForRevision_ReturnsSessionWithoutTheCard()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            
            var newCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(newCardBox);
            
            var notReady = new Card(new CardId(Guid.NewGuid()), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(notReady, new UtcTime(new DateTime(2020, 2, 20)));
            
            var ready = new Card(new CardId(Guid.NewGuid()), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(ready, new UtcTime(new DateTime(2020, 2, 16)));
            
            // Act
            var revision = cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 20)));
            
            // Assert
            Assert.Equal(RevisionSessionStatus.Started, revision.Status);
            Assert.Single(revision.SessionCards);
            Assert.Equal(ready.Id, revision.SessionCards.First().CardId);
        }

        [Fact]
        public void StartRevisionSession_CreateSessionWithoutCards_ThrowsNoCardsForRevisionException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            
            // Act & Assert
            Assert.Throws<DomainException.NoCardsForRevisionException>(
                () => cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 20))));
        }

        #endregion StartRevisionSession
        
        #region ProcessCardsFromRevisionSession

        [Fact]
        public void ProcessCardsFromRevisionSession_HasAnsweredCorrectlyCard_PromotesCardToNextLevel()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);
            
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);
            
            var card = new Card(new CardId(Guid.NewGuid()), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(card, new UtcTime(new DateTime(2020, 2, 16)));
            
            var revision = cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 20)));
            
            revision.CardAnsweredCorrectly(card.Id);
            revision.CompleteSession();
            
            // Act
            cardBoxSet.ProcessCardsFromRevisionSession(revision);
            
            // Assert
            Assert.Single(secondCardBox.Cards);
        }

        [Fact]
        public void ProcessCardsFromRevisionSession_HasAnsweredWrongCard_DemotesCardToFirstLevel()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);
            
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);
            
            var card = new Card(new CardId(Guid.NewGuid()), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(card, new UtcTime(new DateTime(2020, 2, 16)));
            
            var firstRevision = cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 20)));
            firstRevision.CardAnsweredCorrectly(card.Id);
            firstRevision.CompleteSession();
            cardBoxSet.ProcessCardsFromRevisionSession(firstRevision, new UtcTime(new DateTime(2020, 2, 20)));

            var secondRevision = cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 26)));
            secondRevision.CardAnsweredWrong(card.Id);
            secondRevision.CompleteSession();
            
            // Act
            cardBoxSet.ProcessCardsFromRevisionSession(secondRevision, new UtcTime(new DateTime(2020, 2, 26)));
            
            // Assert
            Assert.Single(firstCardBox.Cards);
        }

        [Fact]
        public void ProcessCardsFromRevisionSession_SessionNotCompleted_ThrowsRevisionSessionNotCompletedException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);
            
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);
            
            var card = new Card(new CardId(Guid.NewGuid()), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(card, new UtcTime(new DateTime(2020, 2, 16)));
            
            var revisionSession = cardBoxSet.StartRevisionSession(new UtcTime(new DateTime(2020, 2, 20)));
            revisionSession.CardAnsweredCorrectly(card.Id);

            // Act & Assert
            Assert.Throws<DomainException.RevisionSessionNotCompletedException>(
                () => cardBoxSet.ProcessCardsFromRevisionSession(revisionSession,
                    new UtcTime(new DateTime(2020, 2, 20))));
        }
        
        #endregion ProcessCardsFromRevisionSession
        
        #region AddCardBox

        [Fact]
        public void AddCardBox_AddValidCardBox_AddsCardBoxToSet()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            
            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            
            // Act
            cardBoxSet.AddCardBox(cardBox);
            
            // Assert
            Assert.Single(cardBoxSet.CardBoxes);
            Assert.Equal(cardBox.Id, cardBoxSet.CardBoxes.First().Id);
        }

        [Fact]
        public void AddCardBox_AddValidSecondCardBox_AddsCardBoxToSet()
                 {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);
            
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            
            // Act
            cardBoxSet.AddCardBox(secondCardBox);
            
            // Assert
            Assert.Equal(2, cardBoxSet.CardBoxes.Count);
        }

        [Fact]
        public void AddCardBox_AddCardBoxFromOtherSet_ThrowsCardBoxSetIdMismatchException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()), new CardBoxSetId(Guid.NewGuid()),
                new CardBoxLevel(1), new CardBoxRevisionDelay(3));
            
            // Act & Assert
            Assert.Throws<DomainException.CardBoxSetIdMismatchException>(
                () => cardBoxSet.AddCardBox(cardBox));
        }

        [Fact]
        public void AddCardBox_AddAlreadyContainedCardBox_ThrowsCardBoxAlreadyInSetException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(cardBox);
            
            // Act & Assert
            Assert.Throws<DomainException.CardBoxAlreadyInSetException>(
                () => cardBoxSet.AddCardBox(cardBox));
        }

        [Fact]
        public void AddCardBox_AddCardBoxWithSameLevel_ThrowsCardBoxLevelAlreadyExistException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);
            
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(5));
            
            // Act & Assert
            Assert.Throws<DomainException.CardBoxLevelAlreadyExistException>(
                () => cardBoxSet.AddCardBox(secondCardBox));
        }

        [Fact]
        public void AddCardBox_AddCardBoxWithDecreasingDelay_ThrowsDecreasingRevisionDelayException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);
            
            var secondCardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(1));
            
            // Act & Assert
            Assert.Throws<DomainException.DecreasingRevisionDelayException>(
                () => cardBoxSet.AddCardBox(secondCardBox));
        }
        
        #endregion AddCardBox
        
        #region RemoveCardBox

        [Fact]
        public void RemoveCardBox_PassContainedCardBox_RemovesCardBoxFromSet()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(cardBox);
            
            // Act
            cardBoxSet.RemoveCardBox(cardBox.Id);
            
            // Assert
            Assert.Empty(cardBoxSet.CardBoxes);
        }
        
        [Fact]
        public void RemoveCardBox_PassNotContainedCardBox_ThrowsCardBoxNotFoundException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            
            // Act & Assert
            Assert.Throws<DomainException.CardBoxNotFoundInSetException>(() => cardBoxSet.RemoveCardBox(cardBox.Id));
        }

        #endregion RemoveCardBox
    }
}