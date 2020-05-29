using System;
using Memoyed.Domain.Cards.RevisionSessions;

namespace Memoyed.Application.Dto
{
    public static class Commands
    {
        public interface CreateCardBoxSetCommand
        {
            string Name { get; }
            string TargetLanguage { get; }
            string NativeLanguage { get; }
        }

        public interface CreateCardBoxCommand
        {
            Guid CardBoxSetId { get; }
            int Level { get; }
            int RevisionDelay { get; }
        }

        public interface CreateCardCommand
        {
            Guid CardBoxSetId { get; }
            string NativeLanguageWord { get; }
            string TargetLanguageWord { get; }
            string? Comment { get; }
        }

        public interface RemoveCardCommand
        {
            Guid CardBoxSetId { get; }
            Guid CardId { get; }
        }

        public interface RenameCardBoxSetCommand
        {
            Guid CardBoxSetId { get; }
            string CardBoxSetName { get; }
        }

        public interface StartRevisionSessionCommand
        {
            Guid CardBoxSetId { get; }
        }
        
        public interface SetCardAnswerCommand
        {
            Guid RevisionSessionId { get; }
            Guid CardId { get; }
            SessionCardAnswerType AnswerType { get; }
            string Answer { get; }
        }

        public interface CompleteRevisionSessionCommand
        {
            Guid RevisionSessionId { get; }
        }
    }
}