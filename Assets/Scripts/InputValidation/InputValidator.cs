using UnityEngine;
using Simulation.Edit;
using TMPro;
using System.Linq;

namespace InputValidation
{

    /// <summary>
    /// Class which implements methods concerning input validation in our
    /// simulation program.
    /// </summary>
    public static class InputValidator
    {

        /// <summary>
        /// Method which parses al inputfields in the simulation scene and which checks for correct value ranges.
        /// </summary>
        /// <param name="infectionRisk"></param>
        /// <param name="capacity"></param>
        /// <param name="numberOfPeople"></param>
        /// <param name="carefulness"></param>
        /// <param name="percentageOfWorkers"></param>
        ///  /// <param name="currentSelectedEntity"></param>
        /// <returns>true if all requiered inputfields can be parsed to correct values, else false</returns>
        public static bool TryParseLeftInputFields(ref float infectionRisk, ref int capacity, ref byte numberOfPeople, ref float carefulness, ref float percentageOfWorkers, Entity currentSelectedEntity)
        {
            bool inputIsValid = true;
            if (currentSelectedEntity is Venue)
            {
                bool infectionRiskIsValid = float.TryParse(UIController.Instance.InfectionRiskInputField.text, out infectionRisk) && IsValidPercentage(infectionRisk);
                SetInputFieldColor(UIController.Instance.InfectionRiskInputField, infectionRiskIsValid);
                inputIsValid = inputIsValid && infectionRiskIsValid;

                if (currentSelectedEntity is Workplace)
                {
                    bool capacityIsValid = int.TryParse(UIController.Instance.WorkerCapacityInputField.text, out capacity) && capacity >= 0;
                    SetInputFieldColor(UIController.Instance.WorkerCapacityInputField, capacityIsValid);
                    inputIsValid = inputIsValid && capacityIsValid;
                }
                else if (currentSelectedEntity is Household)
                {
                    bool numberOfPeopleIsValid = byte.TryParse(UIController.Instance.NumberOfPeopleInputField.text, out numberOfPeople);
                    bool carefulnessIsValid = float.TryParse(UIController.Instance.CarefulnessInputField.text, out carefulness) && IsValidPercentage(carefulness);
                    bool percantageOfWorkersIsValid = float.TryParse(UIController.Instance.PercantageOfWorkersInputField.text, out percentageOfWorkers) && IsValidPercentage(percentageOfWorkers);
                    SetInputFieldColor(UIController.Instance.NumberOfPeopleInputField, numberOfPeopleIsValid);
                    SetInputFieldColor(UIController.Instance.CarefulnessInputField, carefulnessIsValid);
                    SetInputFieldColor(UIController.Instance.PercantageOfWorkersInputField, percantageOfWorkersIsValid);
                    inputIsValid = inputIsValid && numberOfPeopleIsValid;
                    inputIsValid = inputIsValid && carefulnessIsValid;
                    inputIsValid = inputIsValid && percantageOfWorkersIsValid;

                }
            }

            return inputIsValid;
        }



        /// <summary>
        /// Method which validated the simulation settings parameters.
        /// It validates the parsing and if:
        ///  IncubationTime + AmountDaysSymptoms   >=LatencyTime + AmountDaysInfectious 
        /// </summary>
        /// <param name="latencyTime"></param>
        /// <param name="amountDaysInfectious"></param>
        /// <param name="incubationTime"></param>
        /// <param name="amountDaysSymptoms"></param>
        /// <returns>true if inputs are valid, else false.</returns>
        public static bool ValidateSimulationParameters(ref int latencyTime, ref int amountDaysInfectious, ref int incubationTime, ref int amountDaysSymptoms)
        {

            bool result = false;
            bool latencyInputOk = TryParseIntDayInputField(UIController.Instance.LatencyInputField, ref latencyTime);
            bool amountDaysInfectiousInputOk = TryParseIntDayInputField(UIController.Instance.AmountDaysInfectiousInputField, ref amountDaysInfectious);
            bool incubationInputOk = TryParseIntDayInputField(UIController.Instance.IncubationTimeInputField, ref incubationTime);
            bool amountDaysSymptomsInputOk = TryParseIntDayInputField(UIController.Instance.AmountDaysSymptomsInputField, ref amountDaysSymptoms);

            result = latencyInputOk && amountDaysInfectiousInputOk && incubationInputOk && amountDaysSymptomsInputOk;

            if (result == true)
            {
                result = incubationTime + amountDaysSymptoms >= latencyTime + amountDaysInfectious;

                if (result == false)
                {
                    //All fields are wrong
                    SetInputFieldColor(UIController.Instance.LatencyInputField, false);
                    SetInputFieldColor(UIController.Instance.AmountDaysInfectiousInputField, false);
                    SetInputFieldColor(UIController.Instance.IncubationTimeInputField, false);
                    SetInputFieldColor(UIController.Instance.AmountDaysSymptomsInputField, false);
                    //TODO DIALOGBOX WHICH EXPLAINS ERROR
                }

            }
            return result;
        }

        /// <summary>
        /// Method which validates the health phase parameters.
        /// </summary>
        /// <param name="recoveringProbability"></param>
        /// <param name="recoveringInHospitalProbability"></param>
        /// <param name="personSurvivesIntensiveCareProbability"></param>
        /// <param name="daysFromSymptomsBeginToDeath"></param>
        /// <returns>true if all parameters are valid, else false</returns>
        public static bool ValidateHealthPhaseParameters(ref float recoveringProbability, ref float recoveringInHospitalProbability, ref float personSurvivesIntensiveCareProbability , ref int daysFromSymptomsBeginToDeath)
        {
            bool result = false;

            bool recoveringProbabilityInputOk = TryParseFloatPercentageInputField(UIController.Instance.RecoverInputField, ref recoveringProbability);
            bool recoveringProbabilityInHospitalInputOk = TryParseFloatPercentageInputField(UIController.Instance.RecoverInHospitalInputField, ref recoveringInHospitalProbability);
            bool personSurvivesIntensiveCareInputOk = TryParseFloatPercentageInputField(UIController.Instance.SurviveIntensiveCareInputField, ref personSurvivesIntensiveCareProbability);
            bool daysFromSymptomsBeginToDeathInputOk = TryParseIntDayInputField(UIController.Instance.AmountDaysToDeathInputField, ref daysFromSymptomsBeginToDeath);

            result = recoveringProbabilityInputOk && recoveringProbabilityInHospitalInputOk && personSurvivesIntensiveCareInputOk && daysFromSymptomsBeginToDeathInputOk;
            return result;
        }







        /// <summary>
        /// Method which checks if an inputfield text can be parsed to an integer value > 0
        /// and which sets the color if condition not fulfilled.
        /// </summary>
        /// <param name="inputField"></param>
        /// <param name="value"></param>
        /// <returns>true if condition fulfilled, else false</returns>
        public static bool TryParseIntDayInputField(TMP_InputField inputField, ref int value)
        {
            bool inputOk = int.TryParse(inputField.text, out value) && value >= 0;
            SetInputFieldColor(inputField, inputOk);
            return inputOk;
        }



        public static bool TryParseFloatPercentageInputField(TMP_InputField inputField, ref float value)
        {
            bool validPercentage = float.TryParse(inputField.text, out value) && IsValidPercentage(value);
            SetInputFieldColor(inputField, validPercentage);
            return validPercentage;
        }







        /// <summary>
        /// Method to check whethe a string is empty or only consists of whitespaces.
        /// </summary>
        /// <param name="simulationName">The simulation name to check</param>
        /// <returns>true if string is not empty and does not only consist of whitespaces , false if string is empty or only consists of whitespaces</returns>
        public static bool BasicInputValidation(string simulationName)
        {

            string trimmedText = string.Concat(simulationName.Where(c => !char.IsWhiteSpace(c)));
            return !trimmedText.Equals("");
        }

        /// <summary>
        /// This method combines SetInputField() and BasicInputValidation-
        /// </summary>
        /// <param name="inputField"></param>
        /// <returns>true if text of inputfield is not empty and does not only consist of whitespaces , false if string is empty or only consists of whitespaces
        /// inputfield will be colored white if true, else it will be colored white</returns>
        public static bool BasicInputFieldValidation(TMP_InputField inputField)
        {
            bool isInputOk = BasicInputValidation(inputField.text);
            SetInputFieldColor(inputField, isInputOk);
            return isInputOk;
        }

        /// <summary>
        /// Method to set the color of an inputfield depending if its content 
        /// can be parsed.
        /// </summary>
        /// <param name="inputField"></param>
        /// <param name="contentIsCorrect"></param>
        private static void SetInputFieldColor(TMP_InputField inputField, bool contentIsCorrect)
        {
            if (!contentIsCorrect)
            {
                inputField.image.color = Color.red;
            }
            else
            {
                inputField.image.color = Color.white;
            }
        }

        /// <summary>
        /// Method to make sure that percentage values are in decimal format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if value is a valid decimal number, else false</returns>
        private static bool IsValidPercentage(float value)
        {
            return value >= 0f && value <= 1f;
        }
    }
}