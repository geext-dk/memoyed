using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Memoyed.DomainFramework
{
    public abstract class DomainValue : IEquatable<DomainValue>
    {
        public bool Equals(DomainValue? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            
            // take all the fields and members and compare them

            foreach (var member in Members)
            {
                var (lhs, rhs) = member switch
                {
                    FieldInfo fi => (fi.GetValue(this), fi.GetValue(other)),
                    PropertyInfo pi => (pi.GetValue(this), pi.GetValue(other)),
                    _ => throw new InvalidOperationException("This should never happen")
                };

                if (lhs == null && rhs != null ||
                    lhs != null && !lhs.Equals(rhs))
                {
                    return false;
                }
            }

            return true;
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
                   && Equals((DomainValue) obj);
        }

        public override int GetHashCode()
        {
            var sum = 0;
            var members = Members
                .OrderBy(m => m.Name)
                .ToArray();
            
            for (var i = 0; i < members.Length; ++i)
            {
                var value = members[i] switch
                {
                    PropertyInfo pi => pi.GetValue(this),
                    FieldInfo fi => fi.GetValue(this),
                    _ => throw new InvalidOperationException("This should never happen")
                };
                sum += value.GetHashCode() * (i + 1) * 7;
            }

            return sum;
        }

        public static bool operator ==(DomainValue? lhs, DomainValue? rhs)
        {
            return lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);
        }

        public static bool operator !=(DomainValue? lhs, DomainValue? rhs)
        {
            return !(lhs == rhs);
        }

        private const BindingFlags MembersBindingFlags = BindingFlags.Instance | BindingFlags.Public;
        private IEnumerable<MemberInfo> Members => GetType().GetFields(MembersBindingFlags)
            .Cast<MemberInfo>()
            .Union(GetType().GetProperties(MembersBindingFlags));
    }
}