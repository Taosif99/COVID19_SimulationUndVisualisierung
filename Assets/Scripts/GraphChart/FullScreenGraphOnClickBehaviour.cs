using UnityEngine;
using UnityEngine.EventSystems;

namespace GraphChart
{
    /// <summary>
    /// This script handles what happens when the fullscreen Graph is clicked
    /// </summary>
    public class FullScreenGraphOnClickBehaviour : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField] private GameObject _lineGraph;
        [SerializeField] private GameObject _barchart;

        public void OnPointerClick(PointerEventData eventData)
        {

            GraphChart.GraphType clikedGraphType = transform.GetComponent<GraphChart>().TypeOfGraph;
            if (clikedGraphType == GraphChart.GraphType.BarChart)
            {
                // GlobalSimulationGraph.Instance.BarChartCreated = false;
                //GlobalSimulationGraph.Instance.Barchart = _barchart.transform.GetComponent<GraphChart>();
                GlobalSimulationGraph.Instance.BarchartGameObject = _barchart;

            }
            else
            {
                //GlobalSimulationGraph.Instance.MultiLineGraph = _lineGraph.GetComponent<GraphChart>();
                GlobalSimulationGraph.Instance.MultiLineGraphGameObject = _lineGraph;
            }

            //Activate graphs
            bool lineChartOn = UIController.Instance.LineChartToggle.isOn;
            bool barChartOn = UIController.Instance.BarChartToggle.isOn;

            _lineGraph.SetActive(lineChartOn);
            _barchart.SetActive(barChartOn);
            //Showing directly the Graph
            GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);

            this.transform.gameObject.SetActive(false);

        }
    }
}