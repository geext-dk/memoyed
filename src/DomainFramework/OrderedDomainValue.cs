using System;

namespace Memoyed.DomainFramework
{
    public abstract class OrderedDomainValue<T> : DomainValue<T>, IComparable<OrderedDomainValue<T>>
    {
        protected abstract int Position { get; }

        public int CompareTo(OrderedDomainValue<T> other)
        {
            return Position.CompareTo(other.Position);
        }

        public static bool operator <(OrderedDomainValue<T> lhs, OrderedDomainValue<T> rhs)
        {
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
            {
                return false;
            }

            return lhs.Position < rhs.Position;
        }

        public static bool operator >(OrderedDomainValue<T> lhs, OrderedDomainValue<T> rhs)
        {
            return rhs < lhs;
        }

        public static bool operator <=(OrderedDomainValue<T> lhs, OrderedDomainValue<T> rhs)
        {
            return lhs == rhs || lhs < rhs;
        }

        public static bool operator >=(OrderedDomainValue<T> lhs, OrderedDomainValue<T> rhs)
        {
            return rhs <= lhs;
        }
    }
}