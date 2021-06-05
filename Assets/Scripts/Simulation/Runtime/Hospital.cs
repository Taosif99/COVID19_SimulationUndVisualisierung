using System;
using UnityEngine;

namespace Simulation.Runtime
{
    class Hospital : Workplace
    {
        public Hospital(Edit.Hospital editorEntity) : base(editorEntity)
        {
            HospitalScale = (Scale)editorEntity.Scale;
            HospitalWorkerAvailability = (WorkerAvailability)editorEntity.WorkerAvailability;
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
