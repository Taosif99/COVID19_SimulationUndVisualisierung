using FileHandling;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EpidemiologicalCalculation
{
    /// <summary>
    /// This class manages the DayInfo datasets of the simulation.
    /// </summary>
    public class DayInfoHandler
    {
       private StringBuilder _csvStringBuilder;

        /// <summary>
        /// This list manages the days of the simulation.
        /// The first day has index 0, second 1,...
        /// </summary>
        private List<DayInfo> _simulationDays;

        public DayInfoHandler()
        {
            _simulationDays = new List<DayInfo>();
            _csvStringBuilder = new StringBuilder();
            _csvStringBuilder.AppendLine("Day;AmountNewInfections;R-Value;R-Value7;Incidence");
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
        /// 
        /// </summary>
        /// <param name="currentDayOfSimulation">The current day of the simulation, e.g. First day 1, second 2 ...</param>
        public void AddNewInfectionToCurrentDate(int currentDayOfSimulation)
        {
            int dayIndex = currentDayOfSimulation - 1;
            DayInfo dayInfo = _simulationDays[dayIndex];
            dayInfo.AmountNewInfections += 1;
        
        }

        //TODO UPDATE COMMENT
        /// <summary>
        /// Method which updates the R-Value of a day.
        /// </summary>
        /// <param name="currentSimulationDay">The current day since the start of the simulation, day = 1, day2 = 2,...</param>
        /// <param name="playDate"> The real world date the simulation started.</param>
        /// <returns>The calculated R-Value</returns>
        public void UpdateRValueAndIncidence(int currentSimulationDay, out float rValue, out float rValue7 ,out float incidence, DateTime playDate)
        {
            rValue = EpidemiologicalCalculator.CalculateRValue(currentSimulationDay, _simulationDays);
            _simulationDays[currentSimulationDay - 1].RValue = rValue;
            //Debug.Log("Calculated R-Value: (-1 is undefined): " + _simulationDays[currentSimulationDay - 1].RValue);
           // rValue7 = EpidemiologicalCalculator.CalculateRValue7(currentSimulationDay, _simulationDays);
            rValue7 = -1f; //bug
            _simulationDays[currentSimulationDay - 1].RValue7 = rValue7;


            incidence = EpidemiologicalCalculator.CalculateIncidence(currentSimulationDay,_simulationDays) ;
            _simulationDays[currentSimulationDay - 1].Incidence = incidence;

            //TODO Better write once all dayinfos at the end of the simulation
            if (UIController.Instance.CsvLogToggle.isOn) 
            {
                string newDataRow = _simulationDays[currentSimulationDay - 1].ToString();
                _csvStringBuilder.AppendLine(newDataRow);

                FileHandler.WriteToCsv(_csvStringBuilder.ToString(), playDate);

            }
                
            
        
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