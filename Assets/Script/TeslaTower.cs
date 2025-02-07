using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeslaTower : MonoBehaviour
{
    public int cost = 150; // Ajout du coût de la tour

    public float attackRange = 7f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    public GameObject lightningEffectPrefab;
    public float lightningDuration = 1f;
    public int maxChainTargets = 3;
    public int damagePerTarget = 20;

    public float slowFactor = 0.5f; // 50% speed reduction
    public float slowDuration = 2f; // Slows enemies for 2 seconds

    private AudioSource audioSource;
    public GameObject lightningOrb; // Assign Lightning Orb in Inspector
    public float orbDuration = 0.3f; // Orb visibility duration

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (lightningOrb != null)
        {
            lightningOrb.SetActive(false); // Ensure orb is off at the start
        }
    }

    void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            ChainLightningAttack();
        }
    }

    void ChainLightningAttack()
    {
        Enemy[] enemies = FindEnemiesInRange();
        if (enemies.Length == 0) return;

        lastAttackTime = Time.time;

        if (audioSource != null)
        {
            audioSource.Play(); // Play attack sound
        }

        if (lightningOrb != null)
        {
            StartCoroutine(ShowLightningOrb()); // Show orb when attacking
        }

        Vector3 startPoint = transform.position;

        for (int i = 0; i < Mathf.Min(maxChainTargets, enemies.Length); i++)
        {
            Enemy target = enemies[i];

            // Apply damage
            target.TakeDamage(damagePerTarget);

            // Apply slow effect
            target.ApplySlow(slowFactor, slowDuration);
            Debug.Log("Slowing enemy: " + target.name);

            // Spawn Lightning Effect
            SpawnLightningEffect(startPoint, target.transform.position);

            // Move starting point for next chain jump
            startPoint = target.transform.position;

        }
    }

    IEnumerator ShowLightningOrb()
    {
        lightningOrb.SetActive(true); // Show the orb
        yield return new WaitForSeconds(orbDuration); // Keep it visible for some time
        lightningOrb.SetActive(false); // Hide the orb
    }


    void SpawnLightningEffect(Vector3 start, Vector3 end)
    {
        if (lightningEffectPrefab != null)
        {
            GameObject lightning = Instantiate(lightningEffectPrefab, start, Quaternion.identity);
            LightningEffect effect = lightning.GetComponent<LightningEffect>();
            if (effect != null)
            {
                effect.Initialize(start, end);
            }
        }
    }

    Enemy[] FindEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Enemy enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange.ToArray();
    }
}
