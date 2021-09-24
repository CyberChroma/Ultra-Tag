using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITriggerInteractionInput : MonoBehaviour
{
    private bool jumpDown;
    private bool jumpHeld;
    private bool canPressJump;

    private CharacterJump characterJump;

    // Start is called before the first frame update
    void Start()
    {
        canPressJump = true;
        characterJump = GetComponent<CharacterJump>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpInput();
    }

    void JumpInput()
    {
        characterJump.Jump(jumpDown, jumpHeld);
    }

    private void OnTriggerStay(Collider other)
    {
        if (canPressJump && other.CompareTag("Jump Trigger")) {
            JumpTrigger jumpTrigger = other.GetComponent<JumpTrigger>();
            StartCoroutine(HoldJump(jumpTrigger.jumpTime));
            StartCoroutine(WaitToJumpAgain(jumpTrigger.jumpAgainTime));
        }
    }

    IEnumerator HoldJump (float jumpTime)
    {
        jumpDown = true;
        jumpHeld = true;
        yield return null;
        jumpDown = false;
        yield return new WaitForSeconds(jumpTime);
        jumpHeld = false;
    }

    IEnumerator WaitToJumpAgain (float jumpAgainTime)
    {
        canPressJump = false;
        yield return new WaitForSeconds(jumpAgainTime);
        canPressJump = true;
    }
}
