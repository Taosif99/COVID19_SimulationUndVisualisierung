namespace Assets.Scripts.Runtime
{
    class Activity
    {
        private WeekDays _weekDays;
        private int _startTime;
        private int _endTime;
        private Venue _location;

        public Activity(WeekDays weekDays, int startTime, int endTime, Venue location)
        {
            _weekDays = weekDays;
            _startTime = startTime;
            _endTime = endTime;
            _location = location;
        }
    }
}
