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

            WorkShifts = CreateWorkShifts();
        }

        public WorkplaceType Type { get; }
        public int WorkerCapacity { get; }
        public List<WorkShift> WorkShifts { get; }

        private List<WorkShift> CreateWorkShifts()
        {
            //throw new NotImplementedException();
            return new List<WorkShift>
            {
                new WorkShift(this, WeekDays.WorkDays, 9, 8)
            };
        }
    }
}
