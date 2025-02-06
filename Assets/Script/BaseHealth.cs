using UnityEngine;
using UnityEngine.SceneManagement; // Permet de recharger la scène en cas de game over

public class BaseHealth : MonoBehaviour
{
    public int health = 5; // Points de vie de la base

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("🔥 Base touchée ! Points de vie restants : " + health);

        if (health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("💀 GAME OVER !");

        GameOverUI gameOverUI = FindObjectOfType<GameOverUI>(); // Trouver l'UI Game Over
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver(); // Afficher l'écran de défaite
        }
        else
        {
            Debug.LogError("⚠️ Aucun GameOverUI trouvé dans la scène !");
        }
    }
}
