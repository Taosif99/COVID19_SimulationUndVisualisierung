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
             switch (Type)
            {
                case Type.Other:

                    return new List<WorkShift>
                    {
      
                        new WorkShift(this, WeekDays.WorkDays, 8, 8)
                    };


                case Type.Store:
                    return new List<WorkShift>
                    {
                        new WorkShift(this, WeekDays.WorkDays| WeekDays.Saturday, 8, 8),
                        new WorkShift(this, WeekDays.WorkDays| WeekDays.Saturday, 12, 8)

                    };


                 case Type.Hospital:
                    return new List<WorkShift>
                    {

                        new WorkShift(this, WeekDays.AllDays, 0, 8),
                        new Workshift(this, WeekDays.AllDays, 8, 8),
                        new Workshift(this, WeekDays.AllDays, 16, 8)
                        

                    };
            }
        }
    }
}
