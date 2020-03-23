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
                UtcTime dateTime)
            {
                RevisionSessionId = sessionId.Id;
                CardBoxSetId = setId.Id;
                DateTime = dateTime.Time;
            }

            public Guid RevisionSessionId { get; }
            public Guid CardBoxSetId { get; }
            public DateTime DateTime { get; }
        }
    }
}