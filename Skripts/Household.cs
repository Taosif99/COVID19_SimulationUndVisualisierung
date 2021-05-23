using System;

namespace KLASSEN_INF21
{
    /// <summary>
    /// Represents a household.
    /// </summary>
    [Serializable]
    class Household : Venue
    {
        private byte _numberOfPeople;
        private float _percentageOfWorkers;
        private float _carefulnessTendency;
        private float _riskTendency;
        private int _numberOfShoppingRuns;
        private int _numberOfShoppers;

        public Household(Vector2 position, float infectionRisk, byte numberOfPeople, float percentageOfWorkers, float carefulnessTendency, float riskTendency, int numberOfShoppingRuns, int numberOfShoppers) : base(position, infectionRisk)
        {
            _numberOfPeople = numberOfPeople;
            _percentageOfWorkers = percentageOfWorkers;
            _carefulnessTendency = carefulnessTendency;
            _riskTendency = riskTendency;
            _numberOfShoppingRuns = numberOfShoppingRuns;
            _numberOfShoppers = numberOfShoppers;
        }
    }
}
