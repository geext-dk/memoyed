using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Shared
{
    public class UtcTime : DomainValue<DateTime>
    {
        public UtcTime(DateTime time)
        {
            if (time.Kind == DateTimeKind.Local)
            {
                throw new DomainException.LocalDateTimeException();
            }

            if (time.Kind == DateTimeKind.Unspecified)
            {
                time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            }

            Value = time;
        }

        private UtcTime()
        {
        }
    }
}