using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Policies
    {
        public MaskType RequiredMaskType { get; set; }

        public Policies(MaskType requiredMaskType)
        {
            RequiredMaskType = requiredMaskType;
        }
    }
}
