using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EditorObjects;
using Grid;
using System;

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
    //To indicate which button is clicked
    public Color outlineColor;

    //To reset all buttons 
    private List<Button> buttonList;
    //later and invoke placement related functions functions, since they methods can be abstracted
    private Dictionary<PrefabName, Button> _placementButtonDictionary;


    //Probably can be replaced with actions later
   // public GridManager2 SimulationGridManager;
    //Concerning Graph in UI
    public GraphEnabler SimulationGraphEnabler;
    public Toggle BarChartToggle;
    public Toggle LineChartToggle;
    //Needed to implement clear up logic
    private Button _lastClickedButton;

    public static UIController Instance;



    //GameObject groups of editor object UI elements
    public GameObject VenueUI;
    public GameObject HouseholdUI;
    public GameObject WorkplaceUI;
    public GameObject HospitalUI;
    public GameObject SaveAndDeleteUI;

    // All texts field which must be resettet will be cached in this list
    public List<InputField> InputFieldsToReset = new List<InputField>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {


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


        //Assign listeners Chart Toggles
        BarChartToggle.onValueChanged.AddListener(delegate
        {
            SimulationGraphEnabler.SetBarChartActive(BarChartToggle.isOn);
        });

        LineChartToggle.onValueChanged.AddListener(delegate
        {
            SimulationGraphEnabler.SetLineChartActive(LineChartToggle.isOn);
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
        foreach (InputField inputField in InputFieldsToReset)
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
    }

    public void LoadHospitalUI()
    {
        VenueUI.SetActive(true);
        HouseholdUI.SetActive(false);
        WorkplaceUI.SetActive(true);
        HospitalUI.SetActive(true);
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

        SaveAndDeleteUI.SetActive(isSelected);
        if (!isSelected)
        {
            VenueUI.SetActive(false);
            HouseholdUI.SetActive(false);
            WorkplaceUI.SetActive(false);
            HospitalUI.SetActive(false);
        }

    }



}
