using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target; // La base

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Trouve la base avec le tag "Base"
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            target = baseObject.transform;
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogError("❌ Aucune base trouvée ! Assurez-vous qu'elle a le tag 'Base'.");
        }
    }
}