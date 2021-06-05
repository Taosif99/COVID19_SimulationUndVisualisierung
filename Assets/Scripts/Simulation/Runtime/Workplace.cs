using System;

namespace Simulation.Runtime
{
    class Workplace : Venue
    {
        public Workplace(Edit.Workplace editorEntity) : base(editorEntity)
        {
            throw new NotImplementedException();
        }

        public WorkplaceType Type { get; }
        public int WorkerCapacity { get; }
    }
}
