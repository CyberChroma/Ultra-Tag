using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance
    {
        get; private set;
    }

    [Header("Config")]
    public int coinsInPlay = 2;
    public float overlapSpawnDistance = 2f;

    [Header("References")]
    public Transform[] spawnAreas;
    public GameObject coinPrefab;

    public List<Transform> ActiveCoins => coins;
    private List<Transform> coins = new List<Transform>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (coinPrefab == null || spawnAreas.Length == 0)
        {
            Debug.LogError("CoinManager setup is incomplete. Please assign all required references.");
            return;
        }

        CreateCoinPool();

        for (int i = 0; i < coinsInPlay; i++)
        {
            RandomlyPlaceCoin(coins[i]);
        }
    }

    private void CreateCoinPool()
    {
        for (int i = 0; i < coinsInPlay; i++)
        {
            GameObject coinInstance = Instantiate(coinPrefab, transform);
            coinInstance.name = $"Coin {i + 1}";
            coins.Add(coinInstance.transform);
        }
    }

    public void RandomlyPlaceCoin(Transform coin)
    {
        for (int attempts = 0; attempts < 20; attempts++)
        {
            if (TryGetValidSpawnPosition(coin, out Vector3 spawnPosition))
            {
                coin.position = spawnPosition;
                return;
            }
        }

        Debug.LogError($"Failed to place coin {coin.name} after 20 attempts.");
    }

    private bool TryGetValidSpawnPosition(Transform coin, out Vector3 position)
    {
        int randomSpawnIndex = Random.Range(0, spawnAreas.Length);
        position = spawnAreas[randomSpawnIndex].position;

        return IsFarFromCharacters(position) && IsFarFromOtherCoins(coin, position);
    }

    private bool IsFarFromCharacters(Vector3 position)
    {
        List<Transform> characters = CharacterStateTracker.Instance.allCharacters;
        for (int i = 0; i < characters.Count; i++)
        {
            if (!FarEnoughFromObject(position, characters[i]))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsFarFromOtherCoins(Transform currentCoin, Vector3 position)
    {
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i] == currentCoin)
                continue;

            if (!FarEnoughFromObject(position, coins[i]))
            {
                return false;
            }
        }
        return true;
    }

    private bool FarEnoughFromObject(Vector3 position, Transform go)
    {
        return Vector3.Distance(go.position, position) > overlapSpawnDistance;
    }
}
