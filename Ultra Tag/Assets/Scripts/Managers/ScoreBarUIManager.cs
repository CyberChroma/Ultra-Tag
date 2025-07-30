using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarUIManager : MonoBehaviour
{
    [SerializeField] private Slider scoreBarPrefab;

    private Dictionary<Transform, ScoreBarUI> scoreBarUIs = new Dictionary<Transform, ScoreBarUI>();
    private Dictionary<Transform, CharacterScore> characterScores = new Dictionary<Transform, CharacterScore>();

    private void Start()
    {
        SetupScoreBars();
    }

    private void OnEnable()
    {
        CharacterTag.OnTagged += HandleTagChange;
    }

    private void OnDisable()
    {
        CharacterTag.OnTagged -= HandleTagChange;
    }

    private void SetupScoreBars()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        CharacterScore[] allScores = FindObjectsByType<CharacterScore>(FindObjectsSortMode.None);

        foreach (CharacterScore score in allScores)
        {
            Transform character = score.transform;

            CharacterTag tag = character.GetComponent<CharacterTag>();
            ScoreBarUI ui = Instantiate(scoreBarPrefab, canvas.transform).GetComponent<ScoreBarUI>();
            ui.name = character.name + " Score Bar";
            ui.SetUp(score.characterInfo, score.winCoins, score.IsPlayer, tag.IsHunter, character);

            scoreBarUIs[character] = ui;
            characterScores[character] = score;

            score.OnScoreChanged.AddListener((c, scoreLeft) =>
            {
                ui.UpdateUI(score.winCoins - scoreLeft, tag.IsHunter);
            });

            float currentScore = 0f;
            ui.UpdateUI(currentScore, tag.IsHunter);
        }
    }

    private void Update()
    {
        CharacterStateTracker tracker = CharacterStateTracker.Instance;
        if (tracker == null)
            return;

        foreach (var kvp in scoreBarUIs)
        {
            Transform character = kvp.Key;
            ScoreBarUI scoreBar = kvp.Value;
            CharacterScore score = characterScores[character];

            if (!score.IsPlayer)
            {
                bool isFirst = character == tracker.winningCharacter;
                bool isSecond = character == tracker.secondCharacter;
                scoreBar.Move(isFirst, isSecond);
            }
        }
    }

    private void HandleTagChange(Transform hunter, Transform target)
    {
        UpdateRoleColor(hunter);
        UpdateRoleColor(target);
    }

    private void UpdateRoleColor(Transform character)
    {
        if (scoreBarUIs.TryGetValue(character, out ScoreBarUI scoreBar) &&
            characterScores.TryGetValue(character, out CharacterScore score))
        {
            bool isHunter = character.GetComponent<CharacterTag>().IsHunter;
            scoreBar.UpdateUI(score.winCoins - CharacterStateTracker.Instance.characterScoreLeft[character], isHunter);
        }
    }

}
