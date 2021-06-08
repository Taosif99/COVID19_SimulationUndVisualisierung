using System;

namespace Simulation.Edit
{
    [Serializable]
    public class SimulationOptions
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
