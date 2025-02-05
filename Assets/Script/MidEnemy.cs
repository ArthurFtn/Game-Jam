using UnityEngine;

public class MidEnemy : Enemy
{
    protected override void Start()
    {
        maxHealth = 75f; // PV moyens
        speed = 2.5f; // Vitesse moyenne
        moneyReward = 50; // Argent donné à la mort de cet ennemi
        base.Start();
    }
}