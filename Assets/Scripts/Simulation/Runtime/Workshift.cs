namespace Simulation.Runtime
{
    class WorkShift
    {
        private WeekDays _weekDays;
        private int _startTime;
        private int _duration;

        public WorkShift(WeekDays weekDays, int startTime, int duration)
        {
            _weekDays = weekDays;
            _startTime = startTime;
            _duration = duration;
        }
    }
}
