using System.Collections;
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
    private int _currentMonth;
    private event Action<bool> _onDayPassed; //TODO PROPER EVENT

    private bool _isInitialized = false;
    private bool _isPaused = false;

    private Simulation.Runtime.SimulationController _controller;
    private float _lastSimulationUpdate;

    private bool IsRunning => _isInitialized && _isPaused == false;

    private int _amountDaysToForward = 1;

    private int _defaultAmountDaysToForward = 1;

    
    private float _forwardingProgress;
    [SerializeField]
    private Slider _forwardProgressSlider;
    [SerializeField]
    private GameObject _forwardProgressSliderGameObject;

    [SerializeField]
    private Button _forwardButton;
    [SerializeField]
    private TMP_Text _forwardProgressText;
    [SerializeField]
    private TMP_InputField _forwardInputField;


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
            Entity[] entities = _editorObjectsManager.GetAllEditorObjects()
                .Select(RuntimeObjectFactory.Create)
                .ToArray();

            _controller = new Simulation.Runtime.SimulationController();
            _controller.Initialize(entities);

            _currentDay = _controller.SimulationDate.Day;
            _currentMonth = _controller.SimulationDate.Month;
            SimulationMaster.Instance.StartUninfectedCounting();
            SimulationMaster.Instance.OnDayBegins(_controller.SimulationDate);
            SimulationMaster.Instance.PlayDate = DateTime.Now;
            GlobalSimulationGraph.Instance.AmountHorizontalLineUpdater();
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
            _controller.RunUpdate();
            _simulationDateTime.text =
                $"{_controller.SimulationDate.ToLongDateString()}\n{_controller.SimulationDate.ToShortTimeString()}";
            
            if (_currentDay != _controller.SimulationDate.Day)
            {
                OnDayChanges();
            }

            if (_currentMonth != _controller.SimulationDate.Month)
            {
                OnMonthChanges();
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
            UIController.Instance.DisableBedMessages();
            StopAllCoroutines();
            _forwardProgressSliderGameObject.SetActive(false);
        }
    }



    public void ForwardSimulation()
    {
        if (!IsRunning)
        {
            return;
        }

        _forwardButton.interactable = false;

        int amountDaysToForward;
        bool forwardInputOk = int.TryParse(_forwardInputField.text, out amountDaysToForward);
        if (!forwardInputOk) amountDaysToForward = _defaultAmountDaysToForward;         
        

        if (amountDaysToForward > 0)
        {
            _forwardProgressSliderGameObject.SetActive(true);
        }
        StartCoroutine(ForwardSimulationRoutine(amountDaysToForward)); //This is lame
        // ForwardSimulationBlocking(); //This is blocking which worse
        
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


    //TODO REMOVE
    public void ForwardSimulationBlocking()
    {
        if (!IsRunning)
        {
            return;
        }

        while (_currentDay == _controller.SimulationDate.Day)
        {
            _controller.RunUpdate();
        }
        OnDayChanges();
    }


    /// <summary>
    /// Using a coroutine to avoiding program lagging if many days are forwarded spammed.
    /// </summary>
    /// 
    private IEnumerator ForwardSimulationRoutine(int amountDaysToForward)
    {
        _forwardingProgress = 0f;
        Pause();
        SimulationMaster.Instance.IsForwardingSimulation = true;
        for (int i = 1; i <= amountDaysToForward; i++)
        {
            while (_currentDay == _controller.SimulationDate.Day)
            {
                _controller.RunUpdate();
                yield return null;
            }

            OnDayChanges();

            if (_currentMonth != _controller.SimulationDate.Month)
            { 
                OnMonthChanges();
            }

            _forwardingProgress = i /(float)amountDaysToForward ;
            _forwardProgressSlider.value = _forwardingProgress;
            _forwardProgressText.SetText((_forwardingProgress * 100).ToString("00.00") + "%");
        }
        
        SimulationMaster.Instance.IsForwardingSimulation = false;
        _forwardProgressSliderGameObject.SetActive(false);
        Play();
        _forwardButton.interactable = true;
    }


    private void OnDayChanges()
    {
        _currentDay = _controller.SimulationDate.Day;
        _onDayPassed?.Invoke(true);
        //10 min bias ???
        //Update statistics each day
        SimulationMaster.Instance.OnDayEnds();
        SimulationMaster.Instance.OnDayBegins(_controller.SimulationDate);
    }

    private void OnMonthChanges()
    {
        _currentMonth = _controller.SimulationDate.Month;
        //TODO SET UP MULTILINEGRAPH TO UPDATE EVENTUALLY
    }

}