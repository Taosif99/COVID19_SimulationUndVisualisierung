using System;

namespace Data
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
