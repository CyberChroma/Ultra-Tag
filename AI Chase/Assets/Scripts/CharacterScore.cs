using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScore : MonoBehaviour
{
    public int winTime = 150;
    public float scorebarMoveSmoothing = 10;
    public CharacterInfo characterInfo;
    public Slider scoreBarPrefab;

    private float curScore = 0;
    private ITCharacterTracker itCharacterTracker;
    private ScoreBarUI[] scoreBars;

    // Start is called before the first frame update
    void Start()
    {
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
        itCharacterTracker.characterScoreLeft.Add(transform, winTime);
        Canvas[] characterCanvases = FindObjectsOfType<Canvas>();
        scoreBars = new ScoreBarUI[characterCanvases.Length];

        Canvas selfCanvas = GetComponentInChildren<Canvas>();
        for (int i = 0; i < characterCanvases.Length; i++) {
            scoreBars[i] = Instantiate(scoreBarPrefab, characterCanvases[i].transform).GetComponent<ScoreBarUI>();
            scoreBars[i].name = name + " Score Bar";
            scoreBars[i].SetUp(characterInfo, winTime, characterCanvases[i] == selfCanvas, itCharacterTracker.ITCharacters.Contains(transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!itCharacterTracker.ITCharacters.Contains(transform)) {
            curScore += Time.deltaTime;
            itCharacterTracker.characterScoreLeft[transform] = winTime - curScore;

            if (curScore >= winTime) {
                print(name + " Won!!!");
            }
        }

        float timeSmoothing = scorebarMoveSmoothing * Time.deltaTime;
        bool first = itCharacterTracker.winningCharacter == transform;
        bool second = itCharacterTracker.secondCharacter == transform;
        foreach (ScoreBarUI scoreBar in scoreBars) {
            scoreBar.UpdateUI(curScore, itCharacterTracker.ITCharacters.Contains(transform));
            scoreBar.Move(first, second, timeSmoothing);
        }
    }
}
