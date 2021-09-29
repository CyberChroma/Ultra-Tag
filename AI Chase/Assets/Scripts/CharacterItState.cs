using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItState : MonoBehaviour
{
    public float waitToTagTime;

    private bool canTag;
    private ITCharacterTracker itCharacterTracker;
    private CharacterDisable characterDisable;

    void Start()
    {
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
        canTag = itCharacterTracker.ITCharacters.Contains(transform.parent);
        characterDisable = GetComponentInParent<CharacterDisable>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent && itCharacterTracker.ITCharacters.Contains(transform.parent) && !itCharacterTracker.ITCharacters.Contains(other.transform.parent) && canTag) {
            CharacterItState otherCharacterItState = other.GetComponent<CharacterItState>();
            if (otherCharacterItState != null) {
                canTag = false;
                itCharacterTracker.ITCharacters.Remove(transform.parent);
                itCharacterTracker.ITCharacters.Add(otherCharacterItState.transform.parent);
                StartCoroutine(otherCharacterItState.WaitToTag());
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
