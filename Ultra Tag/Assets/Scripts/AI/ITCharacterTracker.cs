using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITCharacterTracker : MonoBehaviour
{
    public static List<Transform> ITCharacters = new List<Transform>();


    public List<Transform> startingITCharacters = new List<Transform>();
    public Dictionary<Transform, float> characterScoreLeft = new Dictionary<Transform, float>();
    public Transform winningCharacter;
    public Transform secondCharacter;
    public Transform player;

    private float curMinScoreLeft;

    void Awake()
    {
        ITCharacters = startingITCharacters;
        winningCharacter = ITCharacters[0];
        secondCharacter = ITCharacters[0];
    }

    void Update ()
    {
        curMinScoreLeft = Mathf.Infinity;
        foreach (Transform character in characterScoreLeft.Keys) {
            if (characterScoreLeft[character] <= curMinScoreLeft) {
                winningCharacter = character;
                curMinScoreLeft = characterScoreLeft[character];
            }
        }

        curMinScoreLeft = Mathf.Infinity;
        foreach (Transform character in characterScoreLeft.Keys) {
            if (characterScoreLeft[character] <= curMinScoreLeft && character != winningCharacter) {
                secondCharacter = character;
                curMinScoreLeft = characterScoreLeft[character];
            }
        }
    }
}
