﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DialogBoxSystem;

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

        /// <summary>
        /// Method in which our main simulation process logic is implemented.
        /// Here are methods called which deal with:
        ///  - How and how often people encounter
        ///  - Health/Hospital states of each member of a household
        ///  - What a person does if she/he is infectious
        ///  - Moving persons to diffent locations if they have a activity
        /// </summary>
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

                    //Hospital logic
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

                    if (member.IsInHospital)
                    {
                        //Check if member can leave intensive care and go to a normal bed
                        if (member.CanLeaveIntensiveCare())
                        {
                            TryAssignPersonToRegularBed(member);

                        }
                        continue;
                    }

                    //case to leave quarantine
                    if (member.EndDateOfQuarantine.Equals(SimulationDate))
                    {
                        Debug.Log(member.EndDateOfQuarantine + "     " + SimulationDate.Date);
                        if (member.InfectionState.HasFlag(Person.InfectionStates.Recovered))
                        {
                            member.IsInQuarantine = false;
                        }
                        else
                        {
                            member.EndDateOfQuarantine = SimulationDate.AddDays(7);
                            Debug.Log("Extend qu");
                            //member.EndDateofQuarantine must be extended
                        }
                            
                    }

                    //Infectious persons stay at home
                    if (member.InfectionState.HasFlag(Person.InfectionStates.Symptoms) || member.IsInQuarantine)
                    {
                        if (!household.HasPersonHere(member))
                        {
                            household.MovePersonHere(member);
                        }
                        continue;
                    }

                    //Activities of a person
                    if (member.TryGetActivityAt(SimulationDate, out Activity activity) && !activity.Location.HasPersonHere(member))
                    {
                        activity.Location.MovePersonHere(member);
                        //the hospital checks very thursday the personal
                        //The personal has to be checked with symptoms too
                        if ((activity.Location.ToString().Equals("Simulation.Runtime.Hospital") && !member.IsInQuarantine 
                            && SimulationDate.DayOfWeek.Equals(DayOfWeek.Thursday)) || member.InfectionState.HasFlag(Person.InfectionStates.Symptoms))
                        {   
                                if (member.InfectionState.HasFlag(Person.InfectionStates.Infected))
                                {
                                    member.IsInQuarantine = true;
                                    member.EndDateOfQuarantine = SimulationDate.AddDays(14);
                                    Debug.Log("Person must go home (quarantine): " + SimulationDate + "  ende qu: " + member.EndDateOfQuarantine);
                                    household.MovePersonHere(member);
                                    AssignHousholdToQuarantine(household, member.EndDateOfQuarantine);
                                }
                        }
                    }

                    if (!household.HasPersonHere(member) && !member.HasActivityAt(SimulationDate))
                    {
                        household.MovePersonHere(member);
                    }
                }
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
                    member.EndDateOfQuarantine = SimulationDate.AddDays(14);
                    Debug.Log("Person must go home (quarantine): " + SimulationDate + "  ende qu: " + member.EndDateOfQuarantine);
                    if (!household.HasPersonHere(member))
                    {
                        household.MovePersonHere(member);
                    }
                }
            }
        }

        public void InfectRandomPerson()
        {
            //1. Neues Feld machen
            //1.1 if Bedingung umändern, sodass household.length > Anzahl der Personen im Haushalt & Personen im Haushalt > Anzahl der Personen im Haushalt
            //2. Zahl aus dem Feld entnehemn. --> Es muss gecheckt werden, dass da auch eine Zahl ist
            //3. Schleife machen
            //4. Gucken, dass die Person auch nicht infiziert ist
            //5. Nach drücken von Virus Inputfield unsichtbar machen
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
                dialogBox.HasCancelButton = false;
                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
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


                _hospitalRegularBedAssignmentsCounter++;
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
            DebugHospitalPatients(hospital);
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
            Hospital[] hospitals = _entities.OfType<Hospital>().ToArray();

            if (hospitals != null && hospitals.Length > 0)
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

                //At this point we may check if a person is in hospital,if yes the probabilities stay the same, else reduce survive probability
                //or show message that there was no place for a person in the hospital
                if (!person.IsInIntensiveCare)
                {
                    UIController.Instance.NotEnoughIntensiveBedsMessage.SetActive(true);

                }
                _hospitalRegularBedAssignmentsCounter++;
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
            //person.HasIntensiveCareBed = true;
            DebugHospitalPatients(hospital);
        }

    }
}