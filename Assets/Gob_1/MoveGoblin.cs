using UnityEngine;
using UnityEngine.AI;

public class MoveGoblin : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target; // Cible du gobelin

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target != null)
        {
            agent.SetDestination(target.position); // Le gobelin court vers la cible
        }
    }
}