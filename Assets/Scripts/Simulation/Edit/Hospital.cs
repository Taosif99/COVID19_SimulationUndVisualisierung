using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Hospital : Workplace
    {

        public Hospital(int amountRegularBeds,int amountIntensiveCareBeds,GridCell position, float infectionRisk, WorkplaceType type, int workerCapacity) : base(position, infectionRisk, type, workerCapacity)
        {
            AmountRegularBeds = amountRegularBeds;
            AmountIntensiveCareBeds = amountIntensiveCareBeds; 
        }


        public int AmountRegularBeds { get; set; }
        public int AmountIntensiveCareBeds { get; set; }

    }
}
