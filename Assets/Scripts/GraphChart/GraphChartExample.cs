using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphChart
{
    /// <summary>
    /// Examples how the GraphChart Class can be used (Barchart,Linegraph and Multilinegraph)
    /// </summary>
    public class GraphChartExample : MonoBehaviour
    {

        [SerializeField] private GraphChart _graphChart;
        public List<int> valueList;

        public bool startExample = false;

        public enum GraphTestType
        {
            BarChart,
            LineGraph,
            Multiline
        }

        public GraphTestType UsedGraphTestType = GraphTestType.BarChart;


        void Awake()
        {
            valueList = new List<int>() { 5, 98, 56, 45 };
        }


        // Start is called before the first frame update
        void Start()
        {

            if (startExample)
            {            //Test/Showcase 
                switch (UsedGraphTestType)
                {
                    case GraphTestType.LineGraph:
                        StartCoroutine(ExampleLineChartSimulation());
                        break;
                    case GraphTestType.BarChart:
                        StartCoroutine(ExampleBarChartSimulation());
                        break;
                    case GraphTestType.Multiline:
                        StartCoroutine(ExampleMultipleLineChartSimulation());
                        break;
                }
            }
        }




        #region Test methods
        private IEnumerator ExampleBarChartSimulation()
        {
            //Example how to declare a label delegate function...
            Func<int, string> testXLabel = delegate (int index)
            {
                switch (index)
                {
                    case 0:
                        return "Susceptible";
                    case 1:
                        return "Infected";
                    case 2:
                        return "Recovered";
                    case 3:
                        return "Dead";
                    default:
                        return index.ToString();
                }

            };

            _graphChart.ShowGraph(valueList, testXLabel);

            for (; ; )
            {
                for (int i = 0; i < 4; i++)
                {
                    int randomNumber = UnityEngine.Random.Range(0, 100);
                    valueList[i] = randomNumber;
                    _graphChart.UpdateValue(i, randomNumber);

                    string valStr = "";
                    foreach (int val in valueList)
                    {
                        valStr += val;
                        valStr += ", ";
                    }
                    //Debug.Log(valStr);
                    yield return new WaitForSeconds(2f);
                }
            }
        }



        private IEnumerator ExampleLineChartSimulation()
        {
            _graphChart.TypeOfGraph = GraphChart.GraphType.LineGraph;
            for (; ; )
            {

                int randomNumber = UnityEngine.Random.Range(0, 100);
                valueList.Add(randomNumber);
                //For better performance an addValue method may be required for the graph
                _graphChart.ShowGraph(valueList);
                string valStr = "";
                foreach (int val in valueList)
                {
                    valStr += val;
                    valStr += ", ";
                }
                Debug.Log(valStr);
                yield return new WaitForSeconds(1f);

            }
        }

        private IEnumerator ExampleMultipleLineChartSimulation()
        {


            List<int> list1 = new List<int> { 10, 12, 67, 33 };
            List<int> list2 = new List<int> { 20, 82, 24 };
            List<int> list3 = new List<int> { 55, 32, 34 };

            List<List<int>> listList = new List<List<int>>();
            listList.Add(list1);
            listList.Add(list2);
            listList.Add(list3);


            //Three random colors
            Color c1 = UnityEngine.Random.ColorHSV();
            Color c2 = UnityEngine.Random.ColorHSV();
            Color c3 = UnityEngine.Random.ColorHSV();
            List<Color> colorList = new List<Color>();
            colorList.Add(c1);
            colorList.Add(c2);
            colorList.Add(c3);

            _graphChart.ShowMultiLineGraph(listList, colorList);

            for (; ; )
            {

                int randomNumber1 = UnityEngine.Random.Range(0, 100);
                int randomNumber2 = UnityEngine.Random.Range(0, 100);
                int randomNumber3 = UnityEngine.Random.Range(0, 100);

                list1.Add(randomNumber1);
                list2.Add(randomNumber2);
                list3.Add(randomNumber3);
                _graphChart.ShowMultiLineGraph(listList, colorList);
                yield return new WaitForSeconds(1f);

            }

        }

        #endregion
    }
}