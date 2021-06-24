using System;

namespace Simulation.Edit
{
    [Serializable]
    public class SimulationOptions
    {
        private Policies _policies;
        private Event [] _event;
        private AdjustableSimulationSettings _adjustableSimulationPrameters;

        public SimulationOptions(Policies policies, Event[] @event, AdjustableSimulationSettings adjustableSimulationPrameters)
        {
            _policies = policies;
            _event = @event;
            _adjustableSimulationPrameters = adjustableSimulationPrameters;
        }

        public AdjustableSimulationSettings AdjustableSimulationPrameters { get => _adjustableSimulationPrameters; set => _adjustableSimulationPrameters = value; }
    }
}
