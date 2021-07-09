using System;

namespace Simulation.Edit
{
    /// <summary>
    /// Represents a household.
    /// </summary>
    [Serializable]
    public class Household : Venue 
    {
        public byte NumberOfPeople { get; set; }
        public float PercentageOfWorkers { get; set; }
        public float CarefulnessTendency { get; set; }
        public int NumberOfShoppingRuns { get; set; }

        public Household(GridCell position, float infectionRisk, byte numberOfPeople, float percentageOfWorkers, float carefulnessTendency, int numberOfShoppingRuns) : base(position, infectionRisk)
        {
            NumberOfPeople = numberOfPeople;
            PercentageOfWorkers = percentageOfWorkers;
            CarefulnessTendency = carefulnessTendency;
            NumberOfShoppingRuns = numberOfShoppingRuns;
        }
    }
}
