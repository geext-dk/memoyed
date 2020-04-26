using System;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;

namespace Memoyed.Application.Dto
{
    public static class ReturnModels
    {
        public class CardBoxSetModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string NativeLanguage { get; set; }
            public string TargetLanguage { get; set; }
        }

        public class CardBoxModel
        {
            public Guid Id { get; set; }
            public Guid SetId { get; set; }
            public int Level { get; set; }
            public int RevisionDelay { get; set; }
        }

        public class CardModel
        {
            public Guid Id { get; set; }
            public Guid SetId { get; set; }
            public Guid? CardBoxId { get; set; }
            public int? Level { get; set; }
            public DateTimeOffset? RevisionAllowedDate { get; set; }
            public string TargetLanguageWord { get; set; }
            public string NativeLanguageWord { get; set; }
            public string Comment { get; set; }
        }

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