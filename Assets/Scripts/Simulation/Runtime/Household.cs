using System;

namespace Simulation.Runtime
{
    class Household : Venue
    {
        private Person[] _members;

        public Household(Edit.Household editorEntity) : base(editorEntity)
        {
            NumberOfPeople = editorEntity.NumberOfPeople;
            PercentageOfWorkers = editorEntity.PercentageOfWorkers;
            CarefulnessTendency = editorEntity.CarefulnessTendency;
            RiskTendency = editorEntity.RiskTendency;
            NumberOfShoppingRuns = editorEntity.NumberOfShoppingRuns;
            NumberOfShoppers = editorEntity.NumberOfShoppers;
        }

        public Person[] members { get => _members; set => _members = value; }
        public byte NumberOfPeople { get; set; }
        public float PercentageOfWorkers { get; set; }
        public float CarefulnessTendency { get; set; }
        public float RiskTendency { get; set; }
        public int NumberOfShoppingRuns { get; set; }
        public int NumberOfShoppers { get; set; }
    }
}
