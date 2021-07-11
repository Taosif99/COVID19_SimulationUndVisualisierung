using System;

namespace Simulation.Edit
{
    [Serializable]
    public class SimulationOptions
    {
        private Event [] _event;
        private AdjustableSimulationSettings _adjustableSimulationPrameters;

        public SimulationOptions(Policies policies, Event[] @event, AdjustableSimulationSettings adjustableSimulationPrameters)
        {
            Policies = policies;
            _event = @event;
            _adjustableSimulationPrameters = adjustableSimulationPrameters;
        }

        public Policies Policies { get; set; }

        public AdjustableSimulationSettings AdjustableSimulationPrameters { get => _adjustableSimulationPrameters; set => _adjustableSimulationPrameters = value; }
    }
}
