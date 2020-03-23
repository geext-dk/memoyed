using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Shared
{
    public class UtcTime : DomainValue
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

            Time = time;
        }
        
        public DateTime Time { get; }

        // ReSharper disable once UnusedMember.Local
        private UtcTime()
        {
        }
    }
}