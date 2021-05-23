using System;

namespace KLASSEN_INF21
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
