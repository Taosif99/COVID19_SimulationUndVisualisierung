using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EditorObjects;
using Grid;
using TMPro;
using GraphChart;

/// <summary>
/// Script to implement UI related functions/behaviour.
/// Currently this class assigns listeners to the buttons and manages
/// enabling and disabling of UI elements.
/// 
/// 
/// TODO IMPROVE SINGLETON
/// 
/// </summary>
public class UIController : MonoBehaviour
{
    //References to buttons
    public Button placeWorkplaceButton, placeHospitalButton, placeHouseholdButton, placeGraphButton;
    //Color to indicate which button is clicked
    public Color outlineColor;

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
    public GameObject VenueUI;
    public GameObject HouseholdUI;
    public GameObject WorkplaceUI;
    public GameObject HospitalUI;
    public GameObject DeleteEntityGameObject;
    public TMP_Text RValueText;
    // All texts field which must be resettet will be cached in this list
    public List<TMP_InputField> InputFieldsToReset = new List<TMP_InputField>();

    //Left Image UI Elements
    [Header("Left Image UI children")]
    //Venue elements
    public TMP_InputField ObjectNameInputField;
    public TMP_InputField InfectionRiskInputField; //TODO Slider
                                                 
    //Household elements
    public TMP_InputField NumberOfPeopleInputField;
    public TMP_InputField PercantageOfWorkersInputField; //TODO Slider
    public TMP_InputField CarefulnessInputField; //TODO Slider
    //Workplace elements
    public TMP_Dropdown WorkplaceTypeDropdown;
    public TMP_InputField WorkerCapacityInputField;
    //Hospital elements
    public TMP_Dropdown HospitalScaleDropdown;
    public TMP_Dropdown WorkerAvailabilityDropdown;
    
    //inputfields and dropdowns of left image
    private List<TMP_InputField> _leftInputFields; //TODO PRIVATE
    private List<TMP_Dropdown> _leftDropDowns;

    [Header("References to manager(s)")]
    //Will be replaced
    [SerializeField] private EditorObjectsManager editorObjectsManager;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _leftInputFields = new List<TMP_InputField> {ObjectNameInputField,InfectionRiskInputField,NumberOfPeopleInputField,
          Instance.PercantageOfWorkersInputField,CarefulnessInputField,WorkerCapacityInputField};
        _leftDropDowns = new List<TMP_Dropdown> {WorkplaceTypeDropdown, HospitalScaleDropdown,WorkerAvailabilityDropdown };
        //Adding listeners to left UI
        AddOnChangeListenersToLeftUI();



        buttonList = new List<Button>
        {
            placeWorkplaceButton,
            placeHospitalButton,
            placeHouseholdButton,
            placeGraphButton
        };

        _placementButtonDictionary = new Dictionary<PrefabName, Button>();
        _placementButtonDictionary.Add(PrefabName.Workplace, placeWorkplaceButton);
        _placementButtonDictionary.Add(PrefabName.Hospital, placeHospitalButton);
        _placementButtonDictionary.Add(PrefabName.Household, placeHouseholdButton);

        foreach (PrefabName key in _placementButtonDictionary.Keys)
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
        }

        placeGraphButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeGraphButton);
            //Here a Graph must be activated in the UI...
            SimulationGraphEnabler.EnableGraphSettings();
            _lastClickedButton = placeGraphButton;
            DeactivateOldSettingsElements();
            ModelSelector.Instance.CurrentPrefabName = PrefabName.None;
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
        outline.effectColor = outlineColor;
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

        if (_lastClickedButton == placeWorkplaceButton)
        {
            LoadWorkplaceUI();
        }

        if (_lastClickedButton == placeHospitalButton)
        {
            LoadHospitalUI();
        }

        //Handle Graph related UI components
        if (_lastClickedButton != placeGraphButton)
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

        if (_lastClickedButton == placeHouseholdButton)
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
    ///Method which disables UI elements when no entity is selected 
    /// </summary>
    /// <param name="isSelected"></param>
    public void IsEntitySelectedUI(bool isSelected)
    {
        DeleteEntityGameObject.SetActive(isSelected);
        if (!isSelected)
        {
            VenueUI.SetActive(false);
            HouseholdUI.SetActive(false);
            WorkplaceUI.SetActive(false);
            HospitalUI.SetActive(false);
        }
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
}
