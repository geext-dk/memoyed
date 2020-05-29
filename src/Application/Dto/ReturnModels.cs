using System;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;

namespace Memoyed.Application.Dto
{
    public static class ReturnModels
    {
        public class RevisionSessionModel
        {
            public Guid Id { get; set; }
            public Guid CardBoxSetId { get; set; }
            public RevisionSessionStatus Status { get; set; }
        }

        public class SessionCardModel
        {
            public Guid SessionId { get; set; }
            public Guid CardId { get; set; }
            public string TargetLanguageWord { get; set; }
            public string NativeLanguageWord { get; set; }
            public SessionCardStatus Status { get; set; }
        }
    }
}