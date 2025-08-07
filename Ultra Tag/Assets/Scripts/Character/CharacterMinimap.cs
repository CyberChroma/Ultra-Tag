using UnityEngine;

public class CharacterMinimap : MonoBehaviour
{
    [Header("Setup")]
    public float minimapIconOffset = 150f;
    public SpriteRenderer minimapIconPrefab;
    public CharacterInfo characterInfo;

    private Transform iconsParent;
    private SpriteRenderer characterIcon;
    private SpriteRenderer iconBackground;
    private SpriteRenderer iconBorder;
    private SpriteRenderer iconRoleOutline;
    private CharacterTag characterTag;

    private void Start()
    {
        characterTag = GetComponent<CharacterTag>();
        iconsParent = FindIconsParent();

        CreateMinimapIcon();
        ApplyInitialVisuals();
        AssignSortingOrder();
    }

    private void Update()
    {
        UpdateIconPosition();
        UpdateRoleColor();
    }

    private Transform FindIconsParent()
    {
        GameObject iconsRoot = GameObject.FindWithTag("MinimapIcons");
        return iconsRoot.transform;
    }

    private void CreateMinimapIcon()
    {
        characterIcon = Instantiate(minimapIconPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f), iconsParent);
        characterIcon.name = $"{name} Character Icon";
    }

    private void ApplyInitialVisuals()
    {
        characterIcon.sprite = characterInfo.characterIcon;

        iconBackground = characterIcon.transform.Find("Icon Background").GetComponent<SpriteRenderer>();
        iconBackground.color = characterInfo.characterColour;

        iconBorder = characterIcon.transform.Find("Icon Border").GetComponent<SpriteRenderer>();
        iconRoleOutline = characterIcon.transform.Find("Icon Role Outline").GetComponent<SpriteRenderer>();
    }

    private void AssignSortingOrder()
    {
        int baseOrder = Random.Range(0, 100) * 4;
        iconRoleOutline.sortingOrder = baseOrder;
        iconBorder.sortingOrder = baseOrder + 1;
        iconBackground.sortingOrder = baseOrder + 2;
        characterIcon.sortingOrder = baseOrder + 3;
    }

    private void UpdateIconPosition()
    {
        Vector3 pos = transform.position;
        characterIcon.transform.position = new Vector3(pos.x, minimapIconOffset, pos.z);
        Vector3 targetEuler = characterIcon.transform.eulerAngles;
        targetEuler.y = transform.eulerAngles.y;
        characterIcon.transform.eulerAngles = targetEuler;
    }

    private void UpdateRoleColor()
    {
        iconRoleOutline.color = characterTag.IsHunter ? Color.red : Color.green;
    }
}
