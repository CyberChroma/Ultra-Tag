using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItState : MonoBehaviour
{
    public float waitToTagTime;

    private bool canTag;
    private ITCharacterTracker itCharacterTracker;

    void Start()
    {
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
        canTag = itCharacterTracker.ITCharacter == transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent) {
            if (itCharacterTracker.ITCharacter == transform.parent && canTag) {
                CharacterItState otherCharacterItState = other.GetComponent<CharacterItState>();
                if (otherCharacterItState != null) {
                    canTag = false;
                    itCharacterTracker.ITCharacter = otherCharacterItState.transform.parent;
                    StartCoroutine(otherCharacterItState.WaitToTag());
                }
            }
        }
    }

    public IEnumerator WaitToTag ()
    {
        yield return new WaitForSeconds(waitToTagTime);
        canTag = true;
    }
}
