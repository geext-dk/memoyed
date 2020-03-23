using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetName : DomainValue
    {
        public CardBoxSetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Name shouldn't be empty");
            }

            Name = name;
        }
        
        public string Name { get; }
    }
}