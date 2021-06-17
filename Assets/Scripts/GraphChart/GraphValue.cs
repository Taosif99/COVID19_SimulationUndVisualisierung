namespace GraphChart
{
    /// <summary>
    /// Class which reprsents a graph value which is
    /// a dot in a single line graph or a bar in barchart.
    /// </summary>
    public class GraphValue
    {
        private int _value;
        private bool _isEnabled;
        public int Value { get => _value; set => _value = value; }
        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }

        /// <summary>
        /// Creates a GraphValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isEnabled">Is the bar/dot enabled, i.o.w. is the bar/dot plotted</param>
        public GraphValue(int value, bool isEnabled)
        {
            this._value = value;
            this._isEnabled = isEnabled;
        }


    }
}