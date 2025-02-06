using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel; // Le panel de Game Over
    public Button restartButton; // Le bouton de redémarrage

    void Start()
    {
        gameOverPanel.SetActive(false); // Cache l'écran Game Over au début
        restartButton.onClick.AddListener(RestartGame); // Ajoute l'événement au bouton
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // Affiche l'écran Game Over
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la scène
    }
}