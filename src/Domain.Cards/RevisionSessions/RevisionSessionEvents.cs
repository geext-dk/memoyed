using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.Cards;
using Memoyed.Cards.Domain.Shared;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public static class RevisionSessionEvents
    {
        public class RevisionSessionCompleted
        {
            public RevisionSessionCompleted(RevisionSessionId sessionId,
                CardBoxSetId setId,
                IEnumerable<CardId> answeredCorrectlyCardIds,
                IEnumerable<CardId> answeredWrongCardIds,
                UtcTime dateTime)
            {
                RevisionSessionId = sessionId;
                CardBoxSetId = setId.Value;
                AnsweredSuccessfullyCardIds = answeredCorrectlyCardIds
                    .Select(c => c.Value)
                    .ToList();
                
                AnsweredWrongCardsId = answeredWrongCardIds
                    .Select(c => c.Value)
                    .ToList();

                DateTime = dateTime;
            }
            
            public Guid RevisionSessionId { get; }
            public Guid CardBoxSetId { get; }
            public List<Guid> AnsweredSuccessfullyCardIds { get; }
            public List<Guid> AnsweredWrongCardsId { get; }
            public DateTime DateTime { get; }
        }
    }
}