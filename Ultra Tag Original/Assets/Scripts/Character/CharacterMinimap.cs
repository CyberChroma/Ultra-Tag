using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMinimap : MonoBehaviour
{
    public float minimapIconOffset = 150;
    public SpriteRenderer minimapIconPrefab;
    public CharacterInfo characterInfo;

    private Transform iconsParent;
    private SpriteRenderer characterIcon;
    private SpriteRenderer iconBackground;
    private SpriteRenderer iconBorder;
    private SpriteRenderer iconItStateOutline;
    private ITCharacterTracker itCharacterTracker;

    // Start is called before the first frame update
    void Start()
    {
        iconsParent = GameObject.Find("Minimap Character Icons").transform;
        characterIcon = Instantiate(minimapIconPrefab, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)), iconsParent);
        characterIcon.name = name + " Character Icon";
        characterIcon.sprite = characterInfo.characterIcon;
        iconBackground = characterIcon.transform.Find("Icon Background").GetComponent<SpriteRenderer>();
        iconBackground.color = characterInfo.characterColour;
        iconBorder = characterIcon.transform.Find("Icon Border").GetComponent<SpriteRenderer>();
        iconItStateOutline = characterIcon.transform.Find("Icon It State Outline").GetComponent<SpriteRenderer>();
        itCharacterTracker = FindFirstObjectByType<ITCharacterTracker>();
        if (itCharacterTracker.ITCharacters.Contains(transform)) {
            iconItStateOutline.color = Color.red;
        } else {
            iconItStateOutline.color = Color.green;
        }

        int sortingLayerOffset = Random.Range(0, 100) * 4;
        characterIcon.sortingOrder = sortingLayerOffset + 3;
        iconBackground.sortingOrder = sortingLayerOffset + 2;
        iconBorder.sortingOrder = sortingLayerOffset + 1;
        iconItStateOutline.sortingOrder = sortingLayerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        characterIcon.transform.position = new Vector3(transform.position.x, minimapIconOffset, transform.position.z);

        if (itCharacterTracker.ITCharacters.Contains(transform)) {
            iconItStateOutline.color = Color.red;
        }
        else {
            iconItStateOutline.color = Color.green;
        }
    }
}
