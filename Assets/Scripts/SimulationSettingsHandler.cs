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


        bool latencyInputOk = InputValidator.TryParseIntDayInputField(UIController.Instance.LatencyInputField, ref latencyTime) ;
        bool amountDaysInfectiousInputOk = InputValidator.TryParseIntDayInputField(UIController.Instance.AmountDaysInfectiousInputField, ref amountDaysInfectious);
        bool incubationInputOk = InputValidator.TryParseIntDayInputField(UIController.Instance.IncubationTimeInputField, ref incubationTime);
        bool amountDaysSymptomsInputOk = InputValidator.TryParseIntDayInputField(UIController.Instance.AmountDaysSymptomsInputField, ref amountDaysSymptoms);


        if (latencyInputOk)
        {
            currentSettings.LatencyTime = latencyTime;
        }

        if (amountDaysInfectiousInputOk)
        {
            currentSettings.AmountDaysInfectious = amountDaysInfectious;
        }

        if (incubationInputOk)
        {
            currentSettings.IncubationTime = incubationTime;
        }

        if (amountDaysSymptomsInputOk)
        {
            currentSettings.AmountDaysSymptoms = amountDaysSymptoms;
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

    }

}
