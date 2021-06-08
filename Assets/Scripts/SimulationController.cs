using System.Collections.Generic;
using System.Linq;
using RuntimeObjects;
using Simulation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

class SimulationController : MonoBehaviour
{
    private const float SimulationInterval = 0.05f;

    [SerializeField]
    private EditorObjectsManager _editorObjectsManager;

    [SerializeField]
    private TMP_Text _simulationDateTime;

    private bool _isRunning = false;
    
    private Simulation.Runtime.SimulationController _controller = new Simulation.Runtime.SimulationController();
    private float _lastSimulationUpdate;

    private void Awake()
    {
        Assert.IsNotNull(_editorObjectsManager);
        Assert.IsNotNull(_simulationDateTime);
    }

    public void Play()
    {
        List<Entity> entities = _editorObjectsManager.GetAllEditorObjects()
            .Select(RuntimeObjectFactory.Create)
            .ToList();
        
        _controller.Initialize(entities);
        
        _isRunning = true;
    }

    private void Update()
    {
        if (_isRunning == false)
        {
            return;
        }

        if (Time.time - _lastSimulationUpdate >= SimulationInterval)
        {
            _controller.RunUpdate();
            _simulationDateTime.text =
                $"{_controller.SimulationDate.ToLongDateString()}\n{_controller.SimulationDate.ToShortTimeString()}";
            
            _lastSimulationUpdate = Time.time;
        }
    }
}