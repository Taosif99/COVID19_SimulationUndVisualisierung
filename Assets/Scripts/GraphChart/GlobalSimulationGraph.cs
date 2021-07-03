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
        [SerializeField] private GameObject _fullScreenGraphGameObject;

        //Multiline
        private List<Line> _lines;

        private List<Color> _colorList;
        private Func<int, string> _xLabelMultilineGraph;
        //Barchart
        private List<GraphValue> _barchartValues;
        private Func<int, string> _xLabelBarChart;
        //To use the more performant update value of a barchart
        private bool _barChartCreated = false;
        private int _defaultAmountGraphHorizontalLines;


        public static GlobalSimulationGraph Instance;

        public bool BarChartCreated { get => _barChartCreated; set => _barChartCreated = value; }
        public GameObject MultiLineGraphGameObject { get => _multiLineGraphGameObject; set => _multiLineGraphGameObject = value; }
        public GameObject BarchartGameObject { get => _barchartGameObject; set => _barchartGameObject = value; }


        //TODO OUTSOURCE UI CONTROLLER
        public Toggle UninfectedToggle;
        public Toggle InfectedToggle;
        public Toggle InfectiousToggle;
        public Toggle RecoveredToggle;
        public Toggle DeadToggle;


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
            _defaultAmountGraphHorizontalLines = _barchartGameObject.GetComponent<GraphChart>().AmountHorizontalLines;
           
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
        /// <summary>
        /// Method which resets all graphs
        /// </summary>
       public void Reset()
        {

            BarchartGameObject.GetComponent<GraphChart>().ReInitLists();
            MultiLineGraphGameObject.GetComponent<GraphChart>().ReInitLists();
            _fullScreenGraphGameObject.GetComponent<GraphChart>().ReInitLists();
            InitMultiLineGraph();
            InitBarChart();
        }

        private void InitColorList()
        {
      
            Color uninfectedColor = Color.white;
            Color infectedColor = Color.yellow;
            Color infectiousColor = Color.red;
            Color recoveredColor = Color.green;
            Color deadColor = Color.black;

            _colorList = new List<Color>();
            _colorList.Add(uninfectedColor);
            _colorList.Add(infectedColor);
            _colorList.Add(infectiousColor);
            _colorList.Add(recoveredColor);
            _colorList.Add(deadColor);

        }

   
        private void InitMultiLineGraph()
        {

            if (_lines != null)
            {
                for (int i = 0; i < _lines.Count; i++)
                {
                    _lines[i].Values.Clear();
                }

                _lines.Clear();
            }
                
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
            List<int>  deadValues = new List<int>();

            Line phase1Line = new Line(phase1Values, true);
            Line phase2Line = new Line(phase2Values, true);
            Line phase3Line = new Line(phase3Values, true);
            Line phase4Line = new Line(phase4Values, true);
            Line phase5Line = new Line(phase5Values, true);
            Line uninfectedLine = new Line(uninfectedValues, true);
            Line infectiousLine = new Line(infectiousValues,true);
            Line deadLine = new Line(deadValues, true);
            _lines = new List<Line>();
            _lines.Add(uninfectedLine);
            _lines.Add(phase1Line);
            _lines.Add(infectiousLine);
           // _lines.Add(phase2Line);
            //_lines.Add(phase3Line);
            //_lines.Add(phase4Line);
            _lines.Add(phase5Line);
            _lines.Add(deadLine);

        }


        private void InitBarChart()
        {
            if(_barchartValues != null)
                _barchartValues.Clear();
            _barchartValues = new List<GraphValue>() 
            { 
             new GraphValue(0,true),
             new GraphValue(0,true),
             new GraphValue(0,true),
             new GraphValue(0,true),
              new GraphValue(0,true)
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
                    case 4:
                        return "Dead";
                    default:
                        return "undefined";
                }

            };
            BarchartGameObject.GetComponent<GraphChart>().TypeOfGraph = GraphChart.GraphType.BarChart;
        }


        //E.g called each day or each state transition
        private void UpdateLineGraphValues()
        {

            //TODO UPDATE EACH MONTH
            if (_lines[0].Values.Count < 30)
            {

                AddValuesToLinesList();
            }
            else
            {

                _lines[0].Values.Clear();
                _lines[1].Values.Clear();
                _lines[2].Values.Clear();
                _lines[3].Values.Clear();
                _lines[4].Values.Clear();
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
            _lines[4].Values.Add(SimulationMaster.Instance.AmountPeopleDead);

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
            _barchartValues[4].Value = SimulationMaster.Instance.AmountPeopleDead;
        }



        
        public void OnToggleChanged()
        {
            bool uninfectedOn = UninfectedToggle.isOn;
            bool infectedOn = InfectedToggle.isOn;
            bool infectiousOn = InfectiousToggle.isOn;
            bool recoveredOn = RecoveredToggle.isOn;
            bool deadOn = DeadToggle.isOn;

            _barchartValues[0].IsEnabled = uninfectedOn;
            _barchartValues[1].IsEnabled = infectedOn;
            _barchartValues[2].IsEnabled = infectiousOn;
            _barchartValues[3].IsEnabled = recoveredOn;
            _barchartValues[4].IsEnabled = deadOn;

            _lines[0].IsEnabled = uninfectedOn;
            _lines[1].IsEnabled = infectedOn;
            _lines[2].IsEnabled = infectiousOn;
            _lines[3].IsEnabled = recoveredOn;
            _lines[4].IsEnabled = deadOn;
            UpdateValuesAndShowGraphs(false);
            
        }

        /// <summary>
        /// Method which sets the amount of horizontal lines depending on the population size.
        /// </summary>
        public void AmountHorizontalLineUpdater()
        {
            int amountPeople = SimulationMaster.Instance.GetAmountAllPeople();
            if (amountPeople < _defaultAmountGraphHorizontalLines)
            {

                _barchartGameObject.GetComponent<GraphChart>().AmountHorizontalLines = amountPeople;
                _multiLineGraphGameObject.GetComponent<GraphChart>().AmountHorizontalLines = amountPeople; 
                _fullScreenGraphGameObject.GetComponent<GraphChart>().AmountHorizontalLines = amountPeople;
            }

        }

    }
}