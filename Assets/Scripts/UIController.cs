using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to implement UI related functions/behaviour.
/// Currently this class assigns listeners to the buttons.
/// </summary>
public class UIController : MonoBehaviour
{

    //References to buttons
    public Button placeVenueButton, placeWorkplaceButton, placeHospitalButton,placeHouseholdButton,placeGraphButton;
    public Color outlineColor; //To indicate which button is clicked
    List<Button> buttonList; //To reset all buttons later

    //Probably can be replaced with actions later
    public GridManager SimulationGridManager;

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

        });

        placeWorkplaceButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeWorkplaceButton);
            SimulationGridManager.SetCurrentPrefab("Workplace");

        });

        placeHospitalButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeHospitalButton);
            SimulationGridManager.SetCurrentPrefab("Hospital");

        });

        placeHouseholdButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeHouseholdButton);
            SimulationGridManager.SetCurrentPrefab("Household");

        });

        placeGraphButton.onClick.AddListener(() =>
        {
            DisableButtonOutlineColors();
            ModifyOutlineColor(placeGraphButton);
            SimulationGridManager.SetCurrentPrefab("Graph");
            //Here a Graph must be activated in the UI...


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

}
