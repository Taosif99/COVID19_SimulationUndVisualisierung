using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to implement UI related functions/behaviour.
/// Currently this class assigns listeners to the buttons and manages
/// enabling and disabling of UI elements.
/// </summary>
public class UIController : MonoBehaviour
{

    //References to buttons
    public Button placeVenueButton, placeWorkplaceButton, placeHospitalButton,placeHouseholdButton,placeGraphButton;
    public Color outlineColor; //To indicate which button is clicked
    List<Button> buttonList; //To reset all buttons later

    //Probably can be replaced with actions later
    public GridManager SimulationGridManager;
    //Converning Graph in UI
    public GraphEnabler SimulationGraphEnabler;
    public Toggle BarChartToggle;
    public Toggle LineChartToggle;
    //Needed to implement clear up logic
    private Button _lastClickedButton;
    

    // Start is called before the first frame update
    void Start()
    {
        buttonList = new List<Button>
        {
            placeVenueButton,
            placeWorkplaceButton,
            placeHospitalButton,
            placeHouseholdButton,
            placeGraphButton
        };



        //Adding listeners to each button
        placeVenueButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeVenueButton);
            SimulationGridManager.SetCurrentPrefab("Venue");
            _lastClickedButton = placeVenueButton;
            DeactivateOldSettingsElements();

        });

        placeWorkplaceButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeWorkplaceButton);
            SimulationGridManager.SetCurrentPrefab("Workplace");
            _lastClickedButton = placeWorkplaceButton;
            DeactivateOldSettingsElements();

        });

        placeHospitalButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeHospitalButton);
            SimulationGridManager.SetCurrentPrefab("Hospital");
            _lastClickedButton = placeHospitalButton;
            DeactivateOldSettingsElements();

        });

        placeHouseholdButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeHouseholdButton);
            SimulationGridManager.SetCurrentPrefab("Household");
            _lastClickedButton = placeHouseholdButton;
            DeactivateOldSettingsElements();

        });

        placeGraphButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeGraphButton);
            //Here a Graph must be activated in the UI...
            SimulationGraphEnabler.EnableGraphSettings();
            _lastClickedButton = placeGraphButton;
            DeactivateOldSettingsElements();
        });


        //Assign listeners Chart Toggles
        BarChartToggle.onValueChanged.AddListener(delegate {
            SimulationGraphEnabler.SetBarChartActive(BarChartToggle.isOn);
        });

        LineChartToggle.onValueChanged.AddListener(delegate {
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
    /// Has to be replaced later with an Settings Handler or something similar
    /// </summary>
    private void DeactivateOldSettingsElements()
    { 
        
        //Handle not graph button clean up
        if(_lastClickedButton != placeGraphButton)
        {
            SimulationGraphEnabler.DisableGraphSettings();
        }
    }
}
