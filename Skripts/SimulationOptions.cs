using System;

namespace KLASSEN_INF21
{
    [Serializable]
    class SimulationOptions
    {
        private Policies _policies;
        private Event [] _event;

        public SimulationOptions(Policies policies, Event[] @event)
        {
            _policies = policies;
            _event = @event;
        }
    }
}
