using System.Collections.Generic;
using System;
using UnityEngine;

namespace EpidemiologicalCalculation
{
    /// <summary>
    /// This class manages the DayInfo datasets of the simulation.
    /// </summary>
    public class DayInfoHandler
    {
        /// <summary>
        /// This list manages the days of the simulation.
        /// The first day has index 0, second 1,...
        /// </summary>
        private List<DayInfo> _simulationDays;

        public DayInfoHandler()
        {
            _simulationDays = new List<DayInfo>();
        }

     


        //Must be called each day
        
        /// <summary>
        /// Method which adds a dayinfo dataset.
        /// The time information is removed
        /// </summary>
        /// <param name="date"></param>
        public void AddDayInfo(DateTime date)
        {
            date = date.Date;
            _simulationDays.Add(new DayInfo(date));
        }

        /// <summary>
        /// Method which adds 1 to the amounts of new infections in the current day.
        /// </summary>
        /// <param name="currentDayOfSimulation">The current day of the simulation, e.g. First day 1, second 2 ...</param>
        public void AddNewInfectionToCurrentDate(int currentDayOfSimulation)
        {
            int dayIndex = currentDayOfSimulation - 1;
            DayInfo dayInfo = _simulationDays[dayIndex];
            dayInfo.AmountNewInfections += 1;
        
        }

        /// <summary>
        /// Method which updates the R-Value of a day.
        /// </summary>
        /// <param name="currentSimulationDay"></param>
        /// <returns>The calculated R-Value</returns>
        public float UpdateRValue(int currentSimulationDay)
        {
            float RValue = EpidemiologicalCalculator.CalculateR7Value(currentSimulationDay, _simulationDays);
            _simulationDays[currentSimulationDay - 1].RValue = RValue;
            //Debug.Log("Calculated R-Value: (-1 is undefined): " + _simulationDays[currentSimulationDay - 1].RValue);
            return RValue;
        
        }





        private void DebugViewDataSets() 
        {
            Debug.Log(SimulationMaster.Instance.CurrentDayOfSimulation + "#######################################################################") ;
            for (int i = 0; i < SimulationMaster.Instance.CurrentDayOfSimulation - 1; i++) 
            {
                Debug.Log(_simulationDays[i]);
            }


        }

 

    }
}