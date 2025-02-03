using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackRate = 1f;
    public int damage = 10;
    public int chainCount = 2;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    void Start()
    {
        GetComponent<SphereCollider>().radius = attackRange;
        InvokeRepeating("Attack", 0f, 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

 