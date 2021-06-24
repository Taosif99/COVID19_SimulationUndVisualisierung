using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Simulation
    {
        private SimulationOptions _simulationOptions;
        private Entity [] _entities;

        public Entity[] Entities { get => _entities; set => _entities = value; }
        public SimulationOptions SimulationOptions { get => _simulationOptions; set => _simulationOptions = value; }

        public Simulation(SimulationOptions simulationOptions, Entity[] entity)
        {
            _simulationOptions = simulationOptions;
            Entities = entity;
        }

      
    }
}
