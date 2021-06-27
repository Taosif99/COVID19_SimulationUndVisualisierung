using System.Collections.Generic;
using System.Linq;
using EditorObjects;
using RuntimeObjects;
using Simulation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using GraphChart;
using System;
using UnityEngine.UI;

class SimulationController : MonoBehaviour
{
    private const float SimulationInterval = 0.05f;

    [SerializeField]
    private EditorObjectsManager _editorObjectsManager;

    [SerializeField]
    private TMP_Text _simulationDateTime;

    [SerializeField]
    private Button _virusButton;

    private int _currentDay;
    private event Action<bool> _onDayPassed; //TODO PROPER EVENT

    private bool _isInitialized = false;
    private bool _isPaused = false;
    
    private Simulation.Runtime.SimulationController _controller;
    private float _lastSimulationUpdate;

    private bool IsRunning => _isInitialized && _isPaused == false;

    private void Awake()
    {
        Assert.IsNotNull(_editorObjectsManager);
        Assert.IsNotNull(_simulationDateTime);
        _onDayPassed += GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs;
    }

    public void Play()
    {
        if (_isInitialized == false)
        {
            List<Entity> entities = _editorObjectsManager.GetAllEditorObjects()
                .Select(RuntimeObjectFactory.Create)
                .ToList();

            _controller = new Simulation.Runtime.SimulationController();
            _controller.Initialize(entities);

            _currentDay = _controller.SimulationDate.Day;
            SimulationMaster.Instance.StartUninfectedCounting();
            SimulationMaster.Instance.OnDayBegins(_controller.SimulationDate);
            SimulationMaster.Instance.PlayDate = DateTime.Now;
            _isInitialized = true;    
        }
        else if (_isPaused == true)
        {
            _isPaused = false;
        }    
    }

    private void Update()
    {
        if (!IsRunning)
        {
            return;
        }

        if (Time.time - _lastSimulationUpdate >= SimulationInterval)
        {
            _controller.RunUpdate(10);
            _simulationDateTime.text =
                $"{_controller.SimulationDate.ToLongDateString()}\n{_controller.SimulationDate.ToShortTimeString()}";

            
            
            //Update statistics each day
            if (_currentDay != _controller.SimulationDate.Day) 
            {
               
                _currentDay = _controller.SimulationDate.Day;
                _onDayPassed?.Invoke(true);
                //10 min bias ???
                SimulationMaster.Instance.OnDayEnds();
                SimulationMaster.Instance.OnDayBegins(_controller.SimulationDate);

            }
                 
            _lastSimulationUpdate = Time.time;
        }
    }

    public void Pause()
    {
        if (!IsRunning)
        {
            return;
        }

        _isPaused = true;
    }

    public void Stop()
    {
        if (_isInitialized == true)
        {
            _isInitialized = false;
            _isPaused = false;
            _controller = null;
            _simulationDateTime.text = string.Empty;
            _virusButton.interactable = true;

            _editorObjectsManager.ReloadEditorObjects();

            SimulationMaster.Instance.Reset();
            GlobalSimulationGraph.Instance.Reset();
        }
    }

    public void ForwardSimulation()
    {
        for (int counter = 0; counter < 144; counter++)
        {
            _controller.RunUpdate(10);
        }
    }

    public void InfectRandomPerson()
    {
        if (!IsRunning)
        {
            return;
        }
       
        _controller.InfectRandomPerson();
        _virusButton.interactable = false;
    }  
}