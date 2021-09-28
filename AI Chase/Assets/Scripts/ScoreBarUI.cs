using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarUI : MonoBehaviour
{
    public Image fadedIconPrefab;

    private bool barOnSelf;
    private Slider selfScorebar;
    private Image scorebarFill;
    private Image iconBackground;
    private Image icon;
    private Text selfTimeText;
    private Text selfTimeTextDropshadow;
    private RectTransform rect;
    private RectTransform firstOnScreenPosRect;
    private RectTransform risingOnScreenPosRect;
    private RectTransform offScreenPosRect;
    private RectTransform fadedIconRect;
    private Image fadedIconBackground;
    private Image fadedIcon;

    private RectTransform fadedIcon2Rect;
    private Image fadedIcon2Background;
    private Image fadedIcon2;

    //private ITCharacterTracker itCharacterTracker;

    public void SetUp(CharacterInfo characterInfo, float winTime, bool selfScore)
    {
        rect = GetComponent<RectTransform>();
        firstOnScreenPosRect = transform.parent.Find("First On Screen Pos").GetComponent<RectTransform>();
        risingOnScreenPosRect = transform.parent.Find("Rising On Screen Pos").GetComponent<RectTransform>();
        offScreenPosRect = transform.parent.Find("Off Screen Pos").GetComponent<RectTransform>();

        barOnSelf = selfScore;
        if (!selfScore) {
            rect.SetParent(offScreenPosRect, true);
            rect.localPosition = Vector2.zero;
        }

        selfScorebar = GetComponent<Slider>();
        selfScorebar.maxValue = winTime;
        selfScorebar.value = 0;
        scorebarFill = selfScorebar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        scorebarFill.color = characterInfo.characterColour;

        iconBackground = selfScorebar.transform.Find("Character Icon Border").Find("Character Icon Background").GetComponent<Image>();
        iconBackground.color = characterInfo.characterColour;
        icon = iconBackground.transform.Find("Character Icon").GetComponent<Image>();
        icon.sprite = characterInfo.characterIcon;

        selfTimeTextDropshadow = selfScorebar.transform.Find("Not It Time Text Dropshadow").GetComponent<Text>();
        selfTimeText = selfTimeTextDropshadow.transform.Find("Not It Time Text").GetComponent<Text>();
        selfTimeTextDropshadow.text = "0:00";
        selfTimeText.text = "0:00";

        fadedIconRect = Instantiate(fadedIconPrefab, offScreenPosRect).GetComponent<RectTransform>();
        fadedIconRect.name = name + " Faded Icon";
        fadedIconBackground = fadedIconRect.transform.Find("Faded Character Icon Background").GetComponent<Image>();
        fadedIconBackground.color = characterInfo.characterColour;
        fadedIcon = fadedIconBackground.transform.Find("Faded Character Icon").GetComponent<Image>();
        fadedIcon.sprite = characterInfo.characterIcon;

        if (barOnSelf) {
            fadedIcon2Rect = Instantiate(fadedIconPrefab, offScreenPosRect).GetComponent<RectTransform>();
            fadedIcon2Rect.name = name + " Faded Icon 2";
            fadedIcon2Background = fadedIcon2Rect.transform.Find("Faded Character Icon Background").GetComponent<Image>();
            fadedIcon2Background.color = characterInfo.characterColour;
            fadedIcon2 = fadedIcon2Background.transform.Find("Faded Character Icon").GetComponent<Image>();
            fadedIcon2.sprite = characterInfo.characterIcon;
        }
    }

    public void UpdateUI(int minutes, int seconds, float curScore)
    {
        selfScorebar.value = curScore;
        selfTimeTextDropshadow.text = string.Format("{0}:{1}", minutes.ToString("0"), seconds.ToString("00"));
        selfTimeText.text = string.Format("{0}:{1}", minutes.ToString("0"), seconds.ToString("00"));
    }

    public void Move(bool first, bool rising, float smoothing)
    {
        if (barOnSelf) {
            if (first) {
                fadedIconRect.SetParent(firstOnScreenPosRect, true);
            } else {
                fadedIconRect.SetParent(offScreenPosRect, true);
            }
            if (rising) {
                fadedIcon2Rect.SetParent(risingOnScreenPosRect, true);
            } else {
                fadedIcon2Rect.SetParent(offScreenPosRect, true);
            }
            fadedIcon2Rect.localPosition = new Vector3(fadedIcon2Rect.localPosition.x, Mathf.Lerp(fadedIcon2Rect.localPosition.y, 10, smoothing));
        }
        else {
            if (first) {
                rect.SetParent(firstOnScreenPosRect, true);
            }
            else if (rising) {
                rect.SetParent(risingOnScreenPosRect, true);
            }
            if (first && rising) {
                fadedIconRect.SetParent(risingOnScreenPosRect, true);
            }
            else {
                fadedIconRect.SetParent(offScreenPosRect, true);
            }
            if (!first && !rising) {
                rect.SetParent(offScreenPosRect, true);
            }
            rect.localPosition = new Vector3(rect.localPosition.x, Mathf.Lerp(rect.localPosition.y, 0, smoothing));
        }
        fadedIconRect.localPosition = new Vector3(fadedIconRect.localPosition.x, Mathf.Lerp(fadedIconRect.localPosition.y, 10, smoothing));
    }
}
