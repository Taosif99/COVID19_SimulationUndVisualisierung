using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EditorObjects;
using TMPro;
using GraphChart;

/// <summary>
/// Script to implement UI related functions/behaviour.
/// Currently this class assigns listeners to the buttons and manages
/// enabling and disabling of UI elements.
/// 
/// 
/// 
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("UI Messages")]
    public GameObject NotEnoughBedsMessage;
    public GameObject NotEnoughIntensiveBedsMessage;

    [Header("placement button")]
    public Button PlaceWorkplaceButton, PlaceHospitalButton, PlaceHouseholdButton, PlaceGraphButton;
    //Color to indicate which button is clicked
    public Color OutlineColor;

    //To reset all buttons 
    private List<Button> buttonList;
    //later and invoke placement related functions functions, since they methods can be abstracted
    private Dictionary<PrefabName, Button> _placementButtonDictionary;
    //Needed to implement clear up logic
    private Button _lastClickedButton;

    //Probably can be replaced with actions later
    //public GridManager2 SimulationGridManager;
    //Concerning Graph in UI
    [Header("Graph UI children")]
    public GraphEnabler SimulationGraphEnabler;
    public Toggle BarChartToggle;
    public Toggle LineChartToggle;

    public static UIController Instance;

    [Header("To disable workplace type at hospital")]
    [SerializeField] private GameObject _workplaceTypeDropDownGameObject;
    [SerializeField] private GameObject _workplaceTypeTextDownGameObject;

    //GameObject groups of editor object UI elements
    [Header("Right Image UI children")]

    public TMP_Text RValueText;
    public TMP_Text RValue7Text;
    public TMP_Text IncidenceText;
    public Toggle EpidemicInfoToggle;
    [SerializeField] private GameObject RValueGameObject;
    [SerializeField] private GameObject RValue7GameObject;
    [SerializeField] private GameObject IncidenceGameObject;
    public Toggle CsvLogToggle;
    // All texts field which must be resettet will be cached in this list
    public List<TMP_InputField> InputFieldsToReset = new List<TMP_InputField>();

    //Left Image UI Elements
    [Header("Left Image UI children")]
    [SerializeField]
    private RectTransform _entityPropertiesPanel;

    //Right Image UI Elements
    [Header("Right Image UI children")]
    [SerializeField]
    private RectTransform _entitiesPanel;

    public GameObject VenueUI;
    public GameObject HouseholdUI;
    public GameObject WorkplaceUI;
    public GameObject HospitalUI;

    //Venue elements
    public TMP_InputField InfectionRiskInputField; 
                                                 
    //Household elements
    public TMP_InputField NumberOfPeopleInputField;
    public TMP_InputField PercantageOfWorkersInputField; 
    public TMP_InputField CarefulnessInputField; 
    //Workplace elements
    public TMP_Dropdown WorkplaceTypeDropdown;
    public TMP_InputField WorkerCapacityInputField;
    //Hospital elements
    public TMP_InputField AmountNormalBedsInputField;
    public TMP_InputField AmountIntensiveCareInputField;
    
    //inputfields and dropdowns of left image
    private List<TMP_InputField> _leftInputFields; 
    private List<TMP_Dropdown> _leftDropDowns;

    [Header("Simulation Settings Inputfields")]
    public TMP_InputField LatencyInputField;
    public TMP_InputField AmountDaysInfectiousInputField;
    public TMP_InputField IncubationTimeInputField;
    public TMP_InputField AmountDaysSymptomsInputField;
    public TMP_InputField RecoverInputField;
    public TMP_InputField RecoverInHospitalInputField;
    public TMP_InputField SurviveIntensiveCareInputField;
    public TMP_InputField AmountDaysToDeathInputField;
    public TMP_InputField QuarantineDaysInputField;

    [Header("References to manager(s)")]
    //Will be replaced
    [SerializeField] private EditorObjectsManager editorObjectsManager;

    public Button LastClickedButton { get => _lastClickedButton; set => _lastClickedButton = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _leftInputFields = new List<TMP_InputField> {InfectionRiskInputField,NumberOfPeopleInputField,
          Instance.PercantageOfWorkersInputField,CarefulnessInputField,WorkerCapacityInputField};
        _leftDropDowns = new List<TMP_Dropdown> {WorkplaceTypeDropdown};
        //Adding listeners to left UI
        AddOnChangeListenersToLeftUI();

        buttonList = new List<Button>
        {
            PlaceWorkplaceButton,
            PlaceHospitalButton,
            PlaceHouseholdButton,
            PlaceGraphButton
        };

        _placementButtonDictionary = new Dictionary<PrefabName, Button>();
        _placementButtonDictionary.Add(PrefabName.Workplace, PlaceWorkplaceButton);
        _placementButtonDictionary.Add(PrefabName.Hospital, PlaceHospitalButton);
        _placementButtonDictionary.Add(PrefabName.Household, PlaceHouseholdButton);

        // TODO: Remove
        /*foreach (PrefabName key in _placementButtonDictionary.Keys)
        {
            Button button = _placementButtonDictionary[key];
            button.onClick.AddListener(() =>
            {
                DisableButtonOutlineColors();
                ModifyOutlineColor(button);
                ModelSelector.Instance.SetCurrentPrefab(key);
                _lastClickedButton = button;
                DeactivateOldSettingsElements();
            });
        }*/

        // TODO: Remove
        PlaceGraphButton.onClick.AddListener(() =>
        {
            SetEntityPropertiesPanelVisible(true);
            
            DisableButtonOutlineColors();
            ModifyOutlineColor(PlaceGraphButton);
            //Here a Graph must be activated in the UI...
            SimulationGraphEnabler.EnableGraphSettings();
            _lastClickedButton = PlaceGraphButton;
            DeactivateOldSettingsElements();
        });

        //Adding listeners to left Barchart toggles

        BarChartToggle.onValueChanged.AddListener(delegate
        {
            SimulationGraphEnabler.SetBarChartActive(BarChartToggle.isOn);
            GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);
        });

        LineChartToggle.onValueChanged.AddListener(delegate
        {
            SimulationGraphEnabler.SetLineChartActive(LineChartToggle.isOn);
            GlobalSimulationGraph.Instance.UpdateValuesAndShowGraphs(false);
        });
    }

    /// <summary>
    /// Method which assigns an outline color to a button.
    /// </summary>
    /// <param name="button"></param>
    private void ModifyOutlineColor(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = OutlineColor;
        outline.enabled = true;
    }

    /// <summary>
    /// Method to disable the outline component of all buttons in the button list.
    /// </summary>
    private void DisableButtonOutlineColors()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }

    /// <summary>
    /// Has to be replaced later with an Settings Handler or something similar, TODO reset old settings
    /// </summary>
    private void DeactivateOldSettingsElements() //Houshold wie venue...
    {
        ClearInputFields();

        if (_lastClickedButton == PlaceWorkplaceButton)
        {
            LoadWorkplaceUI();
        }

        if (_lastClickedButton == PlaceHospitalButton)
        {
            LoadHospitalUI();
        }

        //Handle Graph related UI components, TODO REMOVE LATER
        if (_lastClickedButton != PlaceGraphButton)
        {
            SimulationGraphEnabler.DisableGraphSettings();
        }
        else
        {
            VenueUI.SetActive(false);
            HouseholdUI.SetActive(false);
            WorkplaceUI.SetActive(false);
            HospitalUI.SetActive(false);
        }

        if (_lastClickedButton == PlaceHouseholdButton)
        {
            LoadHouseholdUI();
        }
    }

    private void ClearInputFields()
    {
        foreach (TMP_InputField inputField in InputFieldsToReset)
        {
            inputField.text = "";
        }
    }

    public void DisableLeftGraphUI()
    {
        SimulationGraphEnabler.DisableGraphSettings();
    }

    //Methods for loading right properties
    public void LoadWorkplaceUI()
    {
        VenueUI.SetActive(true);
        HouseholdUI.SetActive(false);
        WorkplaceUI.SetActive(true);
        HospitalUI.SetActive(false);
        _workplaceTypeDropDownGameObject.SetActive(true);
        _workplaceTypeTextDownGameObject.SetActive(true);
    }

    public void LoadHospitalUI()
    {
        VenueUI.SetActive(true);
        HouseholdUI.SetActive(false);
        WorkplaceUI.SetActive(true);
        HospitalUI.SetActive(true);
        _workplaceTypeDropDownGameObject.SetActive(false);
        _workplaceTypeTextDownGameObject.SetActive(false);
    }

    public void LoadHouseholdUI()
    {
        VenueUI.SetActive(true);
        HouseholdUI.SetActive(true);
        WorkplaceUI.SetActive(false);
        HospitalUI.SetActive(false);
    }

    /// <summary>
    /// Set whether or not the entity properties should be visible.
    /// </summary>
    /// <param name="isVisible">True to show the entity properties, false to hide them</param>
    public void SetEntityPropertiesPanelVisible(bool isVisible)
    {
        _entityPropertiesPanel.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Set whether or not the entities panel should be visible.
    /// </summary>
    /// <param name="isVisible"></param>
    public void SetEntitiesPanelVisible(bool isVisible)
    {
        _entitiesPanel.gameObject.SetActive(isVisible);  
    }
 
    public void OnEpidemicInfoToggleChange()
    {
        IncidenceText.text = "";
        RValueText.text = "";
        RValue7Text.text = "";
        RValueGameObject.SetActive(EpidemicInfoToggle.isOn);
        RValue7GameObject.SetActive(EpidemicInfoToggle.isOn);
        IncidenceGameObject.SetActive(EpidemicInfoToggle.isOn);
    }
    
    /// <summary>
    /// Method to add on change listeners for better usability.
    /// </summary>
    private void AddOnChangeListenersToLeftUI()
    {
        //Adding on change listeners 
        //Debug.Log("Adding listeners: " + UIController.Instance._inputFields.Count);
        foreach (TMP_InputField inputField in _leftInputFields)
        {
            inputField.onValueChanged.AddListener(delegate { editorObjectsManager.SaveToEntity(); });
        }

        foreach (TMP_Dropdown dropdown in _leftDropDowns)
        {
            dropdown.onValueChanged.AddListener(delegate { editorObjectsManager.SaveToEntity(); });
        }
    }

    public void DisableBedMessages()
    {
        NotEnoughBedsMessage.SetActive(false);
        NotEnoughIntensiveBedsMessage.SetActive(false);
    }
}
