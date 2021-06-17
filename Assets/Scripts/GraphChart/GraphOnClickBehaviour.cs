using UnityEngine;
using UnityEngine.EventSystems;

namespace GraphChart {
    /// <summary>
    /// This scripts defines what happens when (general in world UI) a graph is clicked.
    /// </summary>
    public class GraphOnClickBehaviour : MonoBehaviour,IPointerClickHandler
    {
       
        [SerializeField] private GameObject _fullScreenGraphGameObject;
        [SerializeField] private GameObject _lineGraphGameObject;
        [SerializeField] private GameObject _barchartGameObject;

        public void OnPointerClick(PointerEventData eventData)
        {
            GraphChart fullScreenGraphChart = _fullScreenGraphGameObject.transform.GetComponent<GraphChart>();
            GraphChart clickedGraphChart = transform.transform.GetComponent<GraphChart>();
            GraphChart.GraphType clikedGraphType = clickedGraphChart.TypeOfGraph;
            Debug.Log("You clicked a: " + clikedGraphType);
            _fullScreenGraphGameObject.SetActive(true);

            if (clikedGraphType == GraphChart.GraphType.BarChart)
            {
               // GlobalSimulationGraph.Instance.BarChartCreated = false;
                fullScreenGraphChart.TypeOfGraph = GraphChart.GraphType.BarChart;
                GlobalSimulationGraph.Instance.BarchartGameObject = _fullScreenGraphGameObject;
            }
            else

            {               
                fullScreenGraphChart.TypeOfGraph = GraphChart.GraphType.LineGraph;
                GlobalSimulationGraph.Instance.MultiLineGraphGameObject = _fullScreenGraphGameObject;
            }
            GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);
            _lineGraphGameObject.SetActive(false);
            _barchartGameObject.SetActive(false);
        }
    }
}