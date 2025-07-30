using UnityEngine;
using UnityEngine.UI;

public class ScoreBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider scorebar;
    [SerializeField] private Image scorebarFill;
    [SerializeField] private Image iconBackground;
    [SerializeField] private Image icon;
    [SerializeField] private Image roleBackground;

    [SerializeField] private RectTransform onScreenPosRect;
    [SerializeField] private RectTransform offScreenPosRect;

    private RectTransform rect;
    private CharacterStateTracker characterStateTracker;
    private Transform attachedCharacter;

    private static readonly Color HunterColor = Color.red;
    private static readonly Color EvaderColor = Color.green;

    public void SetUp(CharacterInfo characterInfo, float winScore, bool isPlayer, bool startingHunter, Transform character)
    {
        rect = GetComponent<RectTransform>();
        attachedCharacter = character;
        characterStateTracker = CharacterStateTracker.Instance;

        onScreenPosRect = transform.parent.Find("On Screen Pos").GetComponent<RectTransform>();
        offScreenPosRect = transform.parent.Find("Off Screen Pos").GetComponent<RectTransform>();

        if (!isPlayer)
        {
            rect.SetParent(offScreenPosRect, true);
            rect.localPosition = Vector2.zero;
        }

        scorebar.maxValue = winScore;
        scorebar.value = 0;

        scorebarFill.color = characterInfo.characterColour;
        iconBackground.color = characterInfo.characterColour;
        icon.sprite = characterInfo.characterIcon;

        roleBackground.color = startingHunter ? HunterColor : EvaderColor;
    }

    public void UpdateUI(float curScore, bool isHunter)
    {
        scorebar.value = curScore;
        roleBackground.color = isHunter ? HunterColor : EvaderColor;
    }

    public void Move(bool isFirst, bool isSecond)
    {
        const float smoothing = 10f;
        bool shouldMoveOnScreen = ShouldBeOnScreen(isFirst, isSecond);

        rect.SetParent(shouldMoveOnScreen ? onScreenPosRect : offScreenPosRect, true);
        rect.localPosition = new Vector3(rect.localPosition.x, Mathf.Lerp(rect.localPosition.y, 0f, Time.deltaTime * smoothing));
    }

    private bool ShouldBeOnScreen(bool isFirst, bool isSecond)
    {
        Transform player = CharacterStateTracker.Instance.player;
        return isFirst || (isSecond && CharacterStateTracker.Instance.winningCharacter == player);
    }
}
