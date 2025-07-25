using UnityEngine;
using UnityEngine.UI;

public class CharacterScore : MonoBehaviour
{
    public int winTime = 150;
    public float scorebarMoveSmoothing = 10;
    public CharacterInfo characterInfo;
    public Slider scoreBarPrefab;
    public bool player;

    private float curScore = 0;
    private ITCharacterTracker itCharacterTracker;
    private ScoreBarUI scoreBar;
    private EndGameUI endGameUI;

    // Start is called before the first frame update
    void Start()
    {
        itCharacterTracker = FindFirstObjectByType<ITCharacterTracker>();
        itCharacterTracker.characterScoreLeft.Add(transform, winTime);
        Canvas canvas = FindFirstObjectByType<Canvas>();
        scoreBar = Instantiate(scoreBarPrefab, canvas.transform).GetComponent<ScoreBarUI>();
        scoreBar.name = name + " Score Bar";
        scoreBar.SetUp(characterInfo, winTime, player, itCharacterTracker.ITCharacters.Contains(transform), transform);
        endGameUI = FindFirstObjectByType<EndGameUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!itCharacterTracker.ITCharacters.Contains(transform)) {
            curScore += Time.deltaTime;
            itCharacterTracker.characterScoreLeft[transform] = winTime - curScore;

            if (curScore >= winTime) {
                endGameUI.EndGame(name);
            }
        }

        float timeSmoothing = scorebarMoveSmoothing * Time.deltaTime;
        bool first = itCharacterTracker.winningCharacter == transform;
        bool second = itCharacterTracker.secondCharacter == transform;
        scoreBar.UpdateUI(curScore, itCharacterTracker.ITCharacters.Contains(transform));
        if (!player) {
            scoreBar.Move(first, second, timeSmoothing);
        }
    }
}
