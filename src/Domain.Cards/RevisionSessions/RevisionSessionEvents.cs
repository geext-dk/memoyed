using System;
using Memoyed.Domain.Cards.CardBoxSets;

namespace Memoyed.Domain.Cards.RevisionSessions
{
    public static class RevisionSessionEvents
    {
        public class RevisionSessionCompleted
        {
            public RevisionSessionCompleted(Guid sessionId,
                Guid setId,
                DateTimeOffset? dateTime = null)
            {
                RevisionSessionId = sessionId;
                CardBoxSetId = setId;
                DateTime = dateTime ?? DateTimeOffset.UtcNow;
            }

            public Guid RevisionSessionId { get; }
            public Guid CardBoxSetId { get; }
            public DateTimeOffset DateTime { get; }
        }
    }
}