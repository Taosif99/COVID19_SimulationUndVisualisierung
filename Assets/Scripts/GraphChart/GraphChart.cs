using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GraphChart
{
    /// <summary>
    /// Implementation of a linegraph and barchart.
    /// Used and modified the tutorial of CodeMonkey: https://www.youtube.com/watch?v=CmU5-v-v1Qo
    /// </summary>
    public class GraphChart : MonoBehaviour
    {

        [SerializeField] private Sprite _dotSprite;
        private RectTransform _graphContainer;

        //Labels
        private RectTransform _labelTemplateX;
        private RectTransform _labelTemplateY;

        //Dashes
        private RectTransform _dashContainer;
        private RectTransform _dashTemplateX;
        private RectTransform _dashTemplateY;

        //In this List we manage all GameObjects of the Graph
        private List<GameObject> _gameObjectList;

        // Values of the this graph
        private List<int> _valueList;

        //Needed for for dynamic update
        private List<GameObject> _dotsOrBarsList; //Here we handle our dots and bars
        private List<RectTransform> _yLabelList;
        private List<GameObject> _dotsConnectionList;


        //To define own Functions (delegates) for x and y label
        private Func<int, string> _getAxisLabelX;
        private Func<float, string> _getAxisLabelY;


        private float _graphWidth;
        private float _graphHeight;

        //Sets up the y-axis of the graph always starts at zero
        [SerializeField] private bool _startYScaleAtZero = true;
        [SerializeField] private bool _useFixedMaxYValue = true;
        [SerializeField] private float _maxYValue = 100f;
        [SerializeField] private int _amountHorizontalLines = 11;
        [SerializeField] private GraphType _typeOfGraph = GraphType.BarChart;

        //Properties if Graph is modified by other script
        public bool StartYScaleAtZero
        {
            get { return _startYScaleAtZero; }
            set { _startYScaleAtZero = value; }
        }

        public bool UseFixedMaxYValue
        {
            get { return _useFixedMaxYValue; }
            set { _useFixedMaxYValue = value; }
        }

        public float MaxYValue
        {
            get { return _maxYValue; }
            set { _maxYValue = value; }
        }

        public int AmountHorizontalLines
        {
            get { return _amountHorizontalLines; }
            set { _amountHorizontalLines = value; }
        }

        public GraphType TypeOfGraph
        {
            get { return _typeOfGraph; }
            set { _typeOfGraph = value; }
        }

        //Enum for GraphType
        public enum GraphType
        {
            BarChart,
            LineGraph
        }
        private void Awake()
        {

            //Get the templates and container
            _graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            _labelTemplateX = _graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
            _labelTemplateY = _graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
            _dashContainer = _graphContainer.Find("dashContainer").GetComponent<RectTransform>();
            _dashTemplateX = _dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
            _dashTemplateY = _dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();

            _graphWidth = _graphContainer.sizeDelta.x;
            _graphHeight = _graphContainer.sizeDelta.y;

            //Initialize lists
            _gameObjectList = new List<GameObject>();
            _dotsOrBarsList = new List<GameObject>();
            _yLabelList = new List<RectTransform>();
            _dotsConnectionList = new List<GameObject>();
        }

        /// <summary>
        /// Method which shows a Graph.
        /// </summary>
        /// <param name="valueList">Values which will be plotted.</param>
        /// <param name="getAxisLabelX">Delegate for the x-axis.</param>
        /// <param name="getAxisLabelY">Delegate for the y-axis.</param>
        public void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
        {
            this._valueList = valueList;
            InitializeLabels(getAxisLabelX, getAxisLabelY);
            //Destroying objects of previous graph
            foreach (GameObject gameObject in _gameObjectList)
            {
                Destroy(gameObject);
            }
            _gameObjectList.Clear();
            _yLabelList.Clear();
            _dotsConnectionList.Clear();
            float yMax, yMin;
            CalculateYScale(out yMin, out yMax);
            float xSize = _graphWidth / (_valueList.Count + 1);
            //Here we can do a if else statement to check for the graph type
            //I think using an enum makes the code less complicated
            GameObject lastDotGameObject = null;
            for (int i = 0; i < valueList.Count; i++)
            {
                float xPosition = xSize + i * xSize;
                float yPosition = ((valueList[i] - yMin) / (yMax - yMin)) * _graphHeight;
                if (_typeOfGraph == GraphType.BarChart)
                {
                    GameObject barGameObject = CreateBar(new Vector2(xPosition, yPosition), xSize * .9f);
                    _gameObjectList.Add(barGameObject);
                    _dotsOrBarsList.Add(barGameObject);
                }
                else if (_typeOfGraph == GraphType.LineGraph)
                {
                    BuildDotLine(xPosition, yPosition, ref lastDotGameObject, new Color(1, 1, 1, 0.5f));
                }
                BuildXLabelsAndDashes(xPosition, i);
            }
            BuildYLabelsAndDashes(yMin, yMax);
        }

        /// <summary>
        /// Method to update a value at a given index of the valueList.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void UpdateValue(int index, int value)
        {
            float yMinBefore, yMaxBefore;
            CalculateYScale(out yMinBefore, out yMaxBefore);
            _valueList[index] = value;
            float yMin, yMax;
            CalculateYScale(out yMin, out yMax);
            bool yScaleChanged = yMinBefore != yMin || yMaxBefore != yMax;
            float xSize = _graphWidth / (_valueList.Count + 1);

            if (!yScaleChanged)
            {
                // Y Scale did not change, update only this value
                float xPosition = xSize + index * xSize;
                float yPosition = ((value - yMin) / (yMax - yMin)) * _graphHeight;
                HandleGraphType(index, xPosition, yPosition, xSize);
            }

            else
            {
                // Y scale changed, update whole graph and y axis labels
                // Cycle through all visible data points
                for (int i = 0; i < _valueList.Count; i++)
                {
                    float xPosition = xSize + i * xSize;
                    float yPosition = ((_valueList[i] - yMin) / (yMax - yMin)) * _graphHeight;
                    HandleGraphType(i, xPosition, yPosition, xSize);
                }
                for (int i = 0; i < _yLabelList.Count; i++)
                {
                    float normalizedValue = i * 1f / (_yLabelList.Count - 1); //It must be the seperator count value!!!
                    _yLabelList[i].GetComponent<Text>().text = _getAxisLabelY(yMin + (normalizedValue * (yMax - yMin)));
                }
            }
        }


        /// <summary>
        /// Method to show a multiline graph.
        /// </summary>
        /// <param name="valueLists">A list of integer list which represent the linegraphs to display.</param>
        /// <param name="colors">An array of colors which will be applied "clockwise" on the linegraphs. 
        /// White is the default color if colors is null or if it does not contain any color.</param>
        /// <param name="getAxisLabelX">Delegate for the x-axis.</param>
        /// <param name="getAxisLabelY">Delegate for the y-axis.</param>
        public void ShowMultiLineGraph(List<List<int>> valueLists, List<Color> colors = null, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
        {
            _typeOfGraph = GraphType.LineGraph;
            InitializeLabels(getAxisLabelX, getAxisLabelY);
            //Destroying objects of previous graph
            foreach (GameObject gameObject in _gameObjectList)
            {
                Destroy(gameObject);
            }
            _gameObjectList.Clear();
            _yLabelList.Clear();
            _dotsConnectionList.Clear();
            float yMax, yMin;
            CalculateYScaleMultiline(out yMin, out yMax, valueLists);
            //Value list with the most values must be used
            int maxCount = 0;
            foreach (List<int> list in valueLists)
            {
                if (list.Count > maxCount)
                {
                    maxCount = list.Count;
                }
            }
            float xSize = _graphWidth / (maxCount + 1);
            int listCounter = 0;
            //This must be done for each value List
            foreach (List<int> valueList in valueLists)
            {
                GameObject lastDotGameObject = null;
                Color color;
                if (colors != null && colors.Count > 0)
                {
                    //Repeat colors modulu clockwise if not enough given
                    color = colors[listCounter % valueLists.Count];
                }
                else
                {
                    //White as default color
                    color = new Color(1, 1, 1, 0.5f);
                }

                for (int i = 0; i < valueList.Count; i++)
                {
                    float xPosition = xSize + i * xSize;
                    float yPosition = ((valueList[i] - yMin) / (yMax - yMin)) * _graphHeight;
                    BuildDotLine(xPosition, yPosition, ref lastDotGameObject, color);
                }

                listCounter++;

            }

            for (int i = 0; i < maxCount; i++)
            {
                float xPosition = xSize + i * xSize;
                BuildXLabelsAndDashes(xPosition, i);
            }

            BuildYLabelsAndDashes(yMin, yMax);
        }


        private void BuildDotLine(float xPosition, float yPosition, ref GameObject lastDotGameObject, Color color)
        {
            GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));
            _gameObjectList.Add(dotGameObject);
            _dotsOrBarsList.Add(dotGameObject);

            if (lastDotGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition, color);
                _gameObjectList.Add(dotConnectionGameObject);
                _dotsConnectionList.Add(dotConnectionGameObject);
            }
            //That is why we need ref in this method
            lastDotGameObject = dotGameObject;
        }

        private void BuildYLabelsAndDashes(float yMin, float yMax)
        {
            int separatorCount = _amountHorizontalLines - 1;
            for (int i = 0; i <= separatorCount; i++)
            {
                RectTransform labelY = Instantiate(_labelTemplateY);
                labelY.SetParent(_graphContainer, false);
                labelY.gameObject.SetActive(true);
                float normalizedValue = i * 1f / separatorCount;
                labelY.anchoredPosition = new Vector2(-7f, normalizedValue * _graphHeight);
                labelY.GetComponent<Text>().text = _getAxisLabelY(yMin + (normalizedValue * (yMax - yMin)));
                _yLabelList.Add(labelY);
                _gameObjectList.Add(labelY.gameObject);

                RectTransform dashY = Instantiate(_dashTemplateY);
                dashY.SetParent(_graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(-4f, normalizedValue * _graphHeight);
                _gameObjectList.Add(dashY.gameObject);
            }
        }

        private void BuildXLabelsAndDashes(float xPosition, int index)
        {
            RectTransform labelX = Instantiate(_labelTemplateX);
            labelX.SetParent(_graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = _getAxisLabelX(index);
            _gameObjectList.Add(labelX.gameObject);

            RectTransform dashX = Instantiate(_dashTemplateX);
            dashX.SetParent(_graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
            _gameObjectList.Add(dashX.gameObject);
        }

        private void InitializeLabels(Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
        {
            //Initialize labelfunctions
            if (getAxisLabelX == null)
            {
                _getAxisLabelX = delegate (int _i) { return _i.ToString(); };
            }
            else _getAxisLabelX = getAxisLabelX;
            if (getAxisLabelY == null)
            {
                _getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
            }
            else _getAxisLabelY = getAxisLabelY;
        }

        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(_graphContainer, false);
            gameObject.GetComponent<Image>().sprite = _dotSprite;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(10, 10); //Width and height of rectangle which holds our dot
            //Lower left  corner
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 firstDotPosition, Vector2 secondDotPosition, Color color)
        {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(_graphContainer, false);
            gameObject.GetComponent<Image>().color = color;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 direction = (secondDotPosition - firstDotPosition).normalized;
            float distance = Vector2.Distance(firstDotPosition, secondDotPosition);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = firstDotPosition + direction * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, GraphHelperMethods.GetAngleFromVector(direction));
            return gameObject;
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth)
        {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(_graphContainer, false);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            return gameObject;
        }
        private void HandleGraphType(int index, float xPosition, float yPosition, float xSize)
        {

            RectTransform rectTransform = _dotsOrBarsList[index].GetComponent<RectTransform>();
            if (_typeOfGraph == GraphType.LineGraph)
            {
                rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
                //Predecessor dot connection
                if (index > 0)
                    UpdateDotConnection(_dotsConnectionList[index - 1], _dotsOrBarsList[index - 1], _dotsOrBarsList[index]);
                //successor dot connection
                if (index != _valueList.Count - 1)
                    UpdateDotConnection(_dotsConnectionList[index], _dotsOrBarsList[index + 1], _dotsOrBarsList[index]);

            }
            else if (_typeOfGraph == GraphType.BarChart)
            {

                float barWidthMultiplier = .8f;
                rectTransform.anchoredPosition = new Vector2(xPosition, 0f);
                rectTransform.sizeDelta = new Vector2(xSize * barWidthMultiplier, yPosition);
            }
        }

        //TODO REPLACE LATER LINEAR SEARCH WITH BETTER ALGORITHM
        private void CalculateYScale(out float yMin, out float yMax)
        {
            CalculateYScaleMultiline(out yMin, out yMax, null, _valueList);
        }


        private void UpdateDotConnection(GameObject dotConnection, GameObject lastDot, GameObject currentDot)
        {

            RectTransform dotConnectionRectTransform = dotConnection.GetComponent<RectTransform>();
            Vector2 posOfLastDot = lastDot.GetComponent<RectTransform>().anchoredPosition;
            Vector2 posOfCurrentDot = currentDot.GetComponent<RectTransform>().anchoredPosition;
            Vector2 direction = (posOfLastDot - posOfCurrentDot).normalized;
            float distance = Vector2.Distance(posOfCurrentDot, posOfLastDot);
            dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
            dotConnectionRectTransform.anchoredPosition = posOfCurrentDot + direction * distance * .5f;
            dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, GraphHelperMethods.GetAngleFromVector(direction));

        }

        private void CalculateYScaleMultiline(out float yMin, out float yMax, List<List<int>> valueLists = null, List<int> singleLinevalues = null)
        {

            if (singleLinevalues != null)
            {
                valueLists = new List<List<int>>();
                valueLists.Add(singleLinevalues);
            }

            GraphHelperMethods.MultiIntegerListSearch(valueLists, out yMax, out yMin);

            float yDifference = yMax - yMin;
            //Default Case if all values are the same or something went wrong
            if (yDifference <= 0)
            {
                yDifference = 5f;
            }
            //Scalling the min and maximum value a little bit up / lengthen y axis
            yMax = yMax + (yDifference * 0.2f);
            yMin = yMin - (yDifference * 0.2f);
            // Start the graph always at zero on the y-axis
            if (_startYScaleAtZero)
            {
                yMin = 0f;
            }

            if (_useFixedMaxYValue)
            {
                yMax = _maxYValue;
            }
        }
    }
}