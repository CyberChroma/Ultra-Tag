using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed;

    private int fbDir = 0;
    private int lrDir = 0;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector3(lrDir, 0, fbDir) * moveSpeed, ForceMode.Impulse);
        fbDir = 0;
        lrDir = 0;
    }

    public void Move(bool inputFront, bool inputBack, bool inputLeft, bool inputRight)
    {
        if (inputBack) {
            fbDir = -1;
        } else if (inputFront) {
            fbDir = 1;
        } else {
            fbDir = 0;
        }

        if (inputLeft) {
            lrDir = -1;
        }
        else if (inputRight) {
            lrDir = 1;
        }
        else {
            lrDir = 0;
        }
    }
}
