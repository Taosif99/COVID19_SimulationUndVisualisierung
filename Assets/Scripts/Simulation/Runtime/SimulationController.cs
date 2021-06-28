using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DialogBoxSystem;

namespace Simulation.Runtime
{
    class SimulationController
    {
        private const double SimulationStepsMinutes = 10;
        
        private List<Entity> _entities = new List<Entity>();
        
        public DateTime SimulationDate { get; private set; } = new DateTime(2020, 1, 1);

        public void Initialize(List<Entity> entities)
        {
            _entities = entities;
            
            List<WorkShift> workShifts = _entities.OfType<Workplace>()
                .SelectMany(w => w.WorkShifts)
                .ToList();

            int workShiftIndex = 0;

            foreach (var household in _entities.OfType<Household>())
            {
                foreach (Person member in household.Members)
                {
                    if (!member.IsWorker)
                    {
                        continue;
                    }

                    WorkShift shift = workShifts[workShiftIndex];

                    member.Activities.Add(new Activity(
                        shift.Days,
                        shift.StartTime,
                        shift.StartTime + shift.Duration,
                        shift.Workplace
                    ));

                    workShiftIndex = (workShiftIndex + 1) % workShifts.Count;
                }

                var editorHousehold = household.GetEditorEntity<Edit.Household>();
                for (int i = 0; i < editorHousehold.NumberOfShoppingRuns; i++)
                {
                    int numberOfShoppers = Random.Range(1, editorHousehold.NumberOfShoppers + 1);

                    do
                    {
                        // TODO: Use ActivityScheduler to schedule shopping runs
                    } while (false);
                }
            }
        }

        public void RunUpdate()
        {
            SimulationDate = SimulationDate.AddMinutes(SimulationStepsMinutes);

            foreach (var venue in _entities.OfType<Venue>())
            {
                venue.SimulateEncounters(SimulationDate);

                if (!(venue is Household household))
                {
                    continue;
                }

                foreach (Person member in household.Members)
                {
                    if (member.IsDead) continue;

                    member.UpdateInfectionState(SimulationDate);
                    member.UpdateHealthState();

                    if (member.InfectionState.HasFlag(Person.InfectionStates.Symptoms))
                    {
                        if (!household.HasPersonHere(member))
                        {
                            household.MovePersonHere(member);  
                        }
                        
                        continue;
                    }
                    
                    if (member.TryGetActivityAt(SimulationDate, out Activity activity) && !activity.Location.HasPersonHere(member))
                    {
                        activity.Location.MovePersonHere(member);
                    }

                    if (!household.HasPersonHere(member) && !member.HasActivityAt(SimulationDate))
                    {
                        household.MovePersonHere(member);
                    }
                }
            }
        }

        public void InfectRandomPerson()
        {
            Household[] households = _entities.OfType<Household>().ToArray();
            if (households != null && households.Length > 0)
            {
                Household randomHousehold = households[Random.Range(0, households.Length)];
                Person randomPerson = randomHousehold.Members[Random.Range(0, randomHousehold.Members.Length)];
                randomPerson.SetInfected(SimulationDate);
            }
            else 
            { 
                Debug.Log("No households");
                string msg = "Atleast one household with one person is requiered to infect one person!";
                string name = "No households";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.HasCancelButon = false;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            }
        }
    }
}
