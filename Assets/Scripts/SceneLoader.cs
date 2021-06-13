using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuTMP");
            
    }

    public void LoadSimulation()
    {
        SceneManager.LoadScene("SimulationTMP");
    }

    public void LoadStartSimulation()
    {
        SceneManager.LoadScene("StartSimulation");
    }

    public void LoadCreateSimulation()
    {

        SceneManager.LoadScene("CreateSimulationTMP");
    }

    public void LoadAboutUs()
    {

        SceneManager.LoadScene("AboutUsTMP");
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
