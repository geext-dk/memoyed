using System;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class RevisionSessionsTests
    {
        [Fact]
        public void RevisionSessionConstructor_DontPassCardBox_TakesSessionCardsFromEntireSet()
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

            cardBoxSet.AddNewCard(card1);
            var card2 = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Дом"),
                new LearningCardWord("Hus"),
                null);
            cardBoxSet.AddNewCard(card2);
            
            cardBoxSet.PromoteCard(card2.Id);
            
            // Act
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet);
            
            // Assert
            Assert.Equal(2, session.SessionCards.Count());
            Assert.True(session.SessionCards.All(sc => sc.LearningCardId == card1.Id
                                                       || sc.LearningCardId == card2.Id));
        }
    }
}