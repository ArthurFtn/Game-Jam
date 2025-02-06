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
        // Ici, tu peux afficher un écran de défaite ou recharger la scène
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge le niveau
    }
}