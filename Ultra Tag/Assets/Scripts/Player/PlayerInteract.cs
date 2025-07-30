using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 4;

    private CharacterTag characterTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterTag = GetComponentInParent<CharacterTag>();
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
                    // If I'm a hunter, and agent I'm grabbing is an evader...
                    if (characterTag.IsHunter && !hit.transform.GetComponentInParent<CharacterTag>().IsHunter!) {
                        // DO THE GRAB
                        print("TAGGED YA!!");
                        characterTag.AttemptTag(hit.transform);
                    }
                }
            }
        }
    }
}
