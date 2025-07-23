using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItState : MonoBehaviour
{
    public float waitToTagTime;

    private bool canTag;
    private SkinnedMeshRenderer[] meshRenderers;
    private CharacterDisable characterDisable;
    private ITCharacterTracker itCharacterTracker;

    void Start()
    {
        meshRenderers = transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        itCharacterTracker = FindFirstObjectByType<ITCharacterTracker>();
        canTag = itCharacterTracker.ITCharacters.Contains(transform.parent);
        characterDisable = GetComponentInParent<CharacterDisable>();
        ChangeOutline();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent && itCharacterTracker.ITCharacters.Contains(transform.parent) && !itCharacterTracker.ITCharacters.Contains(other.transform.parent) && canTag) {
            CharacterItState otherCharacterItState = other.GetComponent<CharacterItState>();
            if (otherCharacterItState != null) {
                canTag = false;
                itCharacterTracker.ITCharacters.Remove(transform.parent);
                itCharacterTracker.ITCharacters.Add(otherCharacterItState.transform.parent);
                ChangeOutline();
                otherCharacterItState.ChangeOutline();
                StartCoroutine(otherCharacterItState.WaitToTag());
            }
        }
    }

    public void ChangeOutline()
    {
        bool changeToRed = itCharacterTracker.ITCharacters.Contains(transform.parent);
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers) {
            if (changeToRed) {
                meshRenderer.material.SetColor("_Outline_Colour", Color.red);
            } else {
                meshRenderer.material.SetColor("_Outline_Colour", Color.green);
            }
        }
    }

    public IEnumerator WaitToTag ()
    {
        characterDisable.DeactivateCharacter();
        yield return new WaitForSeconds(waitToTagTime);
        characterDisable.ActivateCharacter();
        canTag = true;
    }
}
