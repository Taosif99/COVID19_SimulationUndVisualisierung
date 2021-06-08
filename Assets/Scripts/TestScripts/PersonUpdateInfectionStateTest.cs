using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;
using System;

namespace TestScripts
{

    //https://docs.microsoft.com/de-de/dotnet/api/system.datetime?view=net-5.0
    public class PersonUpdateInfectionStateTest : MonoBehaviour
    {

        private List<Person> _testPopulation = new List<Person>();

        DateTime today = DateTime.Today;
        // Start is called before the first frame update
        void Start()
        {

            /*
            int currentMonth = UnityEngine.Random.Range(1, 12);
            currentDate = currentDate.AddDays(currentMonth);

            int currentDay = UnityEngine.Random.Range(1, 30);
            currentDate = currentDate.AddMonths(currentDay);
            */
            /*
            Debug.Log(today);
            Debug.Log("Current Day:" + today);
            //init 30 persons
            
            for (int i = 0; i < 5; i++)
            {

                float randomCarefulnessFactor = UnityEngine.Random.Range(0f, 1f);
                float randomRisk = UnityEngine.Random.Range(0f, 1f);
                Person person = new Person(randomCarefulnessFactor, randomRisk, true);

                person._infectionDate = new DateTime(2021, UnityEngine.Random.Range(5,7), UnityEngine.Random.Range(1, today.Day));
                Debug.Log("Infections date: " + person._infectionDate);
                TestPopulation.Add(person);

            }

            //Todo implement 
            
            foreach (Person person in TestPopulation)
            {
                person._infectionStates = Person.InfectionStates.Infected | Person.InfectionStates.Infectious;
                person.UpdateInfectionState(today);    
            }
            */
   



        }

        // Update is called once per frame
       
    }
}