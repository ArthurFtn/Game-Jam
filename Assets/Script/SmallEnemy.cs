using UnityEngine;

public class SmallEnemy : Enemy
{
    protected override void Start()
    {
        maxHealth = 40f; // Peu de PV
        speed = 4.5f; // Très rapide
        base.Start();
    }
}