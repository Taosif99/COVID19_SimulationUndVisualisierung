namespace Simulation.Runtime
{
    class Household : Venue
    {
        private Person[] _members;

        public Household(Person[] members, float infectionRisk) : base(infectionRisk)
        {
            _members = members;
        }
    }
}
