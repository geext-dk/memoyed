using System;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CompletedRevisionSessionId
    {
        public CompletedRevisionSessionId(Guid value)
        {
            Value = value;
        }
        
        public Guid Value { get; private set; }
    }
}