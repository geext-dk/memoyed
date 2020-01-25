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
        
        /// <summary>
        /// Creates a revision session with cards from the cardBox specified in the arguments
        /// </summary>
        /// <param name="id">Id of the revision session</param>
        /// <param name="cardBoxSet">Set of card boxes for which the session is created</param>
        /// <param name="cardBox">Card box for which the session is created</param>
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, CardBox cardBox)
        {
            Id = id;
        }

        /// <summary>
        /// Creates a revision session with cards from the entire set (which are ready for revision)
        /// </summary>
        /// <param name="id">Id of the revision session</param>
        /// <param name="cardBoxSet">Set of card boxes for which the session is created</param>
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet)
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