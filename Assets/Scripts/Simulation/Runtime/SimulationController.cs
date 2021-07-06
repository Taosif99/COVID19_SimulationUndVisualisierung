using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Random = UnityEngine.Random;
using DialogBoxSystem;
using Debug = UnityEngine.Debug;

namespace Simulation.Runtime
{
    class SimulationController
    {
        /// <summary>
        /// Counts how many times we tried to assign a person to a regular / intensive care bed
        /// </summary>
        private int _hospitalRegularBedAssignmentsCounter;
        private int _hospitalIntensiveCareBedAssignmentsCounter;

        private const double SimulationStepsMinutes = 10;

        private Entity[] _entities;
        private Venue[] _venues;
        private Household[] _households;
        private Hospital[] _hospitals;
        private List<Person> _persons = new List<Person>();
        private Edit.AdjustableSimulationSettings _settings = SimulationMaster.Instance.AdjustableSettings;
        public DateTime SimulationDate { get; private set; } = new DateTime(2020, 1, 1);

        public void Initialize(Entity[] entities)
        {
            _entities = entities;
            _venues = _entities.OfType<Venue>().ToArray();
            _households = _entities.OfType<Household>().ToArray();
            _hospitals = _entities.OfType<Hospital>().ToArray();

            WorkShift[] workShifts = _entities.OfType<Workplace>()
                .SelectMany(w => w.WorkShifts)
                .ToArray();

            int workShiftIndex = 0;
            bool workplaceCapacityReached = false;

            foreach (var household in _households)
            {
                if (!workplaceCapacityReached)
                {
                    try
                    {
                        foreach (Person member in household.Members)
                        {
                            _persons.Add(member);

                            if (!member.IsWorker)
                            {
                                continue;
                            }

                            int checkedShifts = 0;
                            WorkShift shift;
                            do
                            {
                                shift = workShifts[workShiftIndex++ % workShifts.Length];

                                if (++checkedShifts > workShifts.Length)
                                {
                                    workplaceCapacityReached = true;
                                    throw new Exception("Not enough work-shift/workplace capacity for all people in the simulation.");
                                }
                            }
                            while (shift.Workplace.AmountAssignedWorkers >= shift.Workplace.WorkerCapacity);

                            member.Activities.Add(new Activity(
                                shift.Days,
                                shift.StartTime,
                                shift.StartTime + shift.Duration,
                                shift.Workplace,
                                true
                            ));

                            shift.Workplace.AmountAssignedWorkers++;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                    }
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

        /// <summary> TODO ADD DESCRIPTION FOR TESTING
        /// Method in which our main simulation process logic is implemented.
        /// Here are methods called which deal with:
        ///  - How and how often people encounter
        ///  - Health/Hospital states of each member of a household
        ///  - What a person does if she/he is infectious
        ///  - Moving persons to diffent locations if they have a activity
        ///  - Performing corona tests
        /// </summary>
        public void RunUpdate()
        {
            SimulationDate = SimulationDate.AddMinutes(SimulationStepsMinutes);

            foreach (Venue venue in _venues)
            {
                venue.SimulateEncounters(SimulationDate);

                if (!(venue is Household household))
                {
                    continue;
                }

                foreach (Person member in household.Members)
                {
                    if (member.IsDead)
                    {
                        continue;
                    }

                    member.UpdateInfectionState(SimulationDate);
                    member.UpdateHealthState();

                    // TODO: Having this twice is kinda bad, but this is needed as the person might have deceased during this health state update
                    if (member.IsDead)
                    {
                        continue;
                    }

                    if (HandleHospitalLogic(member)) continue;

                    //TODO METHOD HANDLE QUARANTINE Method
                    if (member.EndDateOfQuarantine.Equals(SimulationDate))
                    {
                        if (!CanLeaveQuarantine(member)) continue; 
                    }

                    if (member.IsInQuarantine) continue;

                    if (MoveInfectiousPersonToHome(member, household))
                    {
                        // TODO makes the simulation too radical in terms of quarantine (it works too good)
                        //AssignHouseholdToQuarantine(household);
                        continue; 
                    }

                    TryMovePersonToItsActivity(member);

                    TryToDoCoronaTest(member, household);

                    if (member.IsInQuarantine) continue;
   
                    TryMovePersonBackToHome(member, household);
                }
            }
        }

        /// <summary>
        /// Method to check if the person can leave the quarantine.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>true if member can leave quarantine, else false and further logic can be skipped</returns>
        private bool CanLeaveQuarantine(Person member)
        {
            if (member.InfectionState.HasFlag(Person.InfectionStates.Recovered))
            {
                if (IsCoronaQuickTestCorrect(false))
                {
                    LeaveQuaratine(member);
                    return true;
                }
                else
                {
                    ExtendQuarantine(member);
                    return false;
                }
            }

            else
            {
                if (IsCoronaQuickTestCorrect(true))
                {
                    ExtendQuarantine(member);
                    return false;
                }
                else
                {
                    LeaveQuaratine(member);
                    return true;
                }
            }    
        }

        private void ExtendQuarantine(Person member)
        {
            member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day).AddDays(_settings.AdvancedQuarantineDays);
            Debug.Log("Extend qu: " + member.EndDateOfQuarantine);
        }

        private void LeaveQuaratine(Person member)
        {
            member.IsInQuarantine = false;
            Debug.Log("Leave qu: " + member.EndDateOfQuarantine);
        }

        /// <summary>
        /// Try to do corona test if the conditions are met.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="household"></param>
        private void TryToDoCoronaTest(Person member, Household household)
        {
            if (!member.IsInQuarantine && member.TryGetActivityAt(SimulationDate, out Activity activity) && activity.IsWork
                        && SimulationDate.Equals(new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day, activity.StartTime, 0, 0)))
            {
                Workplace workplace = activity.Location as Workplace;

                if(workplace == null || !workplace.CoronaTestsEnabled)
                {
                    return;
                }

                if (activity.Location is Hospital && SimulationDate.DayOfWeek.Equals(DayOfWeek.Thursday))
                {
                    Debug.Log("Hospital");
                    CoronaTest(member, household);
                }

                else if (!(activity.Location is Hospital) && 
                    (SimulationDate.DayOfWeek.Equals(DayOfWeek.Monday) || 
                    SimulationDate.DayOfWeek.Equals(DayOfWeek.Wednesday)))
                {
                    Debug.Log("Workplace");
                    CoronaTest(member, household);
                }
            }
        }

        /// <summary>
        /// Method to do a corona test.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="household"></param>
        private void CoronaTest(Person member, Household household)
        {
            if (member.InfectionState.HasFlag(Person.InfectionStates.Infected))
            {
                if (IsCoronaQuickTestCorrect(true))
                {
                    Debug.Log("Test positiv");
                    member.IsInQuarantine = true;
                    member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day).AddDays(_settings.AmountDaysQuarantine);
                    Debug.Log("Person must go home (quarantine): " + SimulationDate + "  ende qu: " + member.EndDateOfQuarantine);
                    household.MovePersonHere(member);
                    AssignHouseholdToQuarantine(household);
                }
                else
                    return;
            }
            else
            {
                if (!IsCoronaQuickTestCorrect(false))
                {
                    Debug.Log("Test negativ");
                    member.IsInQuarantine = true;
                    member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day).AddDays(_settings.AmountDaysQuarantine);
                    Debug.Log("Person must go home (quarantine): " + SimulationDate + "  ende qu: " + member.EndDateOfQuarantine);
                    household.MovePersonHere(member);
                    AssignHouseholdToQuarantine(household);
                }
                else
                    return;
            }
        }
        /// <summary>
        /// Method to determine that a corona test is correct.
        /// <see cref="https://www.fuldaerzeitung.de/fulda/corona-schnelltest-ergebnis-positiv-negativ-falsch-pcr-test-rki-christian-drosten-fulda-90661119.html"/>
        /// </summary>
        /// <param name="isPersonInfected"></param>
        /// <returns>true if corona test is correct, else false</returns>
        private bool IsCoronaQuickTestCorrect(bool isPersonInfected)
        {
            //float percentageTestIsFalseNegative = 0.45f;
            //float percentageTestIsFalsePositiv = 0.0022f;

            int testAccuracy;

            if (isPersonInfected)
            {
                testAccuracy = Random.Range(1, 101);
                Debug.Log(testAccuracy);
                if (testAccuracy <= 45)
                {
                    Debug.Log("Test is false negative");
                    return false;
                }
                else
                    return true;
            }
            else
            {
                testAccuracy = Random.Range(1, 10001);
                Debug.Log(testAccuracy);
                if (testAccuracy <= 22)
                {
                    Debug.Log("Test is false positve");
                    return false;
                }
                else
                    return true;
            }
        }

        /// <summary>
        /// Method to assign a household to quarantine if a member tests positive.
        /// </summary>
        /// <param name="household"></param>
        private void AssignHouseholdToQuarantine(Household household)
        {
            foreach (Person member in household.Members)
            {
                if (!member.IsInQuarantine)
                {
                    member.IsInQuarantine = true;
                    member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day).AddDays(_settings.AmountDaysQuarantine);
                    Debug.Log("Person must go home (quarantine): " + SimulationDate + "  ende qu: " + member.EndDateOfQuarantine);
                    if (!household.HasPersonHere(member))
                    {
                        household.MovePersonHere(member);
                    }
                }
            }
        }

        public void InfectRandomPerson(int personsToBeInfected)
        {
            if (_persons.Count > 0)
            {
                //All persons get infected
                if (_persons.Count <= personsToBeInfected)
                {
                    foreach (Person member in _persons)
                    {
                        member.SetInfected(SimulationDate);
                    }
                }
                else
                {
                    int infectedPersons = 0;
                    do
                    {
                        Person randomPerson = _persons[Random.Range(0, _persons.Count)];
                        if (randomPerson.InfectionState.Equals(Person.InfectionStates.Uninfected))
                        {
                            randomPerson.SetInfected(SimulationDate);
                            infectedPersons++;
                        }
                    } while (infectedPersons < personsToBeInfected);
                }
            }
            else
            {
                Debug.Log("No persons");
                string msg = "Atleast one household with one person is requiered to infect one person!";
                string name = "No persons";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.HasCancelButton = false;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            }
        }

        /// <summary>
        /// Method which encapsulates the simulation logic for hispitalization.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>true if member is in Hospital and further logic can be skipped, else false</returns>
        private bool HandleHospitalLogic(Person member)
        {
            if (member.MustBeTransferredToHospital())
            {
                TryAssignPersonToRegularBed(member);
            }

            //Know checking if person must go to intensive care
            //code is in this place because user can configure the world so that there are more intensive beds 
            //even though this would not be realistic.
            if (member.MustBeTransferredToIntensiveCare())
            {
                TryAssignPersonToIntensiveBed(member);
            }
            if (member.IsInHospitalization)
            {
                //Check if member can leave intensive care and go to a normal bed
                if (member.CanLeaveIntensiveCare())
                {
                    TryAssignPersonToRegularBed(member);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method encapsulates the simulation logic for moving a person to its home
        /// if the person has symptoms.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="household"></param>
        /// <returns>true if further simulation logic can be skipped since person is at home, else false</returns>
        private bool MoveInfectiousPersonToHome(Person member, Household household)
        {
            if (member.InfectionState.HasFlag(Person.InfectionStates.Symptoms))
            {
                if (!household.HasPersonHere(member))
                {
                    household.MovePersonHere(member);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves person to activity if it exists and person not already there
        /// </summary>
        private void TryMovePersonToItsActivity(Person member)
        {
            if (member.TryGetActivityAt(SimulationDate, out Activity activity) && !activity.Location.HasPersonHere(member))
            {
                //Debug.Log(member.TryGetActivityAt(SimulationDate, out activity) + "  " + activity.Location.ToString());
                activity.Location.MovePersonHere(member);
            }
        }

        /// <summary>
        /// Moves person back to household if person has no activities to fulfill.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="household"></param>
        private void TryMovePersonBackToHome(Person member, Household household)
        {

            if (!household.HasPersonHere(member) && !member.HasActivityAt(SimulationDate))
            {
                household.MovePersonHere(member);
            }
        }

        /// <summary>
        /// Method which tries to assign a person to a regular bed.
        /// Firstly round robin is used, if round robin fails a linear search
        /// is done to search a free regular bed. If person leaves intensive car this method also seareches a 
        /// free regular bed for the person and removes the person from the location with the intensive care bed.
        /// </summary>
        /// <param name="person"></param>
        private void TryAssignPersonToRegularBed(Person person)
        {
            Venue lastLocation = person.CurrentLocation;
            if (_hospitals != null && _hospitals.Length > 0)
            {
                int amountHospitals = _hospitals.Length;
                Hospital hospital = _hospitals[_hospitalRegularBedAssignmentsCounter % amountHospitals];
                if (hospital.PatientsInRegularBeds.Count < hospital.AmountRegularBeds)
                {
                    AssignPersonToRegularBed(person, hospital);
                }
                else
                {
                    for (int i = 0; i < amountHospitals; i++)
                    {
                        if (_hospitals[i].PatientsInRegularBeds.Count < _hospitals[i].AmountRegularBeds)
                        {
                            AssignPersonToRegularBed(person, _hospitals[i]);
                            break;
                        }
                    }
                }
                //Handle case if person leaves sensitive bed and gets a normal bed again
                if (person.IsInIntensiveCare && person.HasRegularBed)
                {
                    Hospital oldHospital = (Hospital)lastLocation;
                    oldHospital.PatientsInIntensiveCareBeds.Remove(person);
                    person.IsInIntensiveCare = false;
                }
                _hospitalRegularBedAssignmentsCounter++;
            }

            //At this point we may check if a person is in hospital,if yes the probabilities stay the same, else reduce atleast survive probability
            //or show message that there was no place for a person in the hospital
            if (!person.HasRegularBed)
            {
                UIController.Instance.NotEnoughBedsMessage.SetActive(true);
            }
        }

        private void DebugHospitalPatients(Hospital hospital)
        {
            Debug.Log("Amount patients in regular beds:" + hospital.PatientsInRegularBeds.Count);
            Debug.Log("Amount patients in intensive beds:" + hospital.PatientsInIntensiveCareBeds.Count);
        }

        private void AssignPersonToRegularBed(Person person, Hospital hospital)
        {
            hospital.PatientsInRegularBeds.Add(person);
            hospital.MovePersonHere(person);
            person.IsInHospitalization = true;
            person.HasRegularBed = true;
        }

        /// Method which tries to assign an intensive care bed to a person.
        /// Firstly round robin is used, if round robin fails a linear search,
        /// if linear search also failed the person still maintains its
        /// previous regular bed. If assignment is successful the regular bed
        ///  will no longer be used.
        /// 
        /// <param name="person"></param>
        private void TryAssignPersonToIntensiveBed(Person person)
        {
            //Save the old bed temporary if it changes it will be deleted
            Hospital oldHospital = null;
            if (person.HasRegularBed)
            {
                oldHospital = (Hospital)person.CurrentLocation;
            }

            //Assign a free intensive care bed in our simulation world, if this fails we have to assign a regular bed (again)
            if (_hospitals != null && _hospitals.Length > 0)
            {
                int amountHospitals = _hospitals.Length;
                //Use round robin first 
                Hospital hospital = _hospitals[_hospitalIntensiveCareBedAssignmentsCounter % amountHospitals];
                //Check amount normal beds
                if (hospital.PatientsInIntensiveCareBeds.Count < hospital.AmountIntensiveCareBeds)
                {
                    AssignPersonToIntensiveBed(person, hospital, oldHospital);
                }
                else
                {
                    //Debug.Log("No more placees in this hospital, try next hospitals");
                    // if round robin fails, try search for a free hospital place linearly
                    for (int i = 0; i < amountHospitals; i++)
                    {
                        if (_hospitals[i].PatientsInIntensiveCareBeds.Count < _hospitals[i].AmountIntensiveCareBeds)
                        {
                            AssignPersonToIntensiveBed(person, _hospitals[i], oldHospital);
                            break;
                        }
                    }
                }

                _hospitalRegularBedAssignmentsCounter++;
            }

            //At this point we may check if a person is in hospital,if yes the probabilities stay the same, else reduce survive probability
            //or show message that there was no place for a person in the hospital
            if (!person.IsInIntensiveCare)
            {
                UIController.Instance.NotEnoughIntensiveBedsMessage.SetActive(true);
            }
        }

        private void AssignPersonToIntensiveBed(Person person, Hospital hospital, Hospital oldHospital)
        {
            //Remove person from old Hospital normal bed
            if (oldHospital != null)
            {
                oldHospital.PatientsInRegularBeds.Remove(person);

            }

            hospital.PatientsInIntensiveCareBeds.Add(person);
            hospital.MovePersonHere(person);
            person.IsInHospitalization = true;
            person.IsInIntensiveCare = true;
            person.HasRegularBed = false;
        }
    }
}