using System.Collections;
using System.Collections.Generic;
using Simulation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private int _amountInfectious;
        private int _amountInfected;
        private int _amountNotInfected;

        public GameObject CounterGameObject { get; set; }
        public Venue Venue { get; set; }

        // Update is called once per frame
        private void Update()
        {
            //TODO Getting values from Simulation Controller add modify text element of counter

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
        }

        private void UpdateText()
        {
            _counterText.SetText($"<color=green>{_amountNotInfected}</color> / <color=orange>{_amountInfected}</color> / <color=red>{_amountInfectious}</color>");
        }
    }
}