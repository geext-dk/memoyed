using System;
using Memoyed.Domain.Cards.RevisionSessions;

namespace Memoyed.Application.Dto
{
    public static class Commands
    {
        public class CreateCardBoxSetCommand
        {
            public string Name { get; set; }
            public string TargetLanguage { get; set; }
            public string NativeLanguage { get; set; }
        }

        public class CreateCardBoxCommand
        {
            public Guid CardBoxSetId { get; set; }

            public int Level { get; set; }
            public int RevisionDelay { get; set; }
        }

        public class CreateCardCommand
        {
            public Guid CardBoxSetId { get; set; }
            public string NativeLanguageWord { get; set; }
            public string TargetLanguageWord { get; set; }
            public string? Comment { get; set; }
        }

        public class RemoveCardCommand
        {
            public Guid CardBoxSetId { get; set; }
            public Guid CardId { get; set; }
        }

        public class RenameCardBoxSetCommand
        {
            public Guid CardBoxSetId { get; set; }
            public string CardBoxSetName { get; set; }
        }

        public class StartRevisionSessionCommand
        {
            public Guid CardBoxSetId { get; set; }
        }
        
        public class SetCardAnswerCommand
        {
            public Guid RevisionSessionId { get; set; }
            public Guid CardId { get; set; }
            public SessionCardAnswerType AnswerType { get; set; }
            public string Answer { get; set; }
        }

        public class CompleteRevisionSessionCommand
        {
            public Guid RevisionSessionId { get; set; }
        }
    }
}