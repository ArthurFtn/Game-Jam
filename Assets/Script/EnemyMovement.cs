using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints; // Tableau des points de passage
    public float speed = 5f; // Vitesse de l'ennemi
    private int currentWaypointIndex = 0; // Index du waypoint actuel

    void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            MoveEnemy();
        }
    }

    void MoveEnemy()
    {  
        // Déplacement de l'ennemi vers le waypoint actuel
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        float step = speed * Time.deltaTime; // Calcul de l'étape de déplacement

        // Déplace l'ennemi vers le waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Si l'ennemi a atteint le waypoint, passe au suivant
        if (transform.position == targetWaypoint.position)
        {
            currentWaypointIndex++;
        }
    }
}
