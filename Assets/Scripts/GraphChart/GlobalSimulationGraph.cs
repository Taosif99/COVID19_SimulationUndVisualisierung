using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace GraphChart
{
    /// <summary>
    /// Class which handles the showing of graphs and updating of the corresponding values
    /// in the simulation.
    /// </summary>
    public class GlobalSimulationGraph : MonoBehaviour
    {

        [SerializeField] private GameObject _multiLineGraphGameObject;
        [SerializeField] private GameObject _barchartGameObject;


        //Multiline
        private List<Line> _lines;

        private List<Color> _colorList;
        private Func<int, string> _xLabelMultilineGraph;
        //Barchart
        private List<GraphValue> _barchartValues;
        private Func<int, string> _xLabelBarChart;
        //To use the more performant update value of a barchart
        private bool _barChartCreated = false;


        //private delegate void UpdateBarchartValues();
        //private delegate void UpdateLinechartValues();

        public static GlobalSimulationGraph Instance;

        public bool BarChartCreated { get => _barChartCreated; set => _barChartCreated = value; }
        public GameObject MultiLineGraphGameObject { get => _multiLineGraphGameObject; set => _multiLineGraphGameObject = value; }
        public GameObject BarchartGameObject { get => _barchartGameObject; set => _barchartGameObject = value; }


        //TODO OUTSOURCE UI CONTROLLER
        public Toggle UninfectedToggle;
        public Toggle InfectedToggle;
        public Toggle InfectiousToggle;
        public Toggle RecoveredToggle;



        private void Awake()
        {

            if (Instance == null)
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {

            InitColorList();
            InitMultiLineGraph();
            InitBarChart();

            //TODO START COROURINE OR USE EVENTS ONLY IF SIMULATION STARTED
            StartCoroutine(UpdateGraphsEachDay());
        }

        /// <summary>
        /// Method to show the active graphs and eventually to update the values.
        /// </summary>
        /// <param name="shouldUpdateValues">If the infection values should be updated, e.g. after a day, a week...</param>
        public void UpdateValuesAndShowGraphs(bool shouldUpdateValues)
        {


            if (shouldUpdateValues)
            {
                UpdateBarChartValues();
                UpdateLineGraphValues();
            }


            if (BarchartGameObject.activeInHierarchy)
            {
                BarchartGameObject.GetComponent<GraphChart>().ShowGraph(_barchartValues, _colorList, _xLabelBarChart);
            }

            if (MultiLineGraphGameObject.activeInHierarchy)
            {
                MultiLineGraphGameObject.GetComponent<GraphChart>().ShowMultiLineGraph(_lines, _colorList, _xLabelMultilineGraph);
            }

        }


        private void InitColorList()
        {
      
            Color uninfectedColor = Color.white;
            Color infectedColor = Color.yellow;
            Color infectiousColor = Color.red;
            Color recoveredColor = Color.green;

            _colorList = new List<Color>();
            _colorList.Add(uninfectedColor);
            _colorList.Add(infectedColor);
            _colorList.Add(infectiousColor);
            _colorList.Add(recoveredColor);


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
            List<int> infectiousValues = new List<int>();

            /*
            _lines = new List<List<int>>();
            _lines.Add(uninfectedValues);
            _lines.Add(phase1Values);
            _lines.Add(infectious);
            //lines.Add(phase2Values);
            //lines.Add(phase3Values);
            //lines.Add(phase4Values);
            _lines.Add(phase5Values);
          
            */
            Line phase1Line = new Line(phase1Values, true);
            Line phase2Line = new Line(phase2Values, true);
            Line phase3Line = new Line(phase3Values, true);
            Line phase4Line = new Line(phase4Values, true);
            Line phase5Line = new Line(phase5Values, true);
            Line uninfectedLine = new Line(uninfectedValues, true);
            Line infectiousLine = new Line(infectiousValues,true);
            _lines = new List<Line>();
            _lines.Add(uninfectedLine);
            _lines.Add(phase1Line);
            _lines.Add(infectiousLine);
           // _lines.Add(phase2Line);
            //_lines.Add(phase3Line);
            //_lines.Add(phase4Line);
            _lines.Add(phase5Line);
           
        
        }


        private void InitBarChart()
        {
            //_barchartValues = new List<int>() { 0, 0, 0,0 };
            _barchartValues = new List<GraphValue>() 
            { 
             new GraphValue(0,true),
             new GraphValue(0,true),
             new GraphValue(0,true),
             new GraphValue(0,true),
            };
            
            _xLabelBarChart = delegate (int index)
            {
                switch (index)
                {
                    case 0:
                        return "Uninfected";
                    case 1:
                        return "Infected";
                    case 2:
                        return "Infectious";
                    case 3:
                        return "Recovered";
                    default:
                        return "undefined";
                }

            };
            BarchartGameObject.GetComponent<GraphChart>().TypeOfGraph = GraphChart.GraphType.BarChart;
        }


        //E.g called each day or each state transition
        private void UpdateLineGraphValues()
        {
            AddValuesToLinesList();
            //Clear lists each 7 days
            if (_lines[0].Values.Count == 8)
            {
                /*
                foreach (var line in _lines)
                {
                    _lines.Clear();
                }*/
                /*
                _lines[0].Clear();
                _lines[1].Clear();
                _lines[2].Clear();
                _lines[3].Clear();
                */
                _lines[0].Values.Clear();
                _lines[1].Values.Clear();
                _lines[2].Values.Clear();
                _lines[3].Values.Clear();

                //Add the cleared day
                AddValuesToLinesList();
           
            }

        }


        private void AddValuesToLinesList()
        {
            /*
            _lines[0].Add(SimulationMaster.Instance.AmountUninfected);
            _lines[1].Add(SimulationMaster.Instance.AmountInfected);
            _lines[2].Add(SimulationMaster.Instance.AmountInfectious);
            _lines[3].Add(SimulationMaster.Instance.AmountRecovered);
            */
            _lines[0].Values.Add(SimulationMaster.Instance.AmountUninfected);
            _lines[1].Values.Add(SimulationMaster.Instance.AmountInfected);
            _lines[2].Values.Add(SimulationMaster.Instance.AmountInfectious);
            _lines[3].Values.Add(SimulationMaster.Instance.AmountRecovered);


        }


        private void UpdateBarChartValues()
        {

            /*
             * FIXME UPDATE CAUSES NULL POINTER EXCEPTION
            Barchart.UpdateValue(0, SimulationMaster.Instance.AmountInfected);
            Barchart.UpdateValue(1, SimulationMaster.Instance.AmountRecovered);
            Barchart.UpdateValue(2, SimulationMaster.Instance.AmountUninfected);
          */
            /*
            _barchartValues[0] = SimulationMaster.Instance.AmountUninfected;
            _barchartValues[1] = SimulationMaster.Instance.AmountInfected;
            _barchartValues[2] = SimulationMaster.Instance.AmountInfectious;
            _barchartValues[3] = SimulationMaster.Instance.AmountRecovered;
            */
            _barchartValues[0].Value = SimulationMaster.Instance.AmountUninfected;
            _barchartValues[1].Value = SimulationMaster.Instance.AmountInfected;
            _barchartValues[2].Value = SimulationMaster.Instance.AmountInfectious;
            _barchartValues[3].Value = SimulationMaster.Instance.AmountRecovered;
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


        
        public void OnToggleChanged()
        {
            bool uninfectedOn = UninfectedToggle.isOn;
            bool infectedOn = InfectedToggle.isOn;
            bool infectiousOn = InfectiousToggle.isOn;
            bool recoveredOn = RecoveredToggle.isOn;


            _barchartValues[0].IsEnabled = uninfectedOn;
            _barchartValues[1].IsEnabled = infectedOn;
            _barchartValues[2].IsEnabled = infectiousOn;
            _barchartValues[3].IsEnabled = recoveredOn;

            _lines[0].IsEnabled = uninfectedOn;
            _lines[1].IsEnabled = infectedOn;
            _lines[2].IsEnabled = infectiousOn;
            _lines[3].IsEnabled = recoveredOn;

            UpdateValuesAndShowGraphs(false);
        }


    }
}