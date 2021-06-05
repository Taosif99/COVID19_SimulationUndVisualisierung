using System;

namespace Simulation.Runtime
{
    class Hospital : Workplace
    {
        public Hospital(Edit.Hospital editorEntity) : base(editorEntity)
        {
            throw new NotImplementedException();
        }

        public Scale HospitalScale { get; }
        public WorkerAvailability HospitalWorkerAvailability { get; }

        public enum Scale
        {
            Small,
            Medium,
            Large
        }

        public enum WorkerAvailability
        {
            Low,
            Medium,
            High
        }
    }
}
