using System;
using System.Collections.Generic;
using Memoyed.Cards.Domain.CardBoxes;

namespace Memoyed.Cards.Domain.CardBoxSets
{
    public interface ICardBoxSetSnapshot
    {
        public Guid Id { get; }

        public string NativeLanguage { get; }
        
        public string TargetLanguage { get; }
        
        public IEnumerable<ICardBoxSnapshot> CardBoxes { get; }
    }
}