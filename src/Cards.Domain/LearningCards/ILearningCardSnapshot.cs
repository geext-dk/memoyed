using System;

namespace Memoyed.Cards.Domain.LearningCards
{
    public interface ILearningCardSnapshot
    {
        Guid Id { get; }
        
        Guid? CardBoxId { get; }
        
        string NativeLanguageWord { get; }
        
        string TargetLanguageWord { get; }
        
        string Comment { get; }
        
        DateTime? CardBoxChangedDate { get; }
    }
}