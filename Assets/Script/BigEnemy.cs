using UnityEngine;

public class BigEnemy : Enemy
{
    protected override void Start()
    {
        maxHealth = 150f; // Beaucoup de PV
        speed = 1.5f; // Très lent
        base.Start();
    }
}