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

        public List<Person> TestPopulation = new List<Person>();

        //https://docs.microsoft.com/de-de/dotnet/api/system.datetime?view=net-5.0
        //Tag heute
        public DateTime  currentDay = new DateTime();

        // Start is called before the first frame update
        void Start()
        {

            Debug.Log("Testbegin");

            //init 30 persons
            for (int i = 0; i < 10; i++)
            {

                float randomCarefulnessFactor = UnityEngine.Random.Range(0f, 1f);
                float randomRisk = UnityEngine.Random.Range(0f, 1f);
                Person person = new Person(randomCarefulnessFactor, randomRisk);

                //assign random dateTime since infection
                // eg. days of current mounth


                TestPopulation.Add(person);
            }

            //Todo implement 

            foreach (Person person in TestPopulation)
            {
                person.UpdateInfectionState();
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}