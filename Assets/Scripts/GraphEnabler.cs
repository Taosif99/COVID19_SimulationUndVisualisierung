using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which enables Graphs in scene and which enables
/// its configurtion.
/// </summary>
public class GraphEnabler : MonoBehaviour
{

    [SerializeField] private GameObject _barChart;
    [SerializeField] private GameObject _lineChart;
    [SerializeField] private GameObject _graphSettings;



    //Following functions simply enable or disable gameobjects in the scene
    public void EnableGraphSettings()
    {
        _graphSettings.SetActive(true);
    }

    public void DisableGraphSettings()
    {
        _graphSettings.SetActive(false);
    }


    public void SetBarChartActive(bool isActive)
    {
        _barChart.SetActive(isActive);
    }

    public void SetLineChartActive(bool isActive)
    {
        _lineChart.SetActive(isActive);
    }

}
