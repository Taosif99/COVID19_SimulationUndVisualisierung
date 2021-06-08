namespace Simulation.Runtime
{
    class WorkShift
    {
        public WorkShift(Workplace workplace, WeekDays weekDays, int startTime, int duration)
        {
            Workplace = workplace;
            Days = weekDays;
            StartTime = startTime;
            Duration = duration;
        }

        public Workplace Workplace { get; }

        public WeekDays Days { get; }
        public int StartTime { get; }
        public int Duration { get; }
    }
}
