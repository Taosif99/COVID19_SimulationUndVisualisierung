using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

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
}
