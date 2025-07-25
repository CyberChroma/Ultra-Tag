using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItState : MonoBehaviour
{
    public float waitToTagTime;

    private bool canTag;
    private SkinnedMeshRenderer[] meshRenderers;
    private CharacterDisable characterDisable;

    void Start()
    {
        meshRenderers = transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        canTag = ITCharacterTracker.ITCharacters.Contains(transform);
        characterDisable = GetComponentInParent<CharacterDisable>();
        ChangeOutline();
    }

    public void AttemptTag(Transform other)
    {
        if (canTag)
        {
            CharacterItState otherCharacterItState = other.GetComponent<CharacterItState>();
            if (otherCharacterItState != null)
            {
                canTag = false;
                ITCharacterTracker.ITCharacters.Remove(transform);
                ITCharacterTracker.ITCharacters.Add(otherCharacterItState.transform);
                ChangeOutline();
                otherCharacterItState.ChangeOutline();
                StartCoroutine(otherCharacterItState.WaitToTag());
            }
        }
    }

    public void ChangeOutline()
    {
        bool changeToRed = ITCharacterTracker.ITCharacters.Contains(transform);
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers) {
            if (changeToRed) {
                meshRenderer.material.SetColor("_Outline_Colour", Color.red);
            } else {
                meshRenderer.material.SetColor("_Outline_Colour", Color.green);
            }
        }
    }

    public IEnumerator WaitToTag()
    {
        characterDisable.DeactivateCharacter();
        yield return new WaitForSeconds(waitToTagTime);
        characterDisable.ActivateCharacter();
        canTag = true;
    }
}
