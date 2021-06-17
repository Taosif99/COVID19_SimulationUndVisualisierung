using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphChart
{
    /// <summary>
    /// Examples how the GraphChart Class can be used (Barchart,Linegraph and Multilinegraph)
    /// This class is mainly used for debug purposes.
    /// </summary>
    public class GraphChartTest : MonoBehaviour
    {

        [SerializeField] private GraphChart _graphChart;
        public List<int> valueList;

        public bool startExample = false;

        public bool LineBar1Enabled = true;
        public bool LineBar2Enabled = true;
        public bool LineBar3Enabled = true;

        public bool CalculateRandomNumbers = false;


        public enum GraphTestType
        {
            BarChart,
            LineGraph,
            Multiline
        }

        public GraphTestType UsedGraphTestType = GraphTestType.BarChart;

        private List<Color> _colorList;



        void Awake()
        {
            valueList = new List<int>() { 5, 98, 56, 45 };
            Color c1 = Color.white;
            Color c2 = Color.yellow;
            Color c3 = Color.green;
            _colorList = new List<Color>();
            _colorList.Add(c1);
            _colorList.Add(c2);
            _colorList.Add(c3);

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
                        //StartCoroutine(ExampleBarChartSimulation());
                        StartCoroutine(ExampleBarChartSimulation2());
                        break;
                    case GraphTestType.Multiline:
                        //StartCoroutine(ExampleMultipleLineChartSimulation());
                        StartCoroutine(ExampleMultipleLineChartSimulation2());
                        break;
                }
            }
        }




        #region Test methods

        private IEnumerator ExampleLineChartSimulation()
        {
            _graphChart.TypeOfGraph = GraphChart.GraphType.LineGraph;
            List<GraphValue> graphValueList = new List<GraphValue>();
            for (; ; )
            {

                
        

                int randomNumber = UnityEngine.Random.Range(0, 100);
                //valueList.Add(randomNumber);
                graphValueList.Add(new GraphValue(randomNumber, true));
                //For better performance an addValue method may be required for the graph
                _graphChart.ShowGraph(graphValueList);
                
                
                string valStr = "";
                foreach (GraphValue graphValue in graphValueList)
                {

                    valStr += graphValue.Value;
                    valStr += ", ";
                }
                Debug.Log(valStr);
                
                yield return new WaitForSeconds(1f);

            }
        }

    

        private IEnumerator ExampleMultipleLineChartSimulation2()
        {


            List<int> list1 = new List<int> { 10, 12, 67, 33 };
            List<int> list2 = new List<int> { 20, 82, 24 };
            List<int> list3 = new List<int> { 55, 32, 34 };


            List<Line> lineList = new List<Line>();
            Line line1 = new Line(list1, LineBar1Enabled);
            Line line2 = new Line(list2, LineBar2Enabled);
            Line line3 = new Line(list3, LineBar3Enabled);
            lineList.Add(line1);
            lineList.Add(line2);
            lineList.Add(line3);



            _graphChart.ShowMultiLineGraph(lineList, _colorList);

            for (; ; )
            {

                if (CalculateRandomNumbers)
                {

                    int randomNumber1 = UnityEngine.Random.Range(0, 100);
                    int randomNumber2 = UnityEngine.Random.Range(0, 100);
                    int randomNumber3 = UnityEngine.Random.Range(0, 100);

                    list1.Add(randomNumber1);
                    list2.Add(randomNumber2);
                    list3.Add(randomNumber3);
                }

                line1.IsEnabled = LineBar1Enabled;
                line2.IsEnabled = LineBar2Enabled;
                line3.IsEnabled = LineBar3Enabled;
                _graphChart.ShowMultiLineGraph(lineList, _colorList);
                yield return new WaitForSeconds(1f);

            }

        }



        private IEnumerator ExampleBarChartSimulation2()
        {
            //Example how to declare a label delegate function...
            Func<int, string> testXLabel = delegate (int index)
            {
                switch (index)
                {
                    case 0:
                        return "Uninfected";
                    case 1:
                        return "Infected";
                    case 2:
                        return "Recovered";
                    default:
                        return "undefined";
                }

            };

            List<GraphValue> graphValueList = new List<GraphValue>();
           GraphValue graphValue1 = new GraphValue(10, LineBar1Enabled);
           GraphValue graphValue2 = new GraphValue(20, LineBar2Enabled);
           GraphValue graphValue3 = new GraphValue(255, LineBar3Enabled);
         

            graphValueList.Add(graphValue1);
            graphValueList.Add(graphValue2);
            graphValueList.Add(graphValue3);


            _graphChart.ShowGraph(graphValueList, _colorList, testXLabel);

            for (; ; )
            {
                for (int i = 0; i < 3; i++)
                {

                    if (CalculateRandomNumbers)
                    {
                        int randomNumber = UnityEngine.Random.Range(0, 100);
                        graphValueList[i].Value = randomNumber;
                    }
                    graphValueList[0].IsEnabled = LineBar1Enabled;
                    graphValueList[1].IsEnabled = LineBar2Enabled;
                    graphValueList[2].IsEnabled = LineBar3Enabled;

                    //_graphChart.UpdateValue(i, randomNumber);
                    _graphChart.ShowGraph(graphValueList, _colorList, testXLabel);
                    yield return new WaitForSeconds(2f);
                }
            }
        }

        #endregion
    }
}