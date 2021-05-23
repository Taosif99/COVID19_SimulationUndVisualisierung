using System;

namespace KLASSEN_INF21
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
