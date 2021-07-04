namespace Simulation.Runtime
{
    public class Activity
    {
        public Activity(WeekDays weekDays, int startTime, int endTime, Venue location, bool isWork)
        {
            Days = weekDays;
            StartTime = startTime;
            EndTime = endTime;
            Location = location;
            IsWork = isWork;
        }

        public WeekDays Days { get; }
        public int StartTime { get; }
        public int EndTime { get; }
        public Venue Location { get; }

        public bool IsWork { get; set; }

    }
}
