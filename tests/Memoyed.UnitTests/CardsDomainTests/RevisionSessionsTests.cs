using System;
using System.Linq;
using Memoyed.Cards.Domain;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions;
using Memoyed.Cards.Domain.Shared;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class RevisionSessionsTests
    {
        [Fact]
        public void RevisionSessionConstructor_DontPassCardBoxId_TakesSessionCardsFromEntireSet()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            var cardBox1 = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(2));
            
            var cardBox2 = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(2), new CardBoxRevisionDelay(7));

            cardBoxSet.AddCardBox(cardBox1);
            cardBoxSet.AddCardBox(cardBox2);
            
            var card1 = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"), 
                new LearningCardWord("Hei"),
                null);

            cardBoxSet.AddNewCard(card1, new UtcTime(new DateTime(2020, 1, 1)));
            var card2 = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Дом"),
                new LearningCardWord("Hus"),
                null);
            cardBoxSet.AddNewCard(card2, new UtcTime(new DateTime(2020, 1, 1)));
            
            cardBoxSet.PromoteCard(card2.Id, new UtcTime(new DateTime(2020, 1, 1)));
            
            // Act
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, new UtcTime(new DateTime(2020, 1, 10)));
            
            // Assert
            Assert.Equal(2, session.SessionCards.Count());
            Assert.True(session.SessionCards.All(sc => sc.LearningCardId == card1.Id
                                                       || sc.LearningCardId == card2.Id));
        }
        
        [Fact]
        public void RevisionSessionConstructor_PassCardBoxId_TakesSessionCardsFromCardBox()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            var cardBox1 = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(2));
            
            var cardBox2 = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(2), new CardBoxRevisionDelay(7));

            cardBoxSet.AddCardBox(cardBox1);
            cardBoxSet.AddCardBox(cardBox2);
            
            var card1 = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"), 
                new LearningCardWord("Hei"),
                null);

            cardBoxSet.AddNewCard(card1, new UtcTime(new DateTime(2020, 1, 1)));
            var card2 = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Дом"),
                new LearningCardWord("Hus"),
                null);
            cardBoxSet.AddNewCard(card2, new UtcTime(new DateTime(2020, 1, 1)));
            
            cardBoxSet.PromoteCard(card2.Id, new UtcTime(new DateTime(2020, 1, 1)));
            
            // Act
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, cardBox2.Id, new UtcTime(new DateTime(2020, 1, 10)));
            
            // Assert
            Assert.Collection(session.SessionCards, sc =>
            {
                Assert.Equal(sc.LearningCardId, card2.Id);
            });
        }
        
        [Fact]
        public void RevisionSessionConstructor_PassCardBoxIdNotFromSet_ThrowsCardBoxNotFoundInSetException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            var cardBox1 = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(2));
            
            // Act && Assert
            Assert.Throws<DomainException.CardBoxNotFoundInSetException>(() =>
                new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, cardBox1.Id, new UtcTime(new DateTime(2020, 1, 10))));
        }
        
        [Fact]
        public void RevisionSessionConstructor_NoSessionCardsCreated_ThrowsNoCardsForRevisionException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            var cardBox = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(2));

            cardBoxSet.AddCardBox(cardBox);
            
            var card = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"), 
                new LearningCardWord("Hei"),
                null);

            cardBoxSet.AddNewCard(card, new UtcTime(new DateTime(2020, 1, 1)));
            
            // Act && Assert
            Assert.Throws<DomainException.NoCardsForRevisionException>(() => 
                new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, cardBox.Id, new UtcTime(new DateTime(2020, 1, 1))));
        }

        [Fact]
        public void RevisionSessionCardAnswered_PassWrongLearningCardId_ThrowsSessionCardNotFoundException()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));

            var cardBox = new CardBox(
                new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id, new CardBoxLevel(1), new CardBoxRevisionDelay(2));

            cardBoxSet.AddCardBox(cardBox);
            
            var card = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"), 
                new LearningCardWord("Hei"),
                null);

            cardBoxSet.AddNewCard(card, new UtcTime(new DateTime(2020, 1, 1)));
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, new UtcTime(new DateTime(2020, 1, 1)));
            
            // Act && Assert
            Assert.Throws<DomainException.SessionCardNotFoundException>(
                () => session.CardAnswered(new LearningCardId(Guid.NewGuid()), AnswerType.NativeLanguage,
                    "test"));
        }
    }
}