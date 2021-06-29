using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpidemiologicalCalculation
{
    /// <summary>
    /// This class performs the epidemiological calculations.
    /// 
    /// 
    /// Used Formulas for the R-Value : <see cref="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Projekte_RKI/R-Wert-Erlaeuterung.pdf?__blob=publicationFile"/> 
    /// </summary>
    public static class EpidemiologicalCalculator
    {
        private const int Tau4 = 4;
        private const int Tau7 = 7;


        /// <summary>
        /// This method calculates the "sensitive R-Value".
        /// </summary>
        /// <param name="currentSimulationDay"></param>
        /// <param name="simulationDays"></param>
        /// <returns> The sensitive 4 days R-Value if it can be calculated, else -1 if cannot be calculated. </returns>
        public static float CalculateRValue(int currentSimulationDay, List<DayInfo> simulationDays)
        {
            float result = -1f;

            if (currentSimulationDay > 7)
            {
                result = CalculateRatio(currentSimulationDay, Tau4, simulationDays);
                if (float.IsInfinity(result) || float.IsNaN(result))
                {
                    //Debug.LogWarning("What the hell---, you destroyed the universe");
                    result = -1f;
                }
            }
            return result;
        }

        /// <summary>
        /// This method calculates the more stable 7-days R-Value according the formula
        /// from the RKI. In difference to the RKI implemntation we do not date the value one day back.
        /// </summary>
        /// <param name="currentSimulationDay"></param>
        /// <param name="simulationDays"></param>
        /// <returns>The stable 7 days R-Value if it can be calculated, else -1 if cannot be calculated.</returns>
        public static float CalculateRValue7(int currentSimulationDay, List<DayInfo> simulationDays)
        {
            float result = -1f;
            if (currentSimulationDay > 10)
            {      
                result = CalculateRatio(currentSimulationDay, Tau7, simulationDays);
                if (float.IsInfinity(result) || float.IsNaN(result))
                {
                    //Debug.LogWarning("What the hell---, you destroyed the universe");
                    result = -1f;
                }
            }
            return result;
        }

        /// <summary>
        /// Method which calculates the incidence as ratio
        /// of the amount of new infections until simualtionDay devided by the riskPopulation
        /// 
        /// 
        /// Definition from:
        /// Einfuehrung in die Epidemiologie 2., vollstaendig ueberarbeitete Auflage 
        /// Aus dem Englischen von Karin Beifuss
        /// ISBN 978-J-456-84535-7
        /// Page 37
        /// 
        /// 
        /// </summary>
        /// <param name="simulationDay"></param>
        /// <param name="simulationDays"></param>
        /// <returns>-1 if riskpopulation is 0, else the incidence</returns>
        public static float CalculateIncidence(int simulationDay, List<DayInfo> simulationDays)
        {
            float amountNewInfectionsInDeltaT = 0f;
            float riskPopulation = SimulationMaster.Instance.AmountUninfected + SimulationMaster.Instance.AmountRecovered;
          
            if (riskPopulation == 0) return -1;
            for (int i = 0; i < simulationDay - 1; i++)
            {
                amountNewInfectionsInDeltaT += simulationDays[i].AmountNewInfections;
            }
            return amountNewInfectionsInDeltaT / riskPopulation;
        }


        /// <summary>
        /// Method which calculates the R-Value depending on the day and tau parameter.
        /// Implements the sum of the RKI formula.
        /// Difference: Indexing is started by 0.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="tau"></param>
        /// <param name="dayInfos"></param>
        /// <returns></returns>
        private static float CalculateRatio(int day, int tau, List<DayInfo> dayInfos)
        {

            int enumerator = 0;
            int denominator = 0;
            for (int s = (day - tau); s < day; s++)
            {
                enumerator += dayInfos[s].AmountNewInfections;
                denominator += dayInfos[s - 4].AmountNewInfections;
            }
            return (float)enumerator / (float)denominator;
        }
    }
}