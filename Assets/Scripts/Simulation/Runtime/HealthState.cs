using UnityEngine;
using Random = UnityEngine.Random;
using static Simulation.Runtime.Person;

namespace Simulation.Runtime
{
    /// <summary>
    /// Class which mainly handles the health logic of person, which is defines
    /// if person must go to a hospital or in intensive care.
    /// </summary>
    public class HealthState
    {
        private Person _person;
        private bool _willRecoverFromCoViDWithoutHospital;
        private bool _willRecoverInHosptal;
        private bool _willGoToIntensiveCare;
        private bool _willDie;
        private Simulation.Edit.AdjustableSimulationSettings _settings = SimulationMaster.Instance.AdjustableSettings;
        public bool WillDieInIntensiveCare { get => _willDie; set => _willDie = value; }

        /// <summary>
        /// The constructor creates a healthState object which determines 
        /// the health/hospital states of a person.
        /// </summary>
        /// <param name="person"></param>
        public HealthState(Person person)
        {
            _person = person;
            Simulation.Edit.AdjustableSimulationSettings settings = SimulationMaster.Instance.AdjustableSettings;

            
            float probabilityToRecover = Random.Range(0f, 1f);
            if (probabilityToRecover <= settings.RecoveringProbability)
            {
                _willRecoverFromCoViDWithoutHospital = true;
               
            }
            else
            {
                _willRecoverFromCoViDWithoutHospital = false;
                float probabilityToRecoverInHospital = Random.Range(0f, 1f);
          

                if (probabilityToRecoverInHospital <= settings.RecoveringInHospitalProbability)
                {
                    _willRecoverInHosptal = true;
                    _willGoToIntensiveCare = false;
                }
                else
                {
                    _willGoToIntensiveCare = true;
                    float probabilityToSurviveIntensiveCare = Random.Range(0f, 1f);
                    if (probabilityToSurviveIntensiveCare <= settings.PersonSurvivesIntensiveCareProbability)
                    {
                        _willRecoverInHosptal = true;
                        _willDie = false;
                    }
                    else
                    {
                        _willRecoverInHosptal = false;
                        _willDie = true;
                        Debug.Log("I will die !");
                    }
                }
            }
        }
        /// <summary>
        /// Method which mainly handles the case a person recovers in hospital or dies anywhere in the world. 
        /// </summary>
        public void UpdateHealthState()
        {

            
            if (_willDie)
            {
                
                if (_person.DaysSinceInfection >= (_settings.IncubationTime + _settings.DaysFromSymptomsBeginToDeath - 1))
                {
                    if (_person.CurrentLocation is Hospital hospital)
                    {
                        hospital.PatientsInRegularBeds.Remove(_person);
                        hospital.PatientsInIntensiveCareBeds.Remove(_person);
                    }
                    _person.CurrentLocation.RemovePerson(_person);
                    //TODO REMOVE PERSON FROM MEMBER OF HOUSEHOLD
                    _person.IsDead = true;
                    SimulationMaster.Instance.OnPersonDies();
                }            
            }

            //Handling case if person is in hospital
            if (_person.IsInHospital)
            {

                /*
                //For simplification, if person survies, just check if person can leave hospital
                if (WillRecoverInHospitalNoIntensiveCare())
                {
                    //TODO USE SETTINGS
                    if (_person.DaysSinceInfection >= DefaultInfectionParameters.HealthPhaseParameters.DayAPersonCanLeaveTheHospital)
                    {
                        Hospital hospital = (Hospital)_person.CurrentLocation;
                        hospital.PatientsInRegularBeds.Remove(_person);
                        _person.IsInHospital = false;
                        _person.HasRegularBed = false;
                        _person.OnStateTransition(InfectionStates.Phase5, _person.InfectionState);
                        _person.InfectionState = Person.InfectionStates.Phase5;
                    }
                }


                if (WillRecoverInHospitalIntensiveCare())
                {
                    if (_person.DaysSinceInfection >= DefaultInfectionParameters.HealthPhaseParameters.DayAPersonCanLeaveTheHospital)
                    {
                        Hospital hospital = (Hospital)_person.CurrentLocation;
                        //Just removing in which bed the patient is, since we us a HashSet we can sum up both cases
                        hospital.PatientsInRegularBeds.Remove(_person);
                        hospital.PatientsInIntensiveCareBeds.Remove(_person);
                        _person.IsInHospital = false;
                        _person.HasRegularBed = false;
                        _person.OnStateTransition(InfectionStates.Phase5, _person.InfectionState);
                        _person.InfectionState = Person.InfectionStates.Phase5;

                    }
                }*/

                //We can sum up both cases above since we use Hashsets
                if (_willRecoverInHosptal)
                {
                

                    if (_person.DaysSinceInfection >= _settings.DayAPersonCanLeaveTheHospital)
                    {
                        Hospital hospital = (Hospital)_person.CurrentLocation;
                        hospital.PatientsInRegularBeds.Remove(_person);
                        hospital.PatientsInIntensiveCareBeds.Remove(_person);
                        _person.IsInHospital = false;
                        _person.HasRegularBed = false;
                        _person.OnStateTransition(InfectionStates.Phase5, _person.InfectionState);
                        _person.InfectionState = InfectionStates.Phase5;

                    }

                }
            }
            
        }





       /// <summary>
       /// Method which checks if a person must go to the hospital,
       /// </summary>
       /// <returns>true if person must be in hospital, else false</returns>

       public bool MustBeInHospital()
        {
            return _person.DaysSinceInfection >= _settings.DayAPersonMustGoToHospital 
                    && _person.DaysSinceInfection < _settings.DayAPersonCanLeaveTheHospital;
        }

        /// <summary>
        /// Methods which checks if a person must be in intensive care
        /// </summary>
        /// <returns>true if person must be in intensive care, else false</returns>
        public bool MustBeInIntensiveCare()
        {
            return _person.DaysSinceInfection >= _settings.DayAPersonMustGoToIntensiveCare
                 && _person.DaysSinceInfection < _settings.DayAPersonCanLeaveIntensiveCare
                 &&_willGoToIntensiveCare;
        }

        /*
        private  bool WillRecoverInHospitalNoIntensiveCare()
        {
            return _willRecoverInHosptal && !_willGoToIntensiveCare;
        
        }

        private  bool WillRecoverInHospitalIntensiveCare()
        {
            return _willRecoverInHosptal && _willGoToIntensiveCare;

        }*/


    }
}
