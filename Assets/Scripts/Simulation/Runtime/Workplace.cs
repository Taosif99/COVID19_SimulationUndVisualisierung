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
            AmountAssignedWorkers = 0;
            WorkShifts = CreateWorkShifts();
        }

        public WorkplaceType Type { get; }
        public int WorkerCapacity { get; }
        public List<WorkShift> WorkShifts { get; }
        public int AmountAssignedWorkers { get; set; }

        private List<WorkShift> CreateWorkShifts()
        {
            switch (Type)
            {
                case WorkplaceType.Other:
                    return new List<WorkShift>
                    {
                        new WorkShift(this, WeekDays.WorkDays, 8, 8)
                    };


                case WorkplaceType.Store:
                    return new List<WorkShift>
                    {
                        new WorkShift(this, WeekDays.WorkDays | WeekDays.Saturday, 8, 8),
                        new WorkShift(this, WeekDays.WorkDays | WeekDays.Saturday, 12, 8)
                    };


                 case WorkplaceType.Hospital:
                    return new List<WorkShift>
                    {
                        new WorkShift(this, WeekDays.All, 0, 8),
                        new WorkShift(this, WeekDays.All, 8, 8),
                        new WorkShift(this, WeekDays.All, 16, 8)
                    };
            }

            throw new NotSupportedException($"Workplace Type {Type} is currently not supported");
        }
    }
}
