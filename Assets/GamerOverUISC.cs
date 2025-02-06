using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    private BaseHealth playerBaseHealth;

    void Start()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);

        // Automatically find BaseHealth if not manually assigned
        playerBaseHealth = FindObjectOfType<BaseHealth>();

        if (playerBaseHealth != null)
        {
            playerBaseHealth.OnDeath += ShowGameOver;
        }
        else
        {
            Debug.LogError("No BaseHealth script found in the scene!");
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDestroy()
    {
        if (playerBaseHealth != null)
        {
            playerBaseHealth.OnDeath -= ShowGameOver;
        }
    }
}