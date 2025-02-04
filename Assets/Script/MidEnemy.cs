using UnityEngine;

public class MidEnemy : Enemy
{
    protected override void Start()
    {
        maxHealth = 75f; // PV moyens
        speed = 2.5f; // Vitesse moyenne
        base.Start();
    }
}