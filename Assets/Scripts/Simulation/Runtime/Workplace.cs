using System;
using System.Collections.Generic;

namespace Simulation.Runtime
{
    class Workplace : Venue
    {
        public Workplace(Edit.Workplace editorEntity) : base(editorEntity)
        {
            Type = editorEntity.Type;
            WorkerCapacity = editorEntity.WorkerCapacity;
        }

        public WorkplaceType Type { get; }
        public int WorkerCapacity { get; }

        public List<WorkShift> GetWorkShifts()
        {
            throw new NotImplementedException();
        }
    }
}
