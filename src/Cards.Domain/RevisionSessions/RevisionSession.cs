using System.Collections.Generic;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions.SessionCards;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public class RevisionSession
    {
        private readonly List<SessionCard> _sessionCards = new List<SessionCard>();
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, CardBox cardBox)
        {
            Id = id;
        }

        public RevisionSessionId Id { get; }
        public IEnumerable<SessionCard> SessionCards { get; }

        public void CardAnswered(LearningCardId cardId, AnswerType targetLanguage, string word)
        {
            throw new System.NotImplementedException();
        }

        public void CompleteSession()
        {
            throw new System.NotImplementedException();
        }
    }
}