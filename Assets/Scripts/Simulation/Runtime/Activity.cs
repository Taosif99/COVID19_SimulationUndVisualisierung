namespace Simulation.Runtime
{
    public class Activity
    {
        public Activity(WeekDays weekDays, int startTime, int endTime, Venue location)
        {
            Days = weekDays;
            StartTime = startTime;
            EndTime = endTime;
            Location = location;
        }

        public WeekDays Days { get; }
        public int StartTime { get; }
        public int EndTime { get; }
        public Venue Location { get; }
    }
}
