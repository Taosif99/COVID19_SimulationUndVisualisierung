using System;

namespace Data
{
    [Serializable]
    class Venue : Entity
    {
        private float _infectionRisk;

        public Venue(Vector2 position, float infectionRisk) : base(position)
        {
            _infectionRisk = infectionRisk;
        }
    }
}
