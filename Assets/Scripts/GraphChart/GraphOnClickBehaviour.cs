using UnityEngine;
using UnityEngine.EventSystems;

namespace GraphChart {
    /// <summary>
    /// This scripts defines what happens when (general in world UI) a graph is clicked.
    /// </summary>
    public class GraphOnClickBehaviour : MonoBehaviour,IPointerClickHandler
    {
       
        [SerializeField] private GameObject _fullScreenGraph;
        [SerializeField] private GameObject _lineGraph;
        [SerializeField] private GameObject _barchart;


        public void OnPointerClick(PointerEventData eventData)
        {
            GraphChart fullScreenGraphChart = _fullScreenGraph.transform.GetComponent<GraphChart>();
            GraphChart clickedGraphChart = transform.transform.GetComponent<GraphChart>();

            GraphChart.GraphType clikedGraphType = clickedGraphChart.TypeOfGraph;
            Debug.Log("You clicked a: " + clikedGraphType);


           


            if (clikedGraphType == GraphChart.GraphType.BarChart)
            {
               // GlobalSimulationGraph.Instance.BarChartCreated = false;

                fullScreenGraphChart.TypeOfGraph = GraphChart.GraphType.BarChart;
                //GlobalSimulationGraph.Instance.Barchart = fullScreenGraphChart;
                GlobalSimulationGraph.Instance.BarchartGameObject = _fullScreenGraph;

                //Showing directly the Graph

                GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);
            }
            else

            {
                
                fullScreenGraphChart.TypeOfGraph = GraphChart.GraphType.LineGraph;
                //GlobalSimulationGraph.Instance.MultiLineGraph =fullScreenGraphChart;
                GlobalSimulationGraph.Instance.MultiLineGraphGameObject = _fullScreenGraph;
                //Showing directly the Graph
                GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);

            }

            _fullScreenGraph.SetActive(true);
            _lineGraph.SetActive(false);
            _barchart.SetActive(false);
            
        }
    }
}