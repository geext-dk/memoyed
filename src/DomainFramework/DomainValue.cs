using System;
using System.Collections.Generic;

namespace Memoyed.DomainFramework
{
    public abstract class DomainValue<T> : IEquatable<DomainValue<T>>
    {
        protected DomainValue()
        {
            Value = default;
        }

        public T Value { get; protected set; }

        public bool Equals(DomainValue<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType()
                   && Equals((DomainValue<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static implicit operator T(DomainValue<T> domainValue)
        {
            return domainValue.Value;
        }

        public static bool operator ==(DomainValue<T> lhs, DomainValue<T> rhs)
        {
            return lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);
        }

        public static bool operator !=(DomainValue<T> lhs, DomainValue<T> rhs)
        {
            return !(lhs == rhs);
        }
    }
}