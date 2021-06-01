namespace Simulation.Runtime
{
    class Workshift
    {
        private WeekDays _weekDays;
        private int _startTime;
        private int _duration;

        public Workshift(WeekDays weekDays, int startTime, int duration)
        {
            _weekDays = weekDays;
            _startTime = startTime;
            _duration = duration;
        }
    }
}
