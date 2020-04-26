using System;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Shared;

namespace Memoyed.Domain.Cards.RevisionSessions
{
    public static class RevisionSessionEvents
    {
        public class RevisionSessionCompleted
        {
            public RevisionSessionCompleted(RevisionSessionId sessionId,
                CardBoxSetId setId,
                UtcTime? dateTime = null)
            {
                RevisionSessionId = sessionId;
                CardBoxSetId = setId.Value;
                DateTime = dateTime ?? new UtcTime(DateTimeOffset.UtcNow);
            }

            public Guid RevisionSessionId { get; }
            public Guid CardBoxSetId { get; }
            public DateTimeOffset DateTime { get; }
        }
    }
}