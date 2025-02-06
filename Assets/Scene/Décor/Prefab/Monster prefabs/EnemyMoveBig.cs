using System.Collections;
using UnityEngine;

public class EnemyMoveBig : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 3f;
    private bool isWaiting = true;

    void Start()
    {
        StartCoroutine(AttendreEtCommencerDeplacement());
        IEnumerator AttendreEtCommencerDeplacement()
        {
            Debug.Log("Attente de 3 secondes avant le déplacement...");
            yield return new WaitForSeconds(3f);  // Attente de 3 secondes
            isWaiting = false;
            Debug.Log("Déplacement commencé !");
        }
    }



    void Update()
    {
        if (isWaiting || waypoints == null || waypoints.Length == 0) return;

        // Déplacement vers le waypoint actuel
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Orientation vers la prochaine destination
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);

        // Vérifie si l'ennemi a atteint le waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            Debug.Log($"Waypoint {currentWaypointIndex} atteint.");
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachBase();
            }
        }
    }

    void ReachBase()
    {
        Debug.Log("💥 L'ennemi a atteint la base !");
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");

        if (baseObject != null)
        {
            BaseHealth baseHealth = baseObject.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(1);
            }
            else
            {
                Debug.LogError("❌ BaseHealth non trouvé sur la base !");
            }
        }

        Destroy(gameObject, 0.1f);
    }
}
