using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterScore : MonoBehaviour
{
    public int winCoins = 10;
    public CharacterInfo characterInfo;

    [HideInInspector]
    public UnityEvent<Transform, float> OnScoreChanged = new UnityEvent<Transform, float>();
    public static event System.Action<Transform> OnCharacterWon;

    public bool IsPlayer
    {
        get; private set;
    }

    private CharacterTag characterTag;
    private float curScore = 0;

    private void Awake()
    {
        IsPlayer = GetComponent<PlayerInput>() != null;
        characterTag = GetComponent<CharacterTag>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!characterTag.IsHunter && other.CompareTag("Coin"))
        {
            curScore++;
            OnScoreChanged.Invoke(transform, winCoins - curScore);
            CoinManager.Instance.RandomlyPlaceCoin(other.transform);

            if (curScore >= winCoins)
            {
                OnCharacterWon?.Invoke(transform);
            }
        }
    }
}
