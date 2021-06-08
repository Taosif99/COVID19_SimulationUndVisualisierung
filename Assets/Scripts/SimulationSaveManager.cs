using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class which handles loading and saving of simulation prefabs/configurations.
/// </summary>
public class SimulationSaveManager : MonoBehaviour
{



    //Variables for simulation Loading

    [SerializeField] private GameObject _VerticalButtonGroupParent;
    [SerializeField] private GameObject _simulationButtonsPrefab;



    private void Awake()
    {
        //Making sure that game object is not destroyed if scene is changed
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void Test()
    {
        string mockName = "simulation";

        for (int i = 0; i < 10; i++)
        {

            GameObject simulationButton = Instantiate(_simulationButtonsPrefab, _VerticalButtonGroupParent.transform);


        }
    
    }

}
