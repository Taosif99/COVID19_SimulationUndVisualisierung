using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace GraphChart
{
    /// <summary>
    /// Class which handles the showing of graphs and updating of the corresponding values
    /// </summary>
    public class GlobalSimulationGraph : MonoBehaviour
    {

        [SerializeField] private GameObject _fullScreenGraphGameObject;
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
            StartCoroutine(UpdateGraphsEachDay());
        }

        /// <summary>
        /// Method to show the active graphs and eventually to update the values.
        /// </summary>
        /// <param name="shouldUpdateValues">If the infection values should be updated, e.g. after a day, a week...</param>
        public void UpdateValuesAndShowGraphs(bool shouldUpdateValues)
        {
            if (BarchartGameObject.activeInHierarchy)
            {

                if (shouldUpdateValues)
                {
                    UpdateBarChartValues();
                }

                BarchartGameObject.GetComponent<GraphChart>().ShowGraph(_barchartValues, _xLabelBarChart,null,_colorList);
            }


            if (shouldUpdateValues)
            {
                UpdateLineGraphValues();
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
            List<int> infectious = new List<int>();


            _lines = new List<List<int>>();
            _lines.Add(uninfectedValues);
            _lines.Add(phase1Values);
            _lines.Add(infectious);
            //lines.Add(phase2Values);
            //lines.Add(phase3Values);
            //lines.Add(phase4Values);
            _lines.Add(phase5Values);
          
        }


        private void InitBarChart()
        {
            _barchartValues = new List<int>() { 0, 0, 0,0 };
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
            if (_lines[0].Count == 8)
            {
                /*
                foreach (var line in _lines)
                {
                    _lines.Clear();
                }*/
                _lines[0].Clear();
                _lines[1].Clear();
                _lines[2].Clear();
                _lines[3].Clear();

                //Add the cleared day
                AddValuesToLinesList();
           
            }

        }


        private void AddValuesToLinesList()
        {

            _lines[0].Add(SimulationMaster.Instance.AmountUninfected);
            _lines[1].Add(SimulationMaster.Instance.AmountInfected);
            _lines[2].Add(SimulationMaster.Instance.AmountInfectious);
            _lines[3].Add(SimulationMaster.Instance.AmountRecovered);
        }


        private void UpdateBarChartValues()
        {

            /*
             * FIXME UPDATE CAUSES NULL POINTER EXCEPTION
            Barchart.UpdateValue(0, SimulationMaster.Instance.AmountInfected);
            Barchart.UpdateValue(1, SimulationMaster.Instance.AmountRecovered);
            Barchart.UpdateValue(2, SimulationMaster.Instance.AmountUninfected);
          */
            _barchartValues[0] = SimulationMaster.Instance.AmountUninfected;
            _barchartValues[1] = SimulationMaster.Instance.AmountInfected;
            _barchartValues[2] = SimulationMaster.Instance.AmountInfectious;
            _barchartValues[3] = SimulationMaster.Instance.AmountRecovered;
            
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


        /*
        public void OnToggleChanged()
        {
            bool uninfectedOn = UninfectedToggle.isOn;
            bool infectedOn = InfectedToggle.isOn;
            bool infectiousOn = InfectiousToggle.isOn;
            bool recoveredOn = RecoveredToggle.isOn;
            int indexCounter = 0;

            if (uninfectedOn)
            {


                indexCounter++;
            }

            if (infectedOn)
            {

                indexCounter++;
            }

            if (infectiousOn)
            {

                indexCounter++;
            }

            if (recoveredOn)
            {


                indexCounter++;
            }


        }*/


    }
}