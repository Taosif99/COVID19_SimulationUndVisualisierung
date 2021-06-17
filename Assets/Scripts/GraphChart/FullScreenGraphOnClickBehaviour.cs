using UnityEngine;
using UnityEngine.EventSystems;

namespace GraphChart
{
    /// <summary>
    /// This script handles what happens when the fullscreen Graph is clicked
    /// </summary>
    public class FullScreenGraphOnClickBehaviour : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField] private GameObject _lineGraphGameObject;
        [SerializeField] private GameObject _barchartGameObject;

        public void OnPointerClick(PointerEventData eventData)
        {

            GraphChart.GraphType clikedGraphType = transform.GetComponent<GraphChart>().TypeOfGraph;
            if (clikedGraphType == GraphChart.GraphType.BarChart)
            {
                // GlobalSimulationGraph.Instance.BarChartCreated = false;
                GlobalSimulationGraph.Instance.BarchartGameObject = _barchartGameObject;

            }
            else
            {
                GlobalSimulationGraph.Instance.MultiLineGraphGameObject = _lineGraphGameObject;
            }

            //Activate graphs
            bool lineChartOn = UIController.Instance.LineChartToggle.isOn;
            bool barChartOn = UIController.Instance.BarChartToggle.isOn;
            _lineGraphGameObject.SetActive(lineChartOn);
            _barchartGameObject.SetActive(barChartOn);
            //Showing directly the Graph
            GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);
            this.transform.gameObject.SetActive(false);

        }
    }
}