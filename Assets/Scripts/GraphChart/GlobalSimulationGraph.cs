using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GraphChart
{
    public class GlobalSimulationGraph : MonoBehaviour
    {


        [SerializeField] private GraphChart _multiLineGraph;
        //public List<int> valueList;


        public Action OnUpdate;




        private List<List<int>> _lines;
        private List<Color> _colorList;


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
            //lines.Add(phase5Values);
            _lines.Add(uninfectedValues);


            //For simplification only showing infected and uninfected
            Color infectedColor = Color.yellow;
            Color uninfectedColor = Color.green;
   
            _colorList = new List<Color>();
            _colorList.Add(infectedColor);
            _colorList.Add(uninfectedColor);

            OnUpdate += UpdateLineGraph;

        }

        //E.g called each day or each state transition
        private void UpdateLineGraph() 
        {
            _lines[0].Add(SimulationMaster.Instance.GetAmountInfected());
            _lines[1].Add(SimulationMaster.Instance.GetAmountUninfected());
            _multiLineGraph.ShowMultiLineGraph(_lines, _colorList);
        
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}