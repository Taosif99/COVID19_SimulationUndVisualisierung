using UnityEngine;
using InputValidation;
using Simulation.Edit;
using DialogBoxSystem;
/// <summary>
/// Class which implemenets the functionality to adjust simulation settings
/// </summary>
public class SimulationSettingsHandler : MonoBehaviour
{


    public GameObject SimulationSettingsGameObject;
    private bool _saveLock = false;


    public void LoadSettings()
    {
        SimulationSettingsGameObject.SetActive(true);

        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        AdjustableSimulationSettings settings = simulation.SimulationOptions.AdjustableSimulationPrameters;

        //Case to fix old broken saves, TODO REMOVE IN NEWER VERSIONS
        if (settings == null)
        {
            simulation.SimulationOptions.AdjustableSimulationPrameters = new AdjustableSimulationSettings();
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
        AdjustableSimulationSettings currentSettings = simulation.SimulationOptions.AdjustableSimulationPrameters;

        AdjustableSimulationSettings defaultSettings = new AdjustableSimulationSettings();
        AdjustableSimulationSettings settingsToSet = new AdjustableSimulationSettings();

        int latencyTime = defaultSettings.LatencyTime;
        int amountDaysInfectious = defaultSettings.AmountDaysInfectious;
        int incubationTime = defaultSettings.IncubationTime;
        int amountDaysSymptoms = defaultSettings.AmountDaysSymptoms;
        float recoveringProbability = defaultSettings.RecoveringProbability;
        float recoveringInHospitalProbability = defaultSettings.RecoveringInHospitalProbability;
        float personSurvivesIntensiveCareProbability = defaultSettings.PersonSurvivesIntensiveCareProbability;
        int daysFromSymptomsBeginToDeath = defaultSettings.DaysFromSymptomsBeginToDeath;
        int daysInHospital = defaultSettings.DaysInHospital;
        int durationOfSymptomsbeginToHospitalization = defaultSettings.DurationOfSymtombeginToHospitalization;
        int daysInIntensiveCare = defaultSettings.DaysInIntensiveCare;
        int durationOfHospitalizationToIntensiveCare = defaultSettings.DurationOfHospitalizationToIntensiveCare;
        int amountDaysQuarantine = defaultSettings.AmountDaysQuarantine;
        bool infectionPhaseParametersAreValid = InputValidator.ValidateSimulationParameters(ref latencyTime, ref amountDaysInfectious, ref incubationTime, ref amountDaysSymptoms);
        bool healthPhaseParametersAreValid = InputValidator.ValidateHealthPhaseParameters(ref recoveringProbability, ref recoveringInHospitalProbability, ref personSurvivesIntensiveCareProbability, ref daysFromSymptomsBeginToDeath);
        bool hospitalParametersAreValid = InputValidator.ValidateHospitalParameters(ref daysInHospital, ref durationOfSymptomsbeginToHospitalization, ref daysInIntensiveCare, ref durationOfHospitalizationToIntensiveCare);
        bool validQuarentineParameters = InputValidator.TryParseIntDayInputField(UIController.Instance.QuarantineDaysInputField, ref amountDaysQuarantine);


        if (infectionPhaseParametersAreValid && healthPhaseParametersAreValid && hospitalParametersAreValid && validQuarentineParameters)
        {

            settingsToSet.LatencyTime = latencyTime;
            settingsToSet.AmountDaysInfectious = amountDaysInfectious;
            settingsToSet.IncubationTime = incubationTime;
            settingsToSet.AmountDaysSymptoms = amountDaysSymptoms;
            settingsToSet.RecoveringProbability = recoveringProbability;
            settingsToSet.RecoveringInHospitalProbability = recoveringInHospitalProbability;
            settingsToSet.PersonSurvivesIntensiveCareProbability = personSurvivesIntensiveCareProbability;
            settingsToSet.DaysFromSymptomsBeginToDeath = daysFromSymptomsBeginToDeath;




            settingsToSet.DaysInHospital = daysInHospital;
            settingsToSet.DurationOfSymtombeginToHospitalization = durationOfSymptomsbeginToHospitalization;
            settingsToSet.DaysInIntensiveCare = daysInIntensiveCare;
            settingsToSet.DurationOfHospitalizationToIntensiveCare = durationOfHospitalizationToIntensiveCare;

            settingsToSet.AmountDaysQuarantine = amountDaysQuarantine;

            if (settingsToSet.RangesAreValid())
            {
                simulation.SimulationOptions.AdjustableSimulationPrameters = settingsToSet;

            }
            else if (!_saveLock)
            {
                Debug.Log("Wrong ranges!");
                string msg = "Change not  saved! Please make sure that the ranges are correct (see tooltips)";
                string name = "Wrong ranges!";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.OnConfirmationPressed += () =>
                {
                    DisplaySettings(currentSettings);
                };
                dialogBox.HasCancelButton = false;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            }


        }
    }


    public void ResetDefaultSettings()
    {
        _saveLock = true;
        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        AdjustableSimulationSettings settings = simulation.SimulationOptions.AdjustableSimulationPrameters;
        settings = new AdjustableSimulationSettings();
        DisplaySettings(settings);
        _saveLock = false;
    }


    private void DisplaySettings(AdjustableSimulationSettings settings)
    {
        UIController.Instance.LatencyInputField.text = settings.LatencyTime.ToString();
        UIController.Instance.AmountDaysInfectiousInputField.text = settings.AmountDaysInfectious.ToString();
        UIController.Instance.IncubationTimeInputField.text = settings.IncubationTime.ToString();
        UIController.Instance.AmountDaysSymptomsInputField.text = settings.AmountDaysSymptoms.ToString();
        UIController.Instance.RecoverInputField.text = settings.RecoveringProbability.ToString();
        UIController.Instance.RecoverInHospitalInputField.text = settings.RecoveringInHospitalProbability.ToString();
        UIController.Instance.SurviveIntensiveCareInputField.text = settings.PersonSurvivesIntensiveCareProbability.ToString();
        UIController.Instance.AmountDaysToDeathInputField.text = settings.DaysFromSymptomsBeginToDeath.ToString();
        UIController.Instance.DaysInHosputalInputField.text = settings.DaysInHospital.ToString();
        UIController.Instance.DaysSymptomsBeginToHospitalizationInputField.text = settings.DurationOfSymtombeginToHospitalization.ToString();
        UIController.Instance.DaysIntensiveCareInputField.text = settings.DaysInIntensiveCare.ToString();
        UIController.Instance.DaysRegularToIntensiveInputField.text = settings.DurationOfHospitalizationToIntensiveCare.ToString();
        UIController.Instance.QuarantineDaysInputField.text = settings.AmountDaysQuarantine.ToString();
    }

}
