using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfindingNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath currentPath;

    private Vector3 randomDestination;
    private Vector3 navDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPath = new NavMeshPath();
        randomDestination = new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
    }

    void Update()
    {
        agent.CalculatePath(randomDestination, currentPath);
        if (currentPath.corners.Length >= 2)
        {
            navDirection = currentPath.corners[1] - currentPath.corners[0];
        }
        agent.SetPath(currentPath);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < currentPath.corners.Length; i++)
            {
                Gizmos.DrawWireSphere(currentPath.corners[i], 0.1f);
            }

            Gizmos.color = Color.blue;
            if (currentPath.corners.Length >= 1)
            {
                Gizmos.DrawLine(currentPath.corners[0], currentPath.corners[0] + navDirection);
            }
        }
    }
}
