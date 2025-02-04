using UnityEngine;

public class InfernoTower : MonoBehaviour
{
    public float attackRange = 6f;
    public float baseDamage = 5f;
    public float maxDamage = 30f;
    public float damageIncreaseRate = 2f;
    public float attackInterval = 0.5f;

    public ParticleSystem fireEffect;
    public LineRenderer fireBeam; // 🔥 Fire beam (LineRenderer)
    public Transform firePoint; // 🔥 Position where the beam starts (attach to tower top)

    private Enemy currentTarget;
    private float currentDamage;
    private float lastAttackTime;

    void Start()
    {
        fireBeam.enabled = false; // Disable beam initially
    }

   void Update()
{
    Enemy enemy = FindClosestEnemy();

    if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
    {
        if (enemy != currentTarget)
        {
            currentTarget = enemy;
            currentDamage = baseDamage;
        }

        if (Time.time - lastAttackTime >= attackInterval)
        {
            Attack(currentTarget);
        }

        if (!fireEffect.isPlaying) fireEffect.Play();

        // 🔥 Ensure beam is updated every frame
        DrawFireBeam(enemy.transform.position);
    }
    else
    {
        currentTarget = null;
        currentDamage = baseDamage;

        if (fireEffect.isPlaying) fireEffect.Stop();
        
        // ❌ Disable beam when no enemy
        fireBeam.enabled = false;
    }
}



    void Attack(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(currentDamage);
            currentDamage = Mathf.Min(currentDamage + damageIncreaseRate, maxDamage);
            lastAttackTime = Time.time;

            // 🔥 Adjust fire intensity based on damage
            var main = fireEffect.main;
            main.startSize = Mathf.Lerp(0.5f, 3f, currentDamage / maxDamage);
        }
    }

    void DrawFireBeam(Vector3 targetPosition)
{
    if (!fireBeam.enabled)
        fireBeam.enabled = true; // Enable fire beam when attacking

    Vector3 start = firePoint.position; // 🔥 Fire starts from FirePoint
    Vector3 end = targetPosition + Vector3.up * 0.5f; // Adjust target to mid-enemy

    Debug.Log($"🔥 Fire Start: {start}, Fire End: {end}"); // Print positions

    fireBeam.SetPosition(0, start);
    fireBeam.SetPosition(1, end);
}


    Enemy FindClosestEnemy()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (enemies.Length == 0) return null;

        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }
}
