using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // Ajout pour TextMeshPro (si utilisé)

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button pauseButton; // Référence au bouton
    [SerializeField] TextMeshProUGUI pauseButtonText; // Référence au texte du bouton

    private bool isPaused = false;

    void Start()
    {
        UpdateButtonText();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }

        UpdateButtonText();
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    private void UpdateButtonText()
    {
        if (isPaused)
            pauseButtonText.text = "Resume";
        else
            pauseButtonText.text = "Pause";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
