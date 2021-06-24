using UnityEngine;
using InputValidation;


public class SimulationSettingsHandler : MonoBehaviour
{


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
        Simulation.Edit.AdjustableSimulationSettings currentSettings = simulation.SimulationOptions.AdjustableSimulationPrameters;

        Simulation.Edit.AdjustableSimulationSettings settingsToSet = new Simulation.Edit.AdjustableSimulationSettings();

        int incubationMinDay = currentSettings.IncubationMinDay;
        int incubationMaxDay = currentSettings.IncubationMaxDay;
        int symptomsMinDay = currentSettings.SymptomsMinDay;
        int symptomsMaxDay = currentSettings.SymptomsMaxDay;
        int infectiousMinDay = currentSettings.InfectiousMinDay;
        int infectiousMaxDay = currentSettings.InfectiousMaxDay;
        int recoveringMinDay = currentSettings.RecoveringMinDay;
        int recoveringMaxDay = currentSettings.RecoveringMaxDay;
        float fatalityRate = currentSettings.FatalityRate;
        float fatalityRatePreIllness = currentSettings.FatalityRatePreIllness;
        float preIllnessProbability = currentSettings.PreIllnessProbability;


        bool validIncubationTime = InputValidator.TryParseMinMaxIntDay(UIController.Instance.IncubationMinDayInputField,
                                   UIController.Instance.IncubationMaxDayInputField, ref incubationMinDay, ref incubationMaxDay);

        bool validSymptomsTime = InputValidator.TryParseMinMaxIntDay(UIController.Instance.SymptomsMinDayInputField,
                                 UIController.Instance.SymptomsMaxDayInputField, ref symptomsMinDay, ref symptomsMaxDay);

        bool validInfectiousTime = InputValidator.TryParseMinMaxIntDay(UIController.Instance.InfectiousMinDayInputField,
                                   UIController.Instance.InfectiousMaxDayInputField, ref infectiousMinDay, ref infectiousMaxDay);

        bool validRecoveringTime = InputValidator.TryParseMinMaxIntDay(UIController.Instance.RecoveringMinDayInputField,
                                   UIController.Instance.RecoveringMaxDayInputField, ref recoveringMinDay, ref recoveringMaxDay);

        bool validFatalityRate = InputValidator.TryParseFloatPercentageInputField(UIController.Instance.FatalityRateTextInputField, ref fatalityRate);
        bool validFatalityRatePreIllness = InputValidator.TryParseFloatPercentageInputField(UIController.Instance.FatalityRatePreIllnessInputField, ref fatalityRatePreIllness);
        bool validPreIllnessProbability = InputValidator.TryParseFloatPercentageInputField(UIController.Instance.PreIllnessProbabilityInputField, ref preIllnessProbability);
        if (validIncubationTime)
        {
            currentSettings.IncubationMinDay = incubationMinDay;
            currentSettings.IncubationMaxDay = incubationMaxDay;
        }
        if (validSymptomsTime)
        {
            currentSettings.SymptomsMinDay = symptomsMinDay;
            currentSettings.SymptomsMaxDay = symptomsMaxDay;
        }
        if (validInfectiousTime)
        {
            currentSettings.InfectiousMinDay = infectiousMinDay;
            currentSettings.InfectiousMaxDay = infectiousMaxDay;
        }
        if (validRecoveringTime)
        {
            currentSettings.RecoveringMinDay = recoveringMinDay;
            currentSettings.RecoveringMaxDay = recoveringMaxDay;
        }
        if(validFatalityRate)
            currentSettings.FatalityRate = fatalityRate;
        if(validFatalityRatePreIllness)
            currentSettings.FatalityRatePreIllness = fatalityRate;
        if(validPreIllnessProbability)
            currentSettings.PreIllnessProbability = fatalityRatePreIllness;
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
        UIController.Instance.FatalityRateTextInputField.text = settings.FatalityRate.ToString();
        UIController.Instance.FatalityRatePreIllnessInputField.text = settings.FatalityRatePreIllness.ToString();
        UIController.Instance.PreIllnessProbabilityInputField.text = settings.PreIllnessProbability.ToString();

    }

}
