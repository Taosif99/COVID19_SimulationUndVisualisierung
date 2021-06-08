using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    class SimulationController
    {
        private const double SimulationTimeStepInMinutes = 10;

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
            SimulationDate = SimulationDate.AddMinutes(SimulationTimeStepInMinutes);

            foreach (var venue in _entities.OfType<Venue>())
            {
                // TODO: venue.SimulateEncounters();

                if (!(venue is Household household))
                {
                    continue;
                }

                foreach (Person member in household.Members)
                {
                    member.UpdateInfectionState(SimulationDate);
                    // TODO: member.UpdateHealthState(SimulationDate);

                    if (member.TryGetActivityAt(SimulationDate, out Activity activity) && !activity.Location.HasPersonHere(member))
                    {
                        member.CurrentLocation = activity.Location;
                    }

                    if (!household.HasPersonHere(member) && !member.HasActivityAt(SimulationDate))
                    {
                        member.CurrentLocation = household;
                    }
                }
            }
        }
    }
}
