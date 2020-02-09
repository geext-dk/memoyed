using System;
using Memoyed.Cards.Domain.CardBoxSets;

namespace Memoyed.Cards.ApplicationServices.Dto
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

        public class LearningCardModel
        {
            public Guid Id { get; set; }
            public Guid SetId { get; set; }
            public Guid BoxId { get; set; }
            public string TargetLanguageWord { get; set; }
            public string NativeLanguageWord { get; set; }
            public string Comment { get; set; }
        }
    }
}