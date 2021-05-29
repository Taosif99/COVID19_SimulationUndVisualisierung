using System;

namespace Data
{
    [Serializable]
    class Simulation
    {
        private SimulationOptions _simulationOptions;
        private Entity [] _entity;

        public Simulation(SimulationOptions simulationOptions, Entity[] entity)
        {
            _simulationOptions = simulationOptions;
            _entity = entity;
        }
    }
}
