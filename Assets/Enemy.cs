
using UnityEngine;
using UnityEngine.AI;

public class PathFollowingEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] pathPoints;
    public float movementSpeed = 5f;

    private int currentPathIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        SetNextDestination();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNextDestination();
        }
    }

    void SetNextDestination()
    {
        if (currentPathIndex >= pathPoints.Length)
        {
            // Reached end of path
            Destroy(gameObject);
            return;
        }

        agent.SetDestination(pathPoints[currentPathIndex].position);
        currentPathIndex++;
    }
}
