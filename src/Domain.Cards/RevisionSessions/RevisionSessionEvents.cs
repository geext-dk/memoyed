using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.Shared;

namespace Memoyed.Domain.Cards.RevisionSessions
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