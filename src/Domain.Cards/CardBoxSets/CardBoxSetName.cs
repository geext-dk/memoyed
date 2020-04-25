using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetName : DomainValue<string>
    {
        public CardBoxSetName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new InvalidOperationException("Name shouldn't be empty");

            Value = value;
        }
    }
}