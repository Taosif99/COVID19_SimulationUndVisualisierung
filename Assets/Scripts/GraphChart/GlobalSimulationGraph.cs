using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace GraphChart
{
    /// <summary>
    /// Class which handles the showing of graphs and updating of the corresponding values
    /// </summary>
    public class GlobalSimulationGraph : MonoBehaviour
    {


        [SerializeField] private GraphChart _multiLineGraph;
        [SerializeField] private GraphChart _barchart;


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

            _barchart.TypeOfGraph = GraphChart.GraphType.BarChart;
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


        private void UpdateBarChart()
        {
            

            _barchart.UpdateValue(0, SimulationMaster.Instance.AmountInfected);
            _barchart.UpdateValue(1, SimulationMaster.Instance.AmountRecovered);
            _barchart.UpdateValue(2, SimulationMaster.Instance.AmountUninfected);
          
       
        }

        private void UpdateValuesAndShowGraphs()
        {



            // UpdateGraph();
            if (!_barChartCreated && _barchart.isActiveAndEnabled)
            {
                _barchart.ShowGraph(_barchartValues, _xLabelBarChart);
                _barChartCreated = true;

            }

            if (_barChartCreated && _barchart.isActiveAndEnabled)
            {

                UpdateBarChart();

            }

         
            UpdateLineGraphValues();
            if (_multiLineGraph.isActiveAndEnabled)
            {
               
                _multiLineGraph.ShowMultiLineGraph(_lines, _colorList, _xLabelMultilineGraph);
            }

        }


        private IEnumerator UpdateGraphsEachDay()
        {
            for (; ; )
            {
                //A day takes approx. 8 seconds
                //TODO CALCULATE DAY LENGTH VIA CODE         
                yield return new WaitForSeconds(8f);
                UpdateValuesAndShowGraphs();

            }
        }



    }
}