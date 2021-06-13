using System.Collections.Generic;
using UnityEngine;
using System;
namespace GraphChart
{
    public class GlobalSimulationGraph : MonoBehaviour
    {


        [SerializeField] private GraphChart _multiLineGraph;
        [SerializeField] private GraphChart _barchart;


        //public Action OnUpdate;



        //Multiline
        private List<List<int>> _lines;
        private List<Color> _colorList;



        //Barchart
        private List<int> _barchartValues;
        private Func<int, string> _xLabel;

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





        }

        // Start is called before the first frame update
        void Start()
        {

            //Init lineGraph
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
            Color recoveredColor = Color.white;
            Color uninfectedColor = Color.green;
            _colorList = new List<Color>();
            _colorList.Add(infectedColor);
            _colorList.Add(recoveredColor);
            _colorList.Add(uninfectedColor);

            //OnUpdate += UpdateLineGraph;


            //Init Bar chart
            //Example how to declare a label delegate function...
            Func<int, string> xLabel = delegate (int index)
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

            _xLabel = xLabel;
           _barchartValues= new List<int>() { 0, 0, 0};
            //_barchart.ShowGraph(_barchartValues, _xLabel);
            //OnUpdate += UpdateBarChart;

        }

        //E.g called each day or each state transition
        private void UpdateLineGraph() 
        {

            if (_lines != null)
            {
                _lines[0].Add(SimulationMaster.Instance.GetAmountInfected());
                _lines[1].Add(SimulationMaster.Instance.GetAmountRecovered());
                _lines[2].Add(SimulationMaster.Instance.GetAmountUninfected());
                _multiLineGraph.ShowMultiLineGraph(_lines, _colorList);
            }
        }


        private void UpdateBarChart()
        {

            _barchart.UpdateValue(0, SimulationMaster.Instance.GetAmountInfected());
            _barchart.UpdateValue(1, SimulationMaster.Instance.GetAmountRecovered());
            _barchart.UpdateValue(2, SimulationMaster.Instance.GetAmountUninfected());

        }

        //can be replaced later with action
        public void UpdateGraph()
        {
            UpdateBarChart();
            UpdateLineGraph();
        
        }

    }
}