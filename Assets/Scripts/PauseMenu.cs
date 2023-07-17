using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField]
    public bool isPaused = false;


    private void Update()
    {
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game by setting the time scale to 0
        isPaused = true;
        pauseMenuUI.SetActive(true); // Show the pause menu UI
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game by setting the time scale back to 1
        isPaused = false;
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
    }

    public void QuitGame()
    {
        // You can add any additional clean-up or save game logic here

        // Load the main menu scene or quit the application
        // If you have a specific main menu scene, use SceneManager.LoadScene("MainMenu");
        // If you want to quit the application, use Application.Quit();
        SceneManager.LoadScene("MenuScenes");
        // Application.Quit();
    }
}
