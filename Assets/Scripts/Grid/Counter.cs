using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Grid
{
    /// <summary>
    /// Class which implements a Counter which shows the amount of infected persons
    /// </summary>
    public class Counter : MonoBehaviour
    {




        public float Height { get; set; }

        //Euler X-Angle rotation
        public float XAngle { get; set; }


        private GameObject _counterGameObject;
        // Start is called before the first frame update
        void Start()
        {



            //Setting up default values
            Height = 4f;
            XAngle = 30f;

        }


        // Update is called once per frame
        void Update()
        {
            //TODO Getting values from Simulation Controller add modify text element of counter
        }


        public void InstantiateCounter()
        {
            GameObject counterPrefab = ModelSelector.Instance.CounterPrefab;
            _counterGameObject = Instantiate(counterPrefab, new Vector3(transform.position.x, Height, transform.position.y), Quaternion.Euler(30, 0, 0));
            _counterGameObject.name = "Counter";

            Debug.Log("Spawned in: " + _counterGameObject.transform.position);
        }


    }
}