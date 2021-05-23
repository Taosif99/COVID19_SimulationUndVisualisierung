using System;

namespace KLASSEN_INF21
{
    [Serializable]
    class Workplace : Venue
    {
        private WorkplaceType _type;
        private int _workerCapacity;

        public Workplace(Vector2 position, float infectionRisk, WorkplaceType type, int workerCapacity) : base(position, infectionRisk)
        {
            _type = type;
            _workerCapacity = workerCapacity;
        }
    }
}