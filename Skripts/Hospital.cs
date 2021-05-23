using System;

namespace KLASSEN_INF21
{
    [Serializable]
    class Hospital : Workplace
    {
        private HospitalScale _scale;
        private WorkerAvailability _workerAvailability;

        public Hospital(Vector2 position, float infectionRisk, WorkplaceType type, int workerCapacity, HospitalScale scale, WorkerAvailability workerAvailability) : base(position, infectionRisk, type, workerCapacity)
        {
            _scale = scale;
            _workerAvailability = workerAvailability;
        }
    }
}
