using System;
using System.Collections.Generic;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public interface ICardBoxSnapshot
    {
        Guid Id { get; }
        Guid SetId { get; }
        int Level { get; }
        int RevisionDelay { get; }
        IEnumerable<ILearningCardSnapshot> LearningCards { get; }
    }
}