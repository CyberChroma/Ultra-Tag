using UnityEngine;
using UnityEngine.AI;

public class AIPathfindingNavMesh : MonoBehaviour
{
    private enum AIState
    {
        CoinHunting, Fleeing
    }

    private NavMeshAgent agent;
    private CharacterTag characterTag;
    private CharacterStateTracker tracker;

    [SerializeField] private float playerPanicDistance = 3.5f;
    [SerializeField] private float minSafeDistanceFromPlayer = 8f;
    [SerializeField] private float stuckCheckInterval = 1f;
    [SerializeField] private float stuckThreshold = 0.1f;
    [SerializeField] private float arrivalThreshold = 0.5f;
    [SerializeField] private float coinPlayerBlockDistance = 3f;
    [SerializeField] private float minimumFleeDuration = 1.5f;

    private float nextStuckCheckTime = 0f;
    private Vector3 lastPosition;
    private float timeEnteredFleeState = 0f;

    private AIState currentState = AIState.CoinHunting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterTag = GetComponent<CharacterTag>();
        tracker = CharacterStateTracker.Instance;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (characterTag.IsHunter)
        {
            agent.SetDestination(tracker.player.position);
            currentState = AIState.CoinHunting;
            return;
        }

        Vector3 enemyPos = transform.position;
        Vector3 playerPos = tracker.player.position;

        bool shouldFlee = ShouldFlee(enemyPos, playerPos);

        // Allow immediate switch to flee mode
        if (shouldFlee && currentState != AIState.Fleeing)
        {
            currentState = AIState.Fleeing;
            timeEnteredFleeState = Time.time;
            PickDistantFleeDestination();
        }
        // Only leave flee mode after cooldown
        else if (!shouldFlee && currentState == AIState.Fleeing && Time.time - timeEnteredFleeState >= minimumFleeDuration)
        {
            currentState = AIState.CoinHunting;
            Transform coin = GetSafeNearestCoin(enemyPos, playerPos);
            if (coin != null)
                agent.SetDestination(coin.position);
        }

        if (currentState == AIState.Fleeing)
        {
            HandleSmartFleeing();
        }
        else
        {
            Transform coin = GetSafeNearestCoin(enemyPos, playerPos);
            if (coin != null)
                agent.SetDestination(coin.position);
        }
    }

    void OnDisable()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
    }

    private bool ShouldFlee(Vector3 enemyPos, Vector3 playerPos)
    {
        float distToPlayer = Vector3.Distance(enemyPos, playerPos);
        if (distToPlayer < playerPanicDistance)
            return true;

        Transform coin = GetSafeNearestCoin(enemyPos, playerPos);
        return coin == null;
    }

    private void HandleSmartFleeing()
    {
        if (!agent.pathPending && agent.remainingDistance <= arrivalThreshold)
        {
            PickDistantFleeDestination();
            return;
        }

        if (Time.time > nextStuckCheckTime)
        {
            float movedDistance = Vector3.Distance(transform.position, lastPosition);

            if (movedDistance < stuckThreshold)
            {
                PickDistantFleeDestination();
            }

            lastPosition = transform.position;
            nextStuckCheckTime = Time.time + stuckCheckInterval;
        }
    }

    private void PickDistantFleeDestination()
    {
        Vector3 playerPos = tracker.player.position;
        int attempts = 10;

        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * 15f;
            randomDir.y = 0;

            Vector3 candidate = transform.position + randomDir;

            if (Vector3.Distance(candidate, playerPos) < minSafeDistanceFromPlayer)
                continue;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }

        if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * 10f, out NavMeshHit fallbackHit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(fallbackHit.position);
        }
    }

    private Transform GetSafeNearestCoin(Vector3 enemyPos, Vector3 playerPos)
    {
        Transform bestCoin = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform coin in CoinManager.Instance.ActiveCoins)
        {
            float playerDist = Vector3.Distance(playerPos, coin.position);

            if (playerDist < coinPlayerBlockDistance)
                continue;

            float distToCoin = Vector3.Distance(enemyPos, coin.position);
            if (distToCoin < closestDistance)
            {
                closestDistance = distToCoin;
                bestCoin = coin;
            }
        }

        return bestCoin;
    }
}