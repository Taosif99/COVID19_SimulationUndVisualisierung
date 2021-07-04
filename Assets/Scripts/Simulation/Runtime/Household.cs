using System;
using UnityEngine;

namespace Simulation.Runtime
{
    class Household : Venue
    {
        public Household(Edit.Household editorEntity) : base(editorEntity)
        {
            int numberOfWorkers = Mathf.CeilToInt(editorEntity.PercentageOfWorkers * editorEntity.NumberOfPeople);

            Members = new Person[editorEntity.NumberOfPeople];

            for (int i = 0; i < editorEntity.NumberOfPeople; i++)
            {
                Person person = new Person(editorEntity.CarefulnessTendency, i < numberOfWorkers);
                person.OnStateTrasitionHandler += SimulationMaster.Instance.AddToGlobalCounter;
                Members[i] = person;
                //At the begnning everyone is at home
                this.MovePersonHere(person);
            }
        }

        public Person[] Members { get; }
    }
}
