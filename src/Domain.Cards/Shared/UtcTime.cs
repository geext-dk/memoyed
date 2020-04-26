using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Shared
{
    public class UtcTime : DomainValue<DateTimeOffset>
    {
        public UtcTime(DateTime time)
        {
            if (time.Kind == DateTimeKind.Local) throw new DomainException.LocalDateTimeException();

            if (time.Kind == DateTimeKind.Unspecified) time = DateTime.SpecifyKind(time, DateTimeKind.Utc);

            Value = new DateTimeOffset(time);
        }
        
        public UtcTime(DateTimeOffset time)
        {
            if (time.Offset != TimeSpan.Zero) throw new DomainException.LocalDateTimeException();

            Value = time;
        }

        private UtcTime()
        {
        }
    }
}