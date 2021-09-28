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
    private float curTime = 0;
    private int minutes = 0;
    private int seconds = 0;
    private ITCharacterTracker itCharacterTracker;
    private ScoreBarUI[] scoreBars;

    // Start is called before the first frame update
    void Start()
    {
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
        itCharacterTracker.characterNotItTimes.Add(transform, 0);
        itCharacterTracker.characterScoreLeft.Add(transform, winTime);
        Canvas[] characterCanvases = FindObjectsOfType<Canvas>();
        scoreBars = new ScoreBarUI[characterCanvases.Length];

        Canvas selfCanvas = GetComponentInChildren<Canvas>();
        for (int i = 0; i < characterCanvases.Length; i++) {
            scoreBars[i] = Instantiate(scoreBarPrefab, characterCanvases[i].transform).GetComponent<ScoreBarUI>();
            scoreBars[i].name = name + " Score Bar";
            if (characterCanvases[i] == selfCanvas) {
                scoreBars[i].SetUp(characterInfo, winTime, true);
            }
            else {
                scoreBars[i].SetUp(characterInfo, winTime, false);
            }
        }
        curTime += Random.Range(0f, 1f);
        itCharacterTracker.characterNotItTimes[transform] = curTime;
    }

    // Update is called once per frame
    void Update()
    {
        curTime = itCharacterTracker.characterNotItTimes[transform];
        if (itCharacterTracker.ITCharacter != transform) {
            curTime += Time.deltaTime;
            minutes = (int)curTime / 60;
            seconds = (int)curTime % 60;

            if (itCharacterTracker.longestNotItCharacter == transform) {
                curScore += Time.deltaTime;
                itCharacterTracker.characterScoreLeft[transform] = winTime - curScore;
            }

            if (curScore >= winTime) {
                print(name + " Won!!!");
            }

            itCharacterTracker.characterNotItTimes[transform] = curTime;
        }
        else if (seconds != 0) {
            minutes = 0;
            seconds = 0;
        }

        float timeSmoothing = scorebarMoveSmoothing * Time.deltaTime;
        bool first = itCharacterTracker.winningCharacter == transform;
        bool rising = itCharacterTracker.longestNotItCharacter == transform;
        foreach (ScoreBarUI scoreBar in scoreBars) {
            scoreBar.UpdateUI(minutes, seconds, curScore);
            scoreBar.Move(first, rising, timeSmoothing);
        }
    }
}
