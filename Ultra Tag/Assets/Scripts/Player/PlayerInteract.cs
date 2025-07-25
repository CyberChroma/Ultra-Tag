using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 4;

    private CharacterItState characterItState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterItState = GetComponentInParent<CharacterItState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, Vector3.one * 0.1f, transform.forward, out hit, transform.rotation, interactDistance))
            {
                if (hit.collider.CompareTag("Agent"))
                {
                    // If I'm IT, and agent I'm grabbing is not IT...
                    if (ITCharacterTracker.ITCharacters.Contains(transform.parent) && !ITCharacterTracker.ITCharacters.Contains(hit.transform)) {
                        // DO THE GRAB
                        print("TAGGED YA!!");
                        characterItState.AttemptTag(hit.transform);
                    }
                }
            }
        }
    }
}
