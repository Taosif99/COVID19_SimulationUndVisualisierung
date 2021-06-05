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
        public float RiskTendency { get; set; }
        public int NumberOfShoppingRuns { get; set; }
        public int NumberOfShoppers { get; set; }

        public Household(GridCell position, float infectionRisk, byte numberOfPeople, float percentageOfWorkers, float carefulnessTendency, float riskTendency, int numberOfShoppingRuns, int numberOfShoppers) : base(position, infectionRisk)
        {
            NumberOfPeople = numberOfPeople;
            PercentageOfWorkers = percentageOfWorkers;
            CarefulnessTendency = carefulnessTendency;
            RiskTendency = riskTendency;
            NumberOfShoppingRuns = numberOfShoppingRuns;
            NumberOfShoppers = numberOfShoppers;
        }


    }
}
