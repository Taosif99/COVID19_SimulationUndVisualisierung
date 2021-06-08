using System;

namespace Simulation.Runtime
{
    static class DayOfWeekExtensions
    {
        public static WeekDays AsDayOfWeek(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return WeekDays.Monday;
                case DayOfWeek.Tuesday:
                    return WeekDays.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekDays.Wednesday;
                case DayOfWeek.Thursday:
                    return WeekDays.Thursday;
                case DayOfWeek.Friday:
                    return WeekDays.Friday;
                case DayOfWeek.Saturday:
                    return WeekDays.Saturday;
                case DayOfWeek.Sunday:
                    return WeekDays.Sunday;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, "Invalid day of week given - this should never happen.");
            }
        }
    }
}
