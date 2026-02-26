using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = true;

    void Start()
    {
        PauseGame(); // Start game paused
    }

    void Update()
    {
        // Toggle Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // Restart Game
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void RestartGame()
    {
        Time.timeScale = 1f; // Important reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}