using System;

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

            public int CardBoxLevel { get; set; }
            public int RevisionDelay { get; set; }
        }

        public class AddNewCardCommand
        {
            public Guid CardBoxSetId { get; set; }
            public string NativeLanguageWord { get; set; }
            public string TargetLanguageWord { get; set; }
            public string Comment { get; set; }
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
            public string Answer { get; set; }
        }

        public class CompleteSessionCommand
        {
            public Guid RevisionSessionId { get; set; }
        }
    }
}