using UnityEngine;
using UnityEngine.AI;

public class AIPathfindingNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterTag characterTag;
    private CharacterStateTracker tracker;
    private CoinManager coinManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterTag = GetComponent<CharacterTag>();

        tracker = CharacterStateTracker.Instance;
        coinManager = FindFirstObjectByType<CoinManager>();
    }

    void Update()
    {
        if (characterTag.IsHunter)
        {
            agent.SetDestination(tracker.player.position);
        }
        else
        {
            Transform nearestCoin = GetNearestCoin();
            if (nearestCoin != null)
            {
                agent.SetDestination(nearestCoin.position);
            }
        }
    }

    private Transform GetNearestCoin()
    {
        float shortestDistance = Mathf.Infinity;
        Transform closest = null;
        Vector3 myPos = transform.position;

        foreach (Transform coin in CoinManager.Instance.ActiveCoins)
        {
            float dist = Vector3.Distance(myPos, coin.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = coin;
            }
        }

        return closest;
    }
}
