using System.Collections.Generic;

namespace GraphChart
{
    /// <summary>
    /// Class which represents a line in an GraphChart graph.
    /// </summary>
    public class Line
    {
        private List<int> _values;
        private bool _isEnabled;

        public List<int> Values { get => _values; set => _values = value; }
        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }

        /// <summary>
        /// Creates a Line object.
        /// </summary>
        /// <param name="values">A list of int values whoch represent the value set.</param>
        /// <param name="isEnabled">Is the line enabled, i.o.w. is the line plotted</param>
        public Line(List<int> values, bool isEnabled)
        {
            this._values = values;
            this._isEnabled = isEnabled;
        }

  
    }

}