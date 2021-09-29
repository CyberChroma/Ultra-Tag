using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarUI : MonoBehaviour
{
    private bool barOnSelf;
    private RectTransform rect;
    private RectTransform onScreenPosRect;
    private RectTransform offScreenPosRect;
    private Slider selfScorebar;
    private Image scorebarFill;
    private Image iconBackground;
    private Image icon;
    private Image itStateBackground;
    private Transform charWeAreOn;
    private ITCharacterTracker itCharacterTracker;

    public void SetUp(CharacterInfo characterInfo, float winTime, bool selfScore, bool startingIt)
    {
        rect = GetComponent<RectTransform>();
        onScreenPosRect = transform.parent.Find("On Screen Pos").GetComponent<RectTransform>();
        offScreenPosRect = transform.parent.Find("Off Screen Pos").GetComponent<RectTransform>();

        barOnSelf = selfScore;
        if (!selfScore) {
            rect.SetParent(offScreenPosRect, true);
            rect.localPosition = Vector2.zero;
        }

        selfScorebar = GetComponent<Slider>();
        selfScorebar.maxValue = winTime;
        selfScorebar.value = 0;
        scorebarFill = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        scorebarFill.color = characterInfo.characterColour;

        iconBackground = transform.Find("Character Icon Border").Find("Character Icon Background").GetComponent<Image>();
        iconBackground.color = characterInfo.characterColour;
        icon = iconBackground.transform.Find("Character Icon").GetComponent<Image>();
        icon.sprite = characterInfo.characterIcon;

        itStateBackground = transform.Find("It State Outline").GetComponent<Image>();
        if (startingIt) {
            itStateBackground.color = Color.red;
        }
        else {
            itStateBackground.color = Color.green;
        }

        charWeAreOn = GetComponentInParent<CharacterMove>().transform;
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
    }

    public void UpdateUI(float curScore, bool isIt)
    {
        selfScorebar.value = curScore;
        if (isIt) {
            itStateBackground.color = Color.red;
        }
        else {
            itStateBackground.color = Color.green;
        }
    }

    public void Move(bool first, bool second, float smoothing)
    {
        if (!barOnSelf) {
            if (itCharacterTracker.winningCharacter != charWeAreOn && first || itCharacterTracker.winningCharacter == charWeAreOn && second) {
                rect.SetParent(onScreenPosRect, true);
            } else {
                rect.SetParent(offScreenPosRect, true);
            }
            rect.localPosition = new Vector3(rect.localPosition.x, Mathf.Lerp(rect.localPosition.y, 0, smoothing));
        }
    }
}
