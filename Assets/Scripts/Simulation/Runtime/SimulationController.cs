using System;
using System.Collections.Generic;
using System.Linq;
using EditorObjects;
using RuntimeObjects;
using Utility;
using Random = UnityEngine.Random;

namespace Simulation.Runtime
{
    class SimulationController
    {
        private const double SimulationTimeStepInMinutes = 10;

        private List<Entity> _entities = new List<Entity>();
        private DateTime _simulationDate = new DateTime(2020, 1, 1);

        public void Initialize(List<IEditorObject> editorObjects)
        {
            foreach (IEditorObject editorObject in editorObjects)
            {
                Entity entity = RuntimeObjectFactory.Create(editorObject);
                _entities.Add(entity);
            }

            foreach (var household in _entities.OfType<Household>())
            {
                foreach (Person member in household.Members)
                {
                    if (!member.IsWorker)
                    {
                        continue;
                    }

                    // TODO: Assign workplace, round-robin, bla bla
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
            _simulationDate = _simulationDate.AddMinutes(SimulationTimeStepInMinutes);

            foreach (var venue in _entities.OfType<Venue>())
            {
                venue.SimulateEncounters();

                if (venue is not Household household)
                {
                    continue;
                }

                foreach (Person member in household.Members)
                {
                    member.UpdateInfectionState();

                    if (member.TryGetActivityAt(_simulationDate, out Activity activity) && !activity.Location.HasPersonHere(member))
                    {
                        member.CurrentLocation = activity.Location;
                    }

                    if (!household.HasPersonHere(member) && !member.HasActivityAt(_simulationDate))
                    {
                        member.CurrentLocation = household;
                    }
                }
            }
        }
    }
}
