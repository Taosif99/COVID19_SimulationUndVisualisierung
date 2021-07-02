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

        Simulation.Edit.AdjustableSimulationSettings defaultSettings = new Simulation.Edit.AdjustableSimulationSettings();


        int latencyTime = defaultSettings.LatencyTime;
        int amountDaysInfectious = defaultSettings.AmountDaysInfectious;
        int incubationTime = defaultSettings.IncubationTime;
        int amountDaysSymptoms = defaultSettings.AmountDaysSymptoms;

        float recoveringProbability = defaultSettings.RecoveringProbability;
        float recoveringInHospitalProbability = defaultSettings.RecoveringInHospitalProbability;
        float personSurvivesIntensiveCareProbability = defaultSettings.PersonSurvivesIntensiveCareProbability;
        int daysFromSymptomsBeginToDeath = defaultSettings.DaysFromSymptomsBeginToDeath;


        bool infectionPhaseParametersAreValid = InputValidator.ValidateSimulationParameters(ref latencyTime, ref amountDaysInfectious, ref incubationTime, ref amountDaysSymptoms);
        bool healthPhaseParametersAreValid = InputValidator.ValidateHealthPhaseParameters(ref recoveringProbability, ref recoveringInHospitalProbability, ref personSurvivesIntensiveCareProbability, ref daysFromSymptomsBeginToDeath);

        if (infectionPhaseParametersAreValid && healthPhaseParametersAreValid)
        {
            currentSettings.LatencyTime = latencyTime;
            currentSettings.AmountDaysInfectious = amountDaysInfectious;
            currentSettings.IncubationTime = incubationTime;
            currentSettings.AmountDaysSymptoms = amountDaysSymptoms;
            currentSettings.RecoveringProbability = recoveringProbability;
            currentSettings.RecoveringInHospitalProbability = recoveringInHospitalProbability;
            currentSettings.PersonSurvivesIntensiveCareProbability = personSurvivesIntensiveCareProbability;
            currentSettings.DaysFromSymptomsBeginToDeath = daysFromSymptomsBeginToDeath;
        }
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
        UIController.Instance.LatencyInputField.text = settings.LatencyTime.ToString();
        UIController.Instance.AmountDaysInfectiousInputField.text = settings.AmountDaysInfectious.ToString();
        UIController.Instance.IncubationTimeInputField.text = settings.IncubationTime.ToString();
        UIController.Instance.AmountDaysSymptomsInputField.text = settings.AmountDaysSymptoms.ToString();
        UIController.Instance.RecoverInputField.text = settings.RecoveringProbability.ToString();
        UIController.Instance.RecoverInHospitalInputField.text = settings.RecoveringInHospitalProbability.ToString();
        UIController.Instance.SurviveIntensiveCareInputField.text = settings.PersonSurvivesIntensiveCareProbability.ToString();
        UIController.Instance.AmountDaysToDeathInputField.text = settings.DaysFromSymptomsBeginToDeath.ToString();
    }

}
