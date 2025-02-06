using System.Threading;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform[] waypoints; // Tableau des waypoints
    private int currentWaypointIndex = 0; // Indice du waypoint actuel
    public float speed = 3f; // Vitesse de déplacement

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return; // Sécurité si aucun waypoint

        // Déplacement vers le waypoint actuel
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Orientation de l'ennemi vers la prochaine destination
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);

        // Si l'ennemi atteint un waypoint, passe au suivant
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;

            // Si l'ennemi atteint le dernier waypoint (base)
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
                baseHealth.TakeDamage(1); // Inflige des dégâts à la base
            }
            else
            {
                Debug.LogError("❌ BaseHealth non trouvé sur la base !");
            }
        }

        Destroy(gameObject, 0.1f); // Supprime l'ennemi après un court délai
    }
}