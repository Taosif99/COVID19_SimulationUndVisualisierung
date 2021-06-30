using Simulation.Runtime;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Class which implements a Counter which shows the amount of infected persons and not infected persons at a venue.
    /// </summary>
    class StateCounter : MonoBehaviour
    {
        private float _eulerAngleX = 30f;
        //Reference to the counter which can be modified
        private TextMeshProUGUI _counterText;
        private TextMeshProUGUI _hospitalCounterText;
        private int _amountInfectious;
        private int _amountInfected;
        private int _amountNotInfected;

        public GameObject CounterGameObject { get; set; }

        public GameObject HospitalCounterGameObject { get; set; }

        public Venue Venue { get; set; }



        private void Start()
        {
            StartCoroutine(UpdateRoutine());
        }


        /// <summary>
        /// Method to instinatiate the counter prefab above a venue model after its creation.
        /// </summary>
        /// <param name="VenueSpawnPosition"></param>
        public void InstantiateCounter(float verticalOffset = 4f)
        {
            GameObject counterPrefab = ModelSelector.Instance.CounterPrefab;
            
            CounterGameObject = Instantiate(counterPrefab, gameObject.transform);
            CounterGameObject.transform.localPosition = new Vector3(0, verticalOffset, 0);
            CounterGameObject.transform.rotation = Quaternion.Euler(_eulerAngleX, 0, 0);
            CounterGameObject.name = "CounterCanvas";
            _counterText = CounterGameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (Venue is Hospital)
            {
                HospitalCounterGameObject = Instantiate(counterPrefab, gameObject.transform);
                HospitalCounterGameObject.transform.localPosition = new Vector3(0, verticalOffset * 1.5f, 0);
                HospitalCounterGameObject.transform.rotation = Quaternion.Euler(_eulerAngleX, 0, 0);
                HospitalCounterGameObject.name = "HospitalCounterCanvas";
                _hospitalCounterText = HospitalCounterGameObject.GetComponentInChildren<TextMeshProUGUI>();
                string counterString = $"<color=#6495ED>0/0</color> / <#9FE2BF>0/0</color>";
                _hospitalCounterText.SetText(counterString);
            }

        }

        private void UpdateText()
        {
            _counterText.SetText($"<color=green>{_amountNotInfected}</color> / <color=orange>{_amountInfected}</color> / <color=red>{_amountInfectious}</color>");



            if (Venue is Hospital hospital)
            {
                _hospitalCounterText.SetText($"<color=#6495ED>{hospital.AmountPeopleInRegularBeds}/{hospital.AmountRegularBeds}</color> / <#9FE2BF>{hospital.AmountPeopleInIntensiveBeds}/{hospital.AmountIntensiveCareBeds}</color>");
            }

        }
        /// <summary>
        /// Using an asynchronous coroutine to update the counter values every second.
        /// This shall increase the speed in the program.
        /// </summary>
        
        private IEnumerator UpdateRoutine()
        {

            for (; ; )
            {

                if (!SimulationMaster.Instance.IsForwardingSimulation)
                {

                    _amountInfectious = 0;
                    _amountInfected = 0;
                    _amountNotInfected = 0;

                    foreach (var person in Venue.GetPeopleAtVenue())
                    {
                        if (person.InfectionState.HasFlag(Person.InfectionStates.Infectious))
                        {
                            _amountInfectious++;
                        }
                        else if (person.InfectionState.HasFlag(Person.InfectionStates.Infected))
                        {
                            _amountInfected++;
                        }
                        else
                        {
                            _amountNotInfected++;
                        }

                    }

                    UpdateText();

                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}