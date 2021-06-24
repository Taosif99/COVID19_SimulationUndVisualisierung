using UnityEngine;
using TMPro;


public class SimulationSettingsHandler : MonoBehaviour
{

    //TODO OUTSOURCE UI CONTROLLER and input validation and save on change function

    public GameObject SimulationSettingsGameObject;
    
    public void LoadSettings()
    {
        SimulationSettingsGameObject.SetActive(true);

        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        Simulation.Edit.AdjustableSimulationSettings settings = simulation.SimulationOptions.AdjustableSimulationPrameters;

        //Case to fix old broken saves, TODO REMOVE IN NEWER VERSIONS
        if (settings == null)
        {
            simulation.SimulationOptions.AdjustableSimulationPrameters = new Simulation.Edit.AdjustableSimulationSettings();
            settings = simulation.SimulationOptions.AdjustableSimulationPrameters;
            Debug.Log("Repair old save");
        }


        DisplaySettings(settings);
    }


    public void CloseSettings()
    {
        SimulationSettingsGameObject.SetActive(false);
    }


    public void SaveSettingsToSimulation()
    {


        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        Simulation.Edit.AdjustableSimulationSettings settings = simulation.SimulationOptions.AdjustableSimulationPrameters;


        settings.IncubationMinDay = int.Parse(UIController.Instance.IncubationMinDayInputField.text);
        settings.IncubationMaxDay = int.Parse(UIController.Instance.IncubationMaxDayInputField.text);
        settings.SymptomsMinDay = int.Parse(UIController.Instance.SymptomsMinDayInputField.text);
        settings.SymptomsMaxDay = int.Parse(UIController.Instance.SymptomsMaxDayInputField.text);
        settings.InfectiousMinDay = int.Parse(UIController.Instance.InfectiousMinDayInputField.text);
        settings.InfectiousMaxDay = int.Parse(UIController.Instance.InfectiousMaxDayInputField.text);
        settings.RecoveringMinDay = int.Parse(UIController.Instance.RecoveringMinDayInputField.text);
        settings.RecoveringMaxDay = int.Parse(UIController.Instance.RecoveringMaxDayInputField.text);
        settings.FatalityRate = float.Parse(UIController.Instance.FatalityRateTextInputField.text);
        settings.FatalityRatePreIllness = float.Parse(UIController.Instance.FatalityRatePreIllnessInputField.text);
        settings.PreIllnessProbability = float.Parse(UIController.Instance.PreIllnessProbabilityInputField.text);

    }


    public void ResetDefaultSettings()
    {
        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        Simulation.Edit.AdjustableSimulationSettings settings = simulation.SimulationOptions.AdjustableSimulationPrameters;
        settings = new Simulation.Edit.AdjustableSimulationSettings();
        DisplaySettings(settings);
    }


    private void DisplaySettings(Simulation.Edit.AdjustableSimulationSettings settings)
    {


        UIController.Instance.IncubationMinDayInputField.text = settings.IncubationMinDay.ToString();
        UIController.Instance.IncubationMaxDayInputField.text = settings.IncubationMaxDay.ToString();
        UIController.Instance.SymptomsMinDayInputField.text = settings.SymptomsMinDay.ToString();
        UIController.Instance.SymptomsMaxDayInputField.text = settings.SymptomsMaxDay.ToString();
        UIController.Instance.InfectiousMinDayInputField.text = settings.InfectiousMinDay.ToString();
        UIController.Instance.InfectiousMaxDayInputField.text = settings.InfectiousMaxDay.ToString();
        UIController.Instance.RecoveringMinDayInputField.text = settings.RecoveringMinDay.ToString();
        UIController.Instance.RecoveringMaxDayInputField.text = settings.RecoveringMaxDay.ToString();
        UIController.Instance.FatalityRateTextInputField.text= settings.FatalityRate.ToString();
        UIController.Instance.FatalityRatePreIllnessInputField.text = settings.FatalityRatePreIllness.ToString();
        UIController.Instance.PreIllnessProbabilityInputField.text = settings.PreIllnessProbability.ToString();

    }

}
