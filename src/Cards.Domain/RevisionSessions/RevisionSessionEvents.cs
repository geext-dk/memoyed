using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.Shared;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public static class RevisionSessionEvents
    {
        public class RevisionSessionCompleted
        {
            public RevisionSessionCompleted(RevisionSessionId sessionId,
                CardBoxSetId setId,
                IEnumerable<LearningCardId> answeredCorrectlyLearningCardIds,
                IEnumerable<LearningCardId> answeredWrongLearningCardIds,
                UtcTime dateTime)
            {
                RevisionSessionId = sessionId;
                CardBoxSetId = setId.Value;
                AnsweredSuccessfullyLearningCardIds = answeredCorrectlyLearningCardIds
                    .Select(c => c.Value)
                    .ToList();
                
                AnsweredWrongLearningCardsId = answeredWrongLearningCardIds
                    .Select(c => c.Value)
                    .ToList();

                DateTime = dateTime;
            }
            
            public Guid RevisionSessionId { get; }
            public Guid CardBoxSetId { get; }
            public List<Guid> AnsweredSuccessfullyLearningCardIds { get; }
            public List<Guid> AnsweredWrongLearningCardsId { get; }
            public DateTime DateTime { get; }
        }
    }
}