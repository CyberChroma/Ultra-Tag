using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public float jumpForce;
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float gravityMultiplier;
    public float fallJumpTime;
    public float justJumpedTime;
    public float jumpStoreTime;

    private bool jumpHeld;
    private bool canJump;
    private bool waitingToNotJump;
    private bool jumpStored;

    private Rigidbody rb;
    private CharacterGroundDetection characterGroundDetection;

    // Start is called before the first frame update
    void Start()
    {
        waitingToNotJump = false;
        jumpStored = false;
        rb = GetComponent<Rigidbody>();
        characterGroundDetection = GetComponentInChildren<CharacterGroundDetection>();
        characterGroundDetection.justJumped = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!characterGroundDetection.isGrounded) {
            if (rb.velocity.y < 0) {
                rb.AddForce(Vector3.down * fallMultiplier * 10);
            }
            else if (!jumpHeld) {
                rb.AddForce(Vector3.down * lowJumpMultiplier * 10);
            }
            rb.AddForce(Vector3.down * gravityMultiplier * 10);
        }

        if (transform.position.y < -100) {
            transform.position = new Vector3(0, 2, -20);
        }
    }

    public void Jump (bool inputJumpDown, bool inputJumpHeld)
    {
        if (characterGroundDetection.isGrounded) {
            canJump = true;
        } else if (!canJump && !waitingToNotJump) {
            waitingToNotJump = true;
            StartCoroutine(WaitToNotJump());
        }
        
        if (inputJumpDown && !jumpStored) {
            jumpStored = true;
            StartCoroutine(StoreJump());
        }

        if (jumpStored && canJump) {
            rb.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);
            StartCoroutine(JustJumped());
            characterGroundDetection.isGrounded = false;
            canJump = false;
            jumpStored = false;
        }

        jumpHeld = inputJumpHeld;
    }

    IEnumerator WaitToNotJump()
    {
        yield return new WaitForSeconds(fallJumpTime);
        if (characterGroundDetection.isGrounded) {
            canJump = false;
        }
        waitingToNotJump = true;
    }

    IEnumerator JustJumped()
    {
        characterGroundDetection.justJumped = true;
        yield return new WaitForSeconds(justJumpedTime);
        characterGroundDetection.justJumped = false;
    }

    IEnumerator StoreJump()
    {
        yield return new WaitForSeconds(jumpStoreTime);
        jumpStored = false;
    }
}
