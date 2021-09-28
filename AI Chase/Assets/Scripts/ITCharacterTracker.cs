using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITCharacterTracker : MonoBehaviour
{
    public Transform ITCharacter;

    public Dictionary<Transform, float> characterNotItTimes = new Dictionary<Transform, float>();
    public Dictionary<Transform, float> characterScoreLeft = new Dictionary<Transform, float>();
    public Transform longestNotItCharacter;
    public Transform winningCharacter;

    private float curMaxTime;
    private float curMinScoreLeft;

    void Start()
    {
        longestNotItCharacter = ITCharacter;
        winningCharacter = ITCharacter;
    }

    void Update ()
    {
        curMaxTime = 0;
        foreach(Transform character in characterNotItTimes.Keys) {
            if (characterNotItTimes[character] > curMaxTime) {
                longestNotItCharacter = character;
                curMaxTime = characterNotItTimes[character];
            }
        }

        curMinScoreLeft = Mathf.Infinity;
        foreach (Transform character in characterScoreLeft.Keys) {
            if (characterScoreLeft[character] < curMinScoreLeft) {
                winningCharacter = character;
                curMinScoreLeft = characterScoreLeft[character];
            }
        }
    }
}
