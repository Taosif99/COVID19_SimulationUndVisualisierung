using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
            
    }

    public void LoadSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    public void LoadStartSimulation()
    {
        SceneManager.LoadScene("StartSimulation");
    }

    public void LoadCreateSimulation()
    {

        SceneManager.LoadScene("CreateSimulation");
    }


    public void QuitGame()

    {
        Application.Quit();
    }


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
