﻿using System;
using System.Linq;
using Memoyed.Domain.Cards;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.Services;
using Xunit;

namespace Memoyed.UnitTests.Domain.Cards.Tests
{
    public class CardBoxSetsTests
    {
        [Fact]
        public void AddCardBox_AddAlreadyContainedCardBox_ThrowsCardBoxAlreadyInSetException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(cardBox);

            // Act & Assert
            Assert.Throws<DomainException.CardBoxAlreadyInSetException>(
                () => cardBoxSet.AddCardBox(cardBox));
        }

        [Fact]
        public void AddCardBox_AddCardBoxFromOtherSet_ThrowsCardBoxSetIdMismatchException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), Guid.NewGuid(),
                new CardBoxLevel(1), new CardBoxRevisionDelay(3));

            // Act & Assert
            Assert.Throws<DomainException.CardBoxSetIdMismatchException>(
                () => cardBoxSet.AddCardBox(cardBox));
        }

        [Fact]
        public void AddCardBox_AddCardBoxWithDecreasingDelay_ThrowsDecreasingRevisionDelayException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(1));

            // Act & Assert
            Assert.Throws<DomainException.DecreasingRevisionDelayException>(
                () => cardBoxSet.AddCardBox(secondCardBox));
        }

        [Fact]
        public void AddCardBox_AddCardBoxWithSameLevel_ThrowsCardBoxLevelAlreadyExistException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(5));

            // Act & Assert
            Assert.Throws<DomainException.CardBoxLevelAlreadyExistException>(
                () => cardBoxSet.AddCardBox(secondCardBox));
        }

        [Fact]
        public void AddCardBox_AddValidCardBox_AddsCardBoxToSet()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
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
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));

            // Act
            cardBoxSet.AddCardBox(secondCardBox);

            // Assert
            Assert.Equal(2, cardBoxSet.CardBoxes.Count);
        }


        [Fact]
        public void AddNewCard_CardAlreadyExists_ThrowsCardAlreadyInSetException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(cardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"),
                new CardWord("Hei"));
            cardBoxSet.AddNewCard(card);

            // Act & Assert
            Assert.Throws<DomainException.CardAlreadyInSetException>(
                () => cardBoxSet.AddNewCard(card));
        }

        [Fact]
        public void AddNewCard_SetWithBoxesNotContainedCard_SuccessfullyAddssCardToLowestLevelBox()
        {
            // Arrange
            var now = new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero);

            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Hei"));

            // Act
            cardBoxSet.AddNewCard(card, now);

            // Assert
            Assert.Single(firstCardBox.Cards);
            Assert.Empty(secondCardBox.Cards);
            Assert.Equal(firstCardBox.Id, card.CardBoxId);
            Assert.Equal(now, card.CardBoxChangedDate);
        }

        [Fact]
        public void AddNewCard_SetWithoutBoxes_ThrowsNoBoxesInSetException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"),
                new CardWord("Hei"));

            // Act & Assert
            Assert.Throws<DomainException.NoBoxesInSetException>(
                () => cardBoxSet.AddNewCard(card));
        }

        [Fact]
        public void Constructor_CreateCardBoxSet_SuccessfullyAssignsArguments()
        {
            // Arrange
            var id = Guid.NewGuid();
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

        private class TestTrueCardAnswerCheckService : ICardAnswerCheckService
        {
            public bool CheckAnswer(string word, string answer)
            {
                return true;
            }
        }

        private class TestFalseCardAnswerCheckService : ICardAnswerCheckService
        {
            public bool CheckAnswer(string word, string answer)
            {
                return false;
            }
        }

        [Fact]
        public void ProcessCardsFromRevisionSession_HasAnsweredCorrectlyCard_PromotesCardToNextLevel()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(card, new DateTimeOffset(2020, 2, 16, 0, 0, 0, TimeSpan.Zero));

            var now = new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero);
            var revision = cardBoxSet.StartRevisionSession(now);

            revision.CardAnswered(card.Id, SessionCardAnswerType.TargetLanguage, "",
                new TestTrueCardAnswerCheckService());
            revision.CompleteSession();

            // Act
            cardBoxSet.ProcessCardsFromRevisionSession(revision, now);

            // Assert
            Assert.Single(secondCardBox.Cards);
            Assert.Equal(card.Id, secondCardBox.Cards.First().Id);
            Assert.Equal(secondCardBox.Id, card.CardBoxId);
            Assert.Equal(now, card.CardBoxChangedDate);
        }

        [Fact]
        public void ProcessCardsFromRevisionSession_HasAnsweredWrongCard_DemotesCardToFirstLevel()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(card, new DateTimeOffset(2020, 2, 16, 0, 0, 0, TimeSpan.Zero));

            var firstRevision = cardBoxSet.StartRevisionSession(new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero));
            firstRevision.CardAnswered(card.Id, SessionCardAnswerType.TargetLanguage, "",
                new TestTrueCardAnswerCheckService());
            firstRevision.CompleteSession();
            cardBoxSet.ProcessCardsFromRevisionSession(firstRevision, new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero));

            var secondRevision = cardBoxSet.StartRevisionSession(new DateTimeOffset(2020, 2, 26, 0, 0, 0, TimeSpan.Zero));
            secondRevision.CardAnswered(card.Id, SessionCardAnswerType.TargetLanguage, "",
                new TestFalseCardAnswerCheckService());
            secondRevision.CompleteSession();

            // Act
            cardBoxSet.ProcessCardsFromRevisionSession(secondRevision,
                new DateTimeOffset(2020, 2, 26, 0, 0, 0, TimeSpan.Zero));

            // Assert
            Assert.Single(firstCardBox.Cards);
            Assert.Equal(card.Id, firstCardBox.Cards.First().Id);
            Assert.Equal(firstCardBox.Id, card.CardBoxId);
        }

        [Fact]
        public void ProcessCardsFromRevisionSession_SessionNotCompleted_ThrowsRevisionSessionNotCompletedException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var firstCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(firstCardBox);

            var secondCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(2),
                new CardBoxRevisionDelay(5));
            cardBoxSet.AddCardBox(secondCardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(card, new DateTimeOffset(2020, 2, 16, 0, 0, 0, TimeSpan.Zero));

            var revisionSession = cardBoxSet.StartRevisionSession(new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero));
            revisionSession.CardAnswered(card.Id, SessionCardAnswerType.TargetLanguage, "",
                new TestTrueCardAnswerCheckService());

            // Act & Assert
            Assert.Throws<DomainException.RevisionSessionNotCompletedException>(
                () => cardBoxSet.ProcessCardsFromRevisionSession(revisionSession,
                    new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero)));
        }

        [Fact]
        public void RemoveCard_PassContainedCard_CardSuccessfullyRemoved()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(cardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"),
                new CardWord("Hei"));
            cardBoxSet.AddNewCard(card);

            // Act
            cardBoxSet.RemoveCard(card.Id);

            // Assert
            Assert.Empty(cardBox.Cards);
        }

        [Fact]
        public void RemoveCard_PassNotContainedCard_ThrowsCardNotInSetException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(cardBox);

            var cardId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException.CardNotInSetException>(
                () => cardBoxSet.RemoveCard(cardId));
        }

        [Fact]
        public void RemoveCardBox_PassContainedCardBox_RemovesCardBoxFromSet()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
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
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var cardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));

            // Act & Assert
            Assert.Throws<DomainException.CardBoxNotFoundInSetException>(() => cardBoxSet.RemoveCardBox(cardBox.Id));
        }

        [Fact]
        public void Rename_PassValidName_CardBoxSetChangesName()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(Guid.NewGuid(), new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"), new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));
            var newName = new CardBoxSetName("New Name");

            // Act
            cardBoxSet.Rename(newName);

            // Assert
            Assert.Equal(newName, cardBoxSet.Name);
        }

        [Fact]
        public void StartRevisionSession_CardNotReadyForRevision_ReturnsSessionWithoutTheCard()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var newCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(newCardBox);

            var notReady = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(notReady, new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero));

            var ready = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Moi"));
            cardBoxSet.AddNewCard(ready, new DateTimeOffset(2020, 2, 16, 0, 0, 0, TimeSpan.Zero));

            // Act
            var revision = cardBoxSet.StartRevisionSession(new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero));

            // Assert
            Assert.Equal(RevisionSessionStatus.Started, revision.Status);
            Assert.Single(revision.SessionCards);
            Assert.Equal(ready.Id, revision.SessionCards.First().CardId);
        }

        [Fact]
        public void StartRevisionSession_CardReadyForRevision_ReturnsSessionWithTheCard()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            var newCardBox = new CardBox(Guid.NewGuid(), cardBoxSet.Id, new CardBoxLevel(1),
                new CardBoxRevisionDelay(3));
            cardBoxSet.AddCardBox(newCardBox);

            var card = new Card(Guid.NewGuid(), new CardWord("Привет"), new CardWord("Moi"));

            cardBoxSet.AddNewCard(card, new DateTimeOffset(2020, 2, 16, 0, 0, 0, TimeSpan.Zero));

            // Act
            var revision = cardBoxSet.StartRevisionSession(new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero));

            // Assert
            Assert.Equal(RevisionSessionStatus.Started, revision.Status);
            Assert.Single(revision.SessionCards);
            var sessionCard = revision.SessionCards.First();
            Assert.Equal(card.Id, sessionCard.CardId);
            Assert.Equal(card.TargetLanguageWord, sessionCard.TargetLanguageWord);
            Assert.Equal(card.NativeLanguageWord, sessionCard.NativeLanguageWord);
        }

        [Fact]
        public void StartRevisionSession_CreateSessionWithoutCards_ThrowsNoCardsForRevisionException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(
                Guid.NewGuid(),
                new CardBoxSetOwnerId(Guid.NewGuid()),
                new CardBoxSetName("Test Name"),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Finnish", _ => true));

            // Act & Assert
            Assert.Throws<DomainException.NoCardsForRevisionException>(
                () => cardBoxSet.StartRevisionSession(new DateTimeOffset(2020, 2, 20, 0, 0, 0, TimeSpan.Zero)));
        }
    }
}