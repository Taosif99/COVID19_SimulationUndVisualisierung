using UnityEngine;
using TMPro;

public class SimulationSettingsHandler : MonoBehaviour
{

    //TODO OUTSOURCE UI CONTROLLER and input validation
    //TMP_Text maxIncubationTimeText:

    public TMP_InputField IncubationMinDayText;
    public TMP_InputField IncubationMaxDayText;
    public TMP_InputField SymptomsMinDayText;
    public TMP_InputField SymptomsMaxDayText;
    public TMP_InputField InfectiousMinDayText;
    public TMP_InputField InfectiousMaxDayText;
    public TMP_InputField RecoveringMinDayText;
    public TMP_InputField RecoveringMaxDayText;
    public TMP_InputField FatalityRateText;
    public TMP_InputField FatalityRatePreIllnessText;
    public TMP_InputField PreIllnessProbabilityText;
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



        IncubationMinDayText.text = settings.IncubationMinDay.ToString();
        IncubationMaxDayText.text = settings.IncubationMaxDay.ToString();
        SymptomsMinDayText.text = settings.SymptomsMinDay.ToString();
        SymptomsMaxDayText.text = settings.SymptomsMaxDay.ToString();
        InfectiousMinDayText.text = settings.InfectiousMinDay.ToString();
        InfectiousMaxDayText.text = settings.InfectiousMaxDay.ToString();
        RecoveringMinDayText.text = settings.RecoveringMinDay.ToString();
        RecoveringMaxDayText.text = settings.RecoveringMaxDay.ToString();
        FatalityRateText.text = settings.FatalityRate.ToString();
        FatalityRatePreIllnessText.text = settings.FatalityRatePreIllness.ToString();
        PreIllnessProbabilityText.text = settings.PreIllnessProbability.ToString();
    }


    public void CloseSettings()
    {
        SimulationSettingsGameObject.SetActive(false);
    }


    public void SaveSettingsToSimulation()
    {


        Simulation.Edit.Simulation simulation = SimulationMaster.Instance.CurrentSimulation;
        Simulation.Edit.AdjustableSimulationSettings settings = simulation.SimulationOptions.AdjustableSimulationPrameters;


        settings.IncubationMinDay = int.Parse(IncubationMinDayText.text);
        settings.IncubationMaxDay = int.Parse(IncubationMaxDayText.text);
        settings.SymptomsMinDay = int.Parse(SymptomsMinDayText.text);
        settings.SymptomsMaxDay = int.Parse(SymptomsMaxDayText.text);
        settings.InfectiousMinDay = int.Parse(InfectiousMinDayText.text);
        settings.InfectiousMaxDay = int.Parse(InfectiousMaxDayText.text);
        settings.RecoveringMinDay = int.Parse(RecoveringMinDayText.text);
        settings.RecoveringMaxDay = int.Parse(RecoveringMaxDayText.text);
        settings.FatalityRatePreIllness = float.Parse(FatalityRatePreIllnessText.text);
        settings.PreIllnessProbability = float.Parse(PreIllnessProbabilityText.text);

    }


}
