
namespace Assets.Scripts.Runtime
{
    class Workplace : Venue
    {
        private Type _type;
        private int _workerCapacity;

        public Workplace(Type type, int workerCapacity, float infectionRisk) : base(infectionRisk)
        {
            _type = type;
            _workerCapacity = workerCapacity;
        }

        public enum Type
        {
            Other,
            Store,
            Hospital
        }
    }
}
