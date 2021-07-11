using System;
using UnityEngine;
using InputValidation;
using Simulation.Edit;
using DialogBoxSystem;
using Event = Simulation.Edit.Event;

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
        simulation.SimulationOptions.AdjustableSimulationPrameters ??= new AdjustableSimulationSettings();
        simulation.SimulationOptions.Policies ??= new Policies(MaskType.None);

        DisplaySettings(simulation.SimulationOptions);
    }

    public void CloseSettings()
    {
        SimulationSettingsGameObject.SetActive(false);
    }

    public void SaveSettingsToSimulation()
    {
        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        SimulationOptions currentOptions = simulation.SimulationOptions;

        AdjustableSimulationSettings defaultSettings = new AdjustableSimulationSettings();
        AdjustableSimulationSettings settingsToSet = new AdjustableSimulationSettings();

        int latencyTime = defaultSettings.LatencyTime;
        int amountDaysInfectious = defaultSettings.AmountDaysInfectious;
        int incubationTime = defaultSettings.IncubationTime;
        int amountDaysSymptoms = defaultSettings.AmountDaysSymptoms;
        float recoveringProbability = defaultSettings.RecoveringProbability;
        float recoveringInHospitalProbability = defaultSettings.RecoveringInHospitalProbability;
        float personSurvivesIntensiveCareProbability = defaultSettings.PersonSurvivesIntensiveCareProbability;
        float infectionRiskIfRecovered = defaultSettings.InfectionRiskIfRecovered;
        int daysFromSymptomsBeginToDeath = defaultSettings.DaysFromSymptomsBeginToDeath;
        int daysInHospital = defaultSettings.DaysInHospital;
        int durationOfSymptomsbeginToHospitalization = defaultSettings.DurationOfSymtombeginToHospitalization;
        int daysInIntensiveCare = defaultSettings.DaysInIntensiveCare;
        int durationOfHospitalizationToIntensiveCare = defaultSettings.DurationOfHospitalizationToIntensiveCare;
        int amountDaysQuarantine = defaultSettings.AmountDaysQuarantine;
        int advancedQuarantineDays = defaultSettings.AdvancedQuarantineDays;
        

        bool infectionPhaseParametersAreValid = InputValidator.ValidateSimulationParameters(ref latencyTime, ref amountDaysInfectious, ref incubationTime, ref amountDaysSymptoms);
        bool healthPhaseParametersAreValid = InputValidator.ValidateHealthPhaseParameters(ref recoveringProbability, ref recoveringInHospitalProbability, ref personSurvivesIntensiveCareProbability, ref daysFromSymptomsBeginToDeath, ref infectionRiskIfRecovered);
        bool hospitalParametersAreValid = InputValidator.ValidateHospitalParameters(ref daysInHospital, ref durationOfSymptomsbeginToHospitalization, ref daysInIntensiveCare, ref durationOfHospitalizationToIntensiveCare);
        bool validQuarantineParameters = InputValidator.ValidateQuarantineParameters(ref amountDaysQuarantine, ref advancedQuarantineDays);

        if (infectionPhaseParametersAreValid && healthPhaseParametersAreValid && hospitalParametersAreValid && validQuarantineParameters)
        {
            settingsToSet.LatencyTime = latencyTime;
            settingsToSet.AmountDaysInfectious = amountDaysInfectious;
            settingsToSet.IncubationTime = incubationTime;
            settingsToSet.AmountDaysSymptoms = amountDaysSymptoms;
            settingsToSet.RecoveringProbability = recoveringProbability;
            settingsToSet.RecoveringInHospitalProbability = recoveringInHospitalProbability;
            settingsToSet.PersonSurvivesIntensiveCareProbability = personSurvivesIntensiveCareProbability;
            settingsToSet.DaysFromSymptomsBeginToDeath = daysFromSymptomsBeginToDeath;
            settingsToSet.InfectionRiskIfRecovered = infectionRiskIfRecovered;

            settingsToSet.DaysInHospital = daysInHospital;
            settingsToSet.DurationOfSymtombeginToHospitalization = durationOfSymptomsbeginToHospitalization;
            settingsToSet.DaysInIntensiveCare = daysInIntensiveCare;
            settingsToSet.DurationOfHospitalizationToIntensiveCare = durationOfHospitalizationToIntensiveCare;

            settingsToSet.AmountDaysQuarantine = amountDaysQuarantine;
            settingsToSet.AdvancedQuarantineDays = advancedQuarantineDays;

            if (settingsToSet.RangesAreValid())
            {
                simulation.SimulationOptions.AdjustableSimulationPrameters = settingsToSet;
                simulation.SimulationOptions.Policies.RequiredMaskType = (MaskType) UIController.Instance.MaskTypeDropdown.value;
            }

            else if (!_saveLock)
            {
                Debug.Log("Wrong ranges!");
                string msg = "Change not  saved! Please make sure that the ranges are correct (see tooltips)";
                string name = "Wrong ranges!";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.OnConfirmationPressed += () =>
                {
                    DisplaySettings(currentOptions);
                };
                dialogBox.HasCancelButton = false;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            }
        }
    }

    public void ResetDefaultSettings()
    {
        _saveLock = true;
        SimulationOptions defaultOptions = new SimulationOptions(new Policies(MaskType.None), Array.Empty<Event>(), new AdjustableSimulationSettings());
        DisplaySettings(defaultOptions);
        _saveLock = false;
    }

    private void DisplaySettings(SimulationOptions options)
    {
        UIController.Instance.MaskTypeDropdown.value = (int) options.Policies.RequiredMaskType;
        UIController.Instance.MaskTypeDropdown.RefreshShownValue();
        
        AdjustableSimulationSettings internalParameters = options.AdjustableSimulationPrameters;
        
        UIController.Instance.LatencyInputField.text = internalParameters.LatencyTime.ToString();
        UIController.Instance.AmountDaysInfectiousInputField.text = internalParameters.AmountDaysInfectious.ToString();
        UIController.Instance.IncubationTimeInputField.text = internalParameters.IncubationTime.ToString();
        UIController.Instance.AmountDaysSymptomsInputField.text = internalParameters.AmountDaysSymptoms.ToString();
        UIController.Instance.RecoverInputField.text = internalParameters.RecoveringProbability.ToString();
        UIController.Instance.RecoverInHospitalInputField.text = internalParameters.RecoveringInHospitalProbability.ToString();
        UIController.Instance.SurviveIntensiveCareInputField.text = internalParameters.PersonSurvivesIntensiveCareProbability.ToString();
        UIController.Instance.AmountDaysToDeathInputField.text = internalParameters.DaysFromSymptomsBeginToDeath.ToString();
        UIController.Instance.DaysInHosputalInputField.text = internalParameters.DaysInHospital.ToString();
        UIController.Instance.DaysSymptomsBeginToHospitalizationInputField.text = internalParameters.DurationOfSymtombeginToHospitalization.ToString();
        UIController.Instance.DaysIntensiveCareInputField.text = internalParameters.DaysInIntensiveCare.ToString();
        UIController.Instance.DaysRegularToIntensiveInputField.text = internalParameters.DurationOfHospitalizationToIntensiveCare.ToString();
        UIController.Instance.QuarantineDaysInputField.text = internalParameters.AmountDaysQuarantine.ToString();
        UIController.Instance.AdvancedQuarantineDaysInputField.text = internalParameters.AdvancedQuarantineDays.ToString();
        UIController.Instance.InfectionRiskIfRecoveredInputField.text = internalParameters.InfectionRiskIfRecovered.ToString();
    }
}