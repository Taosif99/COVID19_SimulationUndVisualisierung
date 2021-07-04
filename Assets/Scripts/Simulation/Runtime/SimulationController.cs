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
        private Simulation.Edit.AdjustableSimulationSettings _settings = SimulationMaster.Instance.AdjustableSettings;
        public DateTime SimulationDate { get; private set; } = new DateTime(2020, 1, 1);

        public void Initialize(Entity[] entities)
        {
            _entities = entities;
            _venues = _entities.OfType<Venue>().ToArray();
            _households = _entities.OfType<Household>().ToArray();
            _hospitals = _entities.OfType<Hospital>().ToArray();
            
            WorkShift[] workShifts = _entities.OfType<Workplace>()
                .SelectMany(w => w.WorkShifts)
                .ToList();

            int workShiftIndex = 0;
            bool workplaceCapacityReached = false;

            foreach (var household in _entities.OfType<Household>())
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
            //Debug.Log(_persons.Count);
        }

        /// <summary>
        /// Method in which our main simulation process logic is implemented.
        /// Here are methods called which deal with:
        ///  - How and how often people encounter
        ///  - Health/Hospital states of each member of a household
        ///  - What a person does if she/he is infectious
        ///  - Moving persons to diffent locations if they have a activity
        ///  - Rapid antigen tests in hospitals
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

                    /*
                    //case to leave quarantine
                    if (member.EndDateOfQuarantine.Equals(SimulationDate))
                    {
                        if (member.InfectionState.HasFlag(Person.InfectionStates.Recovered))
                        {
                            member.IsInQuarantine = false;
                        }
                        else
                        {
                            member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day + _settings.AmountDaysQuarantine);
                            Debug.Log("Extend qu: " + member.EndDateOfQuarantine);
                        }
                    }*/

                    if (MoveInfectiousPersonToHome(member, household)) continue;
                    TryMovePersonToItsActivity(member);
                    //Person Activity & at work => Corona Test
                    /*
                    //Activities of a person
                    if (member.TryGetActivityAt(SimulationDate, out Activity activity) && !activity.Location.HasPersonHere(member))
                    {
                        activity.Location.MovePersonHere(member);
                        //the hospital checks every thursday the personal
                        //The personal has to be checked with symptoms too
                        if ((SimulationDate.DayOfWeek.Equals(DayOfWeek.Thursday) && activity.Location is Simulation.Runtime.Hospital
                            && !member.IsInQuarantine) || member.InfectionState.HasFlag(Person.InfectionStates.Symptoms))
                        {
                            CoronaTest(member, household);
                        }
                    }*/
                    //HERE DO WORK TESTING
                    TryMovePersonBackToHome(member, household);
                }
            }
        }

        private void CoronaTest(Person member, Household household)
        {
            if (member.InfectionState.HasFlag(Person.InfectionStates.Infected))
            {
                Debug.Log("Test positiv");
                member.IsInQuarantine = true;
                member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day + _settings.AmountDaysQuarantine);
                Debug.Log("Person must go home (quarantine): " + SimulationDate + "  ende qu: " + member.EndDateOfQuarantine);
                household.MovePersonHere(member);
                AssignHousholdToQuarantine(household, member.EndDateOfQuarantine);
            }
            else
            {
                Debug.Log("Test negativ");
            }
        }

        private void AssignHousholdToQuarantine(Household household, DateTime endDayOfQuarantine)
        {
            foreach (Person member in household.Members)
            {
                //Check if person is not in quarantine
                if (!member.IsInQuarantine)
                {
                    member.IsInQuarantine = true;
                    member.EndDateOfQuarantine = new DateTime(SimulationDate.Year, SimulationDate.Month, SimulationDate.Day + _settings.AmountDaysQuarantine);
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
                        if (randomPerson.InfectionState.Equals(Simulation.Runtime.Person.InfectionStates.Uninfected))
                        {
                            randomPerson.SetInfected(SimulationDate);
                            infectedPersons++;
                        }
                    } while (infectedPersons < personsToBeInfected);
                    //Debug.Log(infectedPersons + "   " + personsToBeInfected);
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
            Hospital[] hospitals = _entities.OfType<Hospital>().ToArray();

            Venue lastLocation = person.CurrentLocation;


            if (hospitals != null && hospitals.Length > 0)
            {
                int amountHospitals = hospitals.Length;
                Hospital hospital = hospitals[_hospitalRegularBedAssignmentsCounter % amountHospitals];
                if (hospital.PatientsInRegularBeds.Count < hospital.AmountRegularBeds)
                {
                    AssignPersonToRegularBed(person, hospital);
                }
                else
                {
                    //Debug.Log("No more places in this hospital, try next hospitals");
                    for (int i = 0; i < amountHospitals; i++)
                    {
                        if (hospitals[i].PatientsInRegularBeds.Count < hospitals[i].AmountRegularBeds)
                        {
                            AssignPersonToRegularBed(person, hospitals[i]);
                            break;
                        }
                    }
                }

                //At this point we may check if a person is in hospital,if yes the probabilities stay the same, else reduce atleast survive probability
                //or show message that there was no place for a person in the hospital
                if (!person.HasRegularBed)
                {
                    UIController.Instance.NotEnoughBedsMessage.SetActive(true);

                }

                //Handle case if person leaves sensitive bed and gets a normal bed again
                if (person.IsInIntensiveCare && person.HasRegularBed)
                {
                    Hospital oldHospital = (Hospital)lastLocation;
                    oldHospital.PatientsInIntensiveCareBeds.Remove(person);
                    person.IsInIntensiveCare = false;

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
            person.IsInHospital = true;
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
                if (person.CurrentLocation is Hospital hospital)
                {
                    oldHospital = hospital;
                }
            }

            //Assign a free intensive care bed in our simulation world, if this fails we have to assign a regular bed (again)
            if (_hospitals != null && _hospitals.Length > 0)
            {
                int amountHospitals = hospitals.Length;
                //Use round robin first 
                Hospital hospital = hospitals[_hospitalIntensiveCareBedAssignmentsCounter % amountHospitals];
                //Check amount normal beds
                if (hospital.PatientsInIntensiveCareBeds.Count < hospital.AmountIntensiveCareBeds)
                {
                    AssignPersonToIntensiveBed(person, hospital, oldHospital);
                }
                else
                {
                    Debug.Log("No more placees in this hospital, try next hospitals");
                    // if round robin fails, try search for a free hospital place linearly
                    for (int i = 0; i < amountHospitals; i++)
                    {
                        if (hospitals[i].PatientsInIntensiveCareBeds.Count < hospitals[i].AmountIntensiveCareBeds)
                        {
                            AssignPersonToIntensiveBed(person, hospitals[i], oldHospital);
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
                person.HasRegularBed = false;
            }
            hospital.PatientsInIntensiveCareBeds.Add(person);
            hospital.MovePersonHere(person);
            person.IsInHospital = true;
            person.IsInIntensiveCare = true;
            person.HasRegularBed = false;
        }
    }
}