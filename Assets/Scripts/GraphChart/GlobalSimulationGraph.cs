using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace GraphChart
{
    /// <summary>
    /// Class which handles the showing of graphs and updating of the corresponding values
    /// </summary>
    public class GlobalSimulationGraph : MonoBehaviour
    {

        [SerializeField] private GameObject _fullScreenGraphGameObject;
        
        [SerializeField] private GraphChart _multiLineGraph;
        [SerializeField] private GraphChart _barchart;
        

        [SerializeField] private GameObject _multiLineGraphGameObject;
        [SerializeField] private GameObject _barchartGameObject;


        //Multiline
        private List<List<int>> _lines;
        private List<Color> _colorList;
        private Func<int, string> _xLabelMultilineGraph;
        //Barchart
        private List<int> _barchartValues;
        private Func<int, string> _xLabelBarChart;
        //To use the more performant update value of a barchart
        private bool _barChartCreated = false;

        public static GlobalSimulationGraph Instance;

        /*
        public GraphChart MultiLineGraph { get => _multiLineGraph; set => _multiLineGraph = value; }
        public GraphChart Barchart { get => _barchart; set => _barchart = value; }
        */

        public bool BarChartCreated { get => _barChartCreated; set => _barChartCreated = value; }
        public GameObject MultiLineGraphGameObject { get => _multiLineGraphGameObject; set => _multiLineGraphGameObject = value; }
        public GameObject BarchartGameObject { get => _barchartGameObject; set => _barchartGameObject = value; }

        private void Awake()
        {
            //if (Instance == null) Instance = this;
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(Instance);
            }
            else if (Instance != null)
            {
                Destroy(gameObject);
            }


            _barchartValues = new List<int>() { 0, 0, 0 };

        }

        // Start is called before the first frame update
        void Start()
        {


            InitMultiLineGraph();
            InitBarChart();
            StartCoroutine(UpdateGraphsEachDay());
        }

        private void InitMultiLineGraph()
        {


        _xLabelMultilineGraph = delegate (int index)
            {
                return (index + 1).ToString();

            };


            List<int> phase1Values = new List<int>();
            List<int> phase2Values = new List<int>();
            List<int> phase3Values = new List<int>();
            List<int> phase4Values = new List<int>();
            List<int> phase5Values = new List<int>();
            List<int> uninfectedValues = new List<int>();

            _lines = new List<List<int>>();
            _lines.Add(phase1Values);
            //lines.Add(phase2Values);
            //lines.Add(phase3Values);
            //lines.Add(phase4Values);
            _lines.Add(phase5Values);
            _lines.Add(uninfectedValues);


            //For simplification only showing infected and uninfected
            //TODO UNIFORM COLORS and legend in UI
            Color infectedColor = Color.yellow;
            Color recoveredColor = Color.green;
            Color uninfectedColor = Color.white;
            _colorList = new List<Color>();
            _colorList.Add(infectedColor);
            _colorList.Add(recoveredColor);
            _colorList.Add(uninfectedColor);
        }


        private void InitBarChart()
        {
            //Init Bar chart
            //Example how to declare a label delegate function...
            _xLabelBarChart = delegate (int index)
            {
                switch (index)
                {
                    case 0:
                        return "Infected";
                    case 1:
                        return "Recovered";
                    case 2:
                        return "Uninfected";
                    default:
                        return "undefined";
                }

            };

            //Barchart.TypeOfGraph = GraphChart.GraphType.BarChart;
            BarchartGameObject.GetComponent<GraphChart>().TypeOfGraph = GraphChart.GraphType.BarChart;
        }


        //E.g called each day or each state transition
        private void UpdateLineGraphValues() 
        {
         
                _lines[0].Add(SimulationMaster.Instance.AmountInfected);
                _lines[1].Add(SimulationMaster.Instance.AmountRecovered);
                _lines[2].Add(SimulationMaster.Instance.AmountUninfected);
            
            //Clear lists each 7 days
            if (_lines[0].Count == 8)
            {
                _lines[0].Clear();
                _lines[1].Clear();
                _lines[2].Clear(); ;

                //Add the cleared day
                _lines[0].Add(SimulationMaster.Instance.AmountInfected);
                _lines[1].Add(SimulationMaster.Instance.AmountRecovered);
                _lines[2].Add(SimulationMaster.Instance.AmountUninfected);
            }

        }


        private void UpdateBarChartValues()
        {

            /*
             * FIXME UPDATE CAUSES NULL POINTER EXCEPTION
            Barchart.UpdateValue(0, SimulationMaster.Instance.AmountInfected);
            Barchart.UpdateValue(1, SimulationMaster.Instance.AmountRecovered);
            Barchart.UpdateValue(2, SimulationMaster.Instance.AmountUninfected);
          */
            _barchartValues[0] = SimulationMaster.Instance.AmountInfected;
            _barchartValues[1] = SimulationMaster.Instance.AmountRecovered;
            _barchartValues[2] = SimulationMaster.Instance.AmountUninfected;
        }
        /// <summary>
        /// Method to show the active graphs and eventually to update the values.
        /// </summary>
        /// <param name="shouldUpdateValues">If the infection values should be updated, e.g. after a day, a week...</param>
        public void UpdateValuesAndShowGraphs(bool shouldUpdateValues)
        {



            // UpdateGraph();
            /*
            if (!BarChartCreated && Barchart.isActiveAndEnabled)
            {
                Barchart.ShowGraph(_barchartValues, _xLabelBarChart);
                BarChartCreated = true;

            }

            if (BarChartCreated && Barchart.isActiveAndEnabled)
            {

                UpdateBarChart();

            }*/
            //if (Barchart.isActiveAndEnabled)
            if (BarchartGameObject.activeInHierarchy)
            {

                if (shouldUpdateValues)
                    UpdateBarChartValues();

                //Barchart.ShowGraph(_barchartValues, _xLabelBarChart);
                BarchartGameObject.GetComponent<GraphChart>().ShowGraph(_barchartValues, _xLabelBarChart);

            }
   

            if(shouldUpdateValues)
                UpdateLineGraphValues();
            //if (MultiLineGraph.isActiveAndEnabled)
            if (MultiLineGraphGameObject.activeInHierarchy)
            {

                // MultiLineGraph.ShowMultiLineGraph(_lines, _colorList, _xLabelMultilineGraph);
                MultiLineGraphGameObject.GetComponent<GraphChart>().ShowMultiLineGraph(_lines, _colorList, _xLabelMultilineGraph);
            }
           

        }

        //TODO USING ACTION FROM SIMULATION CONTROLLER ???
        private IEnumerator UpdateGraphsEachDay()
        {
            for (; ; )
            {
                //A day takes approx. 8 seconds
                //TODO CALCULATE DAY LENGTH VIA CODE         
                yield return new WaitForSeconds(8f);
                UpdateValuesAndShowGraphs(true);
            }
        }
    }
}