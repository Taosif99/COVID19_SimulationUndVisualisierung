using System;

namespace Simulation.Edit
{
    [Serializable]
   public class Workplace : Venue
    {
        private WorkplaceType _type;
        private int _workerCapacity;

        public WorkplaceType Type   
        {
            get { return _type; }
            set { _type = value; }
        }

        public int WorkerCapacity  
        {
            get { return _workerCapacity; }
            set { _workerCapacity = value; }
        }


        public Workplace(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity) : base(position, infectionRisk)
        {
            _type = type;
            _workerCapacity = workerCapacity;
        }
    }
}