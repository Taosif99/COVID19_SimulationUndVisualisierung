using System;

namespace Data
{
    [Serializable]
    class Policies
    {
        private MaskType _requiredMaskType;
        public Policies(MaskType requiredMaskType)
        {
            _requiredMaskType = requiredMaskType;
        }
    }
}
