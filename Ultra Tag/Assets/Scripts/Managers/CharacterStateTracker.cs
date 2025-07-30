using System.Collections.Generic;
using UnityEngine;

public class CharacterStateTracker : MonoBehaviour
{
    public static CharacterStateTracker Instance
    {
        get; private set;
    }

    [Header("Setup")]
    public Transform player;
    [SerializeField] private EndGameUI endGameUI;

    [Header("Runtime")]
    public List<Transform> allCharacters { get; private set; } = new List<Transform>();
    public Dictionary<Transform, float> characterScoreLeft = new Dictionary<Transform, float>();
    public Transform winningCharacter
    {
        get; private set;
    }
    public Transform secondCharacter
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        winningCharacter = player;
        secondCharacter = player;

        CharacterScore[] allScores = FindObjectsByType<CharacterScore>(FindObjectsSortMode.None);
        foreach (CharacterScore score in allScores)
        {
            Transform character = score.transform;
            allCharacters.Add(character);
            score.OnScoreChanged.AddListener(HandleScoreChanged);
            RegisterCharacterScore(character, score.winCoins);
        }

        CharacterScore.OnCharacterWon += HandleCharacterWon;

        RecalculateCurrentLeaders();
    }

    private void OnDestroy()
    {
        CharacterScore.OnCharacterWon -= HandleCharacterWon;
    }

    public void RegisterCharacterScore(Transform character, float startingScore)
    {
        characterScoreLeft[character] = startingScore;
    }

    private void HandleScoreChanged(Transform character, float scoreLeft)
    {
        characterScoreLeft[character] = scoreLeft;

        RecalculateCurrentLeaders();
    }

    private void RecalculateCurrentLeaders()
    {
        Transform leader = null;
        Transform second = null;
        float min = Mathf.Infinity;
        float secondMin = Mathf.Infinity;

        foreach (var kvp in characterScoreLeft)
        {
            float score = kvp.Value;
            if (score < min)
            {
                second = leader;
                secondMin = min;

                leader = kvp.Key;
                min = score;
            }
            else if (score < secondMin)
            {
                second = kvp.Key;
                secondMin = score;
            }
        }

        winningCharacter = leader;
        secondCharacter = second;
    }

    private void HandleCharacterWon(Transform winner)
    {
        endGameUI.EndGame(winner.name);
    }
}
