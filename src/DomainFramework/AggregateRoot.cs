using System;

namespace Memoyed.DomainFramework
{
    public abstract class AggregateRoot : Entity
    {
        private IDomainEventPublisher _domainEventPublisher;

        protected IDomainEventPublisher? EventPublisher
        {
            get => _domainEventPublisher ?? throw new InvalidOperationException("The event publisher hasn't been set");
            private set => _domainEventPublisher = value;
        }

        public void SetEventPublisher(IDomainEventPublisher publisher)
        {
            if (EventPublisher != null)
            {
                throw new InvalidOperationException("The event publisher has already been set");
            }

            EventPublisher = publisher;
        }
    }
}