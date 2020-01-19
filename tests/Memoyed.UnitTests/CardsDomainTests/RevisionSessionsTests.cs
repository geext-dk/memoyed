using System;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions;
using Memoyed.Cards.Domain.RevisionSessions.SessionCards;
using Xunit;

namespace Memoyed.UnitTests.CardsDomainTests
{
    public class RevisionSessionsTests
    {
        [Fact]
        public void RevisionSession_CreatedCorrectly_PopulatesSessionCardsWithCardsFromBox()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            var box = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id,
                new CardBoxLevel(0), new CardBoxRevisionDelay(7));
            var box2 = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id,
                new CardBoxLevel(1), new CardBoxRevisionDelay(14));

            cardBoxSet.AddCardBox(box);
            cardBoxSet.AddCardBox(box2);

            var card = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            cardBoxSet.AddNewCard(card);
            
            // Act
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, box);
            
            // Assert
            Assert.Collection(session.SessionCards,
                sc => Assert.Equal(card.Id, sc.LearningCardId));
        }

        [Fact]
        public void RevisionSession_ExistingCardAnswered_MarkCardAsAnswered()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            var box = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id,
                new CardBoxLevel(0), new CardBoxRevisionDelay(7));
            var box2 = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id,
                new CardBoxLevel(1), new CardBoxRevisionDelay(14));

            cardBoxSet.AddCardBox(box);
            cardBoxSet.AddCardBox(box2);

            var card = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            cardBoxSet.AddNewCard(card);
            
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, box);
            
            // Act
            session.CardAnswered(card.Id, AnswerType.TargetLanguage, "Hei");
            
            // Assert
            Assert.Collection(session.SessionCards,
                sc =>
                {
                    Assert.Equal(card.Id, sc.LearningCardId);
                    Assert.Equal(SessionCardStatus.CorrectAnswer, sc.Status);
                });
        }

        [Fact]
        public void RevisionSession_SessionEnded_AnsweredCardPromoteToNextLevel()
        {
            // Arrange
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetLanguage("Russian", _ => true),
                new CardBoxSetLanguage("Norwegian", _ => true));
            var box = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id,
                new CardBoxLevel(0), new CardBoxRevisionDelay(7));
            var box2 = new CardBox(new CardBoxId(Guid.NewGuid()), cardBoxSet.Id,
                new CardBoxLevel(1), new CardBoxRevisionDelay(14));

            cardBoxSet.AddCardBox(box);
            cardBoxSet.AddCardBox(box2);

            var card = new LearningCard(new LearningCardId(Guid.NewGuid()),
                new LearningCardWord("Привет"),
                new LearningCardWord("Hei"),
                new LearningCardComment(null));
            cardBoxSet.AddNewCard(card);
            
            var session = new RevisionSession(new RevisionSessionId(Guid.NewGuid()),
                cardBoxSet, box);
            
            session.CardAnswered(card.Id, AnswerType.TargetLanguage, "Hei");

            // Act
            session.CompleteSession();
            
            // Assert
            Assert.DoesNotContain(card, box.LearningCards);
            Assert.Contains(card, box.LearningCards);
        }
    }
}