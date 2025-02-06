using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform[] waypoints; // Tableau des waypoints
    private int currentWaypointIndex = 0; // Indice du waypoint actuel
    public float speed = 3f; // Vitesse de déplacement

    void Start()
    {
        // Vérifie si des waypoints sont bien assignés
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("🚨 Aucun waypoint défini pour " + gameObject.name);
            enabled = false; // Désactive ce script pour éviter les erreurs
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return; // Sécurité

        // Vérifie que l'index ne dépasse pas la taille du tableau
        if (currentWaypointIndex >= waypoints.Length)
        {
            Debug.LogWarning("🚨 " + gameObject.name + " essaie d'accéder à un waypoint inexistant !");
            return;
        }

        // Déplacement vers le waypoint actuel
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Orientation de l'ennemi vers la prochaine destination
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);

        // Vérifie si l'ennemi a atteint son waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;

            // Vérifie si l'ennemi a atteint la base
            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachBase();
            }
        }
    }

    void ReachBase()
    {
        Debug.Log("💥 " + gameObject.name + " a atteint la base !");

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
        else
        {
            Debug.LogError("❌ Aucun objet avec le tag 'Base' trouvé !");
        }

        // Désactive ce script avant de détruire l'ennemi pour éviter des erreurs
        enabled = false;

        Destroy(gameObject, 0.1f); // Supprime l'ennemi après un court délai
    }
}
