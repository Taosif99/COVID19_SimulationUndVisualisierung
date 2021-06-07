using System;
using UnityEngine;
using Simulation;

namespace Simulation.Runtime
{
    class Hospital : Workplace
    {
        public Hospital(Edit.Hospital editorEntity) : base(editorEntity)
        {
            HospitalScale = (HospitalScale)editorEntity.Scale;
            HospitalWorkerAvailability = (WorkerAvailability)editorEntity.WorkerAvailability;
        }

        public HospitalScale HospitalScale { get; }
        public WorkerAvailability HospitalWorkerAvailability { get; }

    }
}
