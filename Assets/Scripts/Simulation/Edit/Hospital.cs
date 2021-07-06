using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Hospital : Workplace
    {



        /*
        public Hospital(GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity, HospitalScale scale, WorkerAvailability workerAvailability) : base(position, infectionRisk, type, workerCapacity)
        {
            Scale = scale;
            WorkerAvailability = workerAvailability;
        }*/

        public Hospital(int amountBeds,int amountIntensiveCareBeds,GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity) : base(position, infectionRisk, type, workerCapacity, false)
        {
            AmountRegularBeds = amountBeds;
            AmountIntensiveCareBeds = amountIntensiveCareBeds; 
        }


        public int AmountRegularBeds { get; set; }
        public int AmountIntensiveCareBeds { get; set; }

    }
}
