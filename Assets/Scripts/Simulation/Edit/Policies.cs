using System;

namespace Simulation.Edit
{
    [Serializable]
    public class Policies
    {
        private MaskType _requiredMaskType;
        public Policies(MaskType requiredMaskType)
        {
            _requiredMaskType = requiredMaskType;
        }
    }
}
