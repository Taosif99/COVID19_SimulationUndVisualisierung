using UnityEngine;
using Simulation.Edit;
using TMPro;
using System.Linq;
using System;

namespace InputValidation
{
    /// <summary>
    /// Class which implements methods concerning input validation in our
    /// simulation program.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        ///  Method which parses al inputfields in the simulation scene and which checks for correct value ranges.
        ///  TODO: This method has way too much parameters !
        /// </summary>
        /// <param name="infectionRisk"></param>
        /// <param name="capacity"></param>
        /// <param name="numberOfPeople"></param>
        /// <param name="carefulness"></param>
        /// <param name="percentageOfWorkers"></param>
        /// <param name="amountBeds"></param>
        /// <param name="amountIntensiveCareBeds"></param>
        /// <param name="currentSelectedEntity"></param>
        /// <returns>true if all requiered inputfields can be parsed to correct values, else false</returns>
        public static bool TryParseLeftInputFields(ref float infectionRisk, ref int capacity, ref byte numberOfPeople, ref float carefulness, ref float percentageOfWorkers, ref int amountBeds,
                ref int amountIntensiveCareBeds, Entity currentSelectedEntity)
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

                    if (currentSelectedEntity is Hospital)
                    {

                        bool amountBedsValid = int.TryParse(UIController.Instance.AmountNormalBedsInputField.text, out amountBeds) && amountBeds >= 0;
                        bool amountIntensiveCareBedsValid = int.TryParse(UIController.Instance.AmountIntensiveCareInputField.text, out amountIntensiveCareBeds) && amountIntensiveCareBeds >= 0;
                        SetInputFieldColor(UIController.Instance.AmountNormalBedsInputField, amountBedsValid);
                        SetInputFieldColor(UIController.Instance.AmountIntensiveCareInputField, amountIntensiveCareBedsValid);
                        inputIsValid = inputIsValid && amountBedsValid && amountIntensiveCareBedsValid;

                    }
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
        public static bool ValidateHealthPhaseParameters(ref float recoveringProbability, ref float recoveringInHospitalProbability, ref float personSurvivesIntensiveCareProbability, ref int daysFromSymptomsBeginToDeath)
        {
            bool result;
            bool recoveringProbabilityInputOk = TryParseFloatPercentageInputField(UIController.Instance.RecoverInputField, ref recoveringProbability);
            bool recoveringProbabilityInHospitalInputOk = TryParseFloatPercentageInputField(UIController.Instance.RecoverInHospitalInputField, ref recoveringInHospitalProbability);
            bool personSurvivesIntensiveCareInputOk = TryParseFloatPercentageInputField(UIController.Instance.SurviveIntensiveCareInputField, ref personSurvivesIntensiveCareProbability);
            bool daysFromSymptomsBeginToDeathInputOk = TryParseIntDayInputField(UIController.Instance.AmountDaysToDeathInputField, ref daysFromSymptomsBeginToDeath);
            result = recoveringProbabilityInputOk && recoveringProbabilityInHospitalInputOk && personSurvivesIntensiveCareInputOk && daysFromSymptomsBeginToDeathInputOk;
            return result;
        }

        /// <summary>
        /// Method which validates the health phase parameters which define the hospitalizatiom process.
        /// </summary>
        /// <param name="daysInHospital"></param>
        /// <param name="durationOfSymptomsbeginToHospitalization"></param>
        /// <param name="daysInIntensiveCare"></param>
        /// <param name="durationOfHospitalizationToIntensiveCare"></param>
        ///<returns>true if all parameters are valid, else false</returns>
        public static bool ValidateHospitalParameters(ref int daysInHospital, ref int durationOfSymptomsbeginToHospitalization, ref int daysInIntensiveCare, ref int durationOfHospitalizationToIntensiveCare)
        {
            bool result = false;
            bool daysInHospitalInputOk = TryParseIntDayInputField(UIController.Instance.DaysInHosputalInputField, ref daysInHospital);
            bool durationOfSymptomsbeginToHospitalizationInputOk = TryParseIntDayInputField(UIController.Instance.DaysSymptomsBeginToHospitalizationInputField, ref durationOfSymptomsbeginToHospitalization);
            bool daysInIntensiveCareInputOk = TryParseIntDayInputField(UIController.Instance.DaysIntensiveCareInputField, ref daysInIntensiveCare);
            bool durationOfHospitalizationToIntensiveCareInputOk = TryParseIntDayInputField(UIController.Instance.DaysRegularToIntensiveInputField, ref durationOfHospitalizationToIntensiveCare);
            result = daysInHospitalInputOk && durationOfSymptomsbeginToHospitalizationInputOk && daysInIntensiveCareInputOk && durationOfHospitalizationToIntensiveCareInputOk;
            return result;
        }

        /// <summary>
        /// Method which validates the quarantine parameters.
        /// </summary>
        /// <param name="quarantineDays"></param>
        /// <param name="advancedQuarantineDays"></param>
        /// <returns>true if all parameters are valid, else false</returns>
        public static bool ValidateQuarantineParameters(ref int quarantineDays, ref int advancedQuarantineDays)
        {
            bool result = false;

            bool quarantineDaysInputOk = TryParseIntDayInputField(UIController.Instance.QuarantineDaysInputField, ref quarantineDays);
            bool advancedQuarantineDaysInputOk = TryParseIntDayInputField(UIController.Instance.AdvancedQuarantineDaysInputField, ref advancedQuarantineDays);

            result = quarantineDaysInputOk && advancedQuarantineDaysInputOk;
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

        public static void ResetAllLeftInputToWhite()
        {

            UIController.Instance.InfectionRiskInputField.image.color = Color.white;
            UIController.Instance.WorkerCapacityInputField.image.color = Color.white;
            UIController.Instance.AmountNormalBedsInputField.image.color = Color.white;
            UIController.Instance.AmountIntensiveCareInputField.image.color = Color.white;
            UIController.Instance.NumberOfPeopleInputField.image.color = Color.white;
            UIController.Instance.CarefulnessInputField.image.color = Color.white;
            UIController.Instance.PercantageOfWorkersInputField.image.color = Color.white;
        }

        /// <summary>
        /// Method to set the color of an inputfield depending if its content 
        /// can be parsed.
        /// </summary>
        /// <param name="inputField"></param>
        /// <param name="contentIsCorrect"></param>
        public static void SetInputFieldColor(TMP_InputField inputField, bool contentIsCorrect)
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