using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 2;

    private int fbDir = 0;
    private int lrDir = 0;
    private Rigidbody rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(lrDir, 0, fbDir) * moveSpeed;
        rb.AddRelativeForce(movement, ForceMode.Impulse);

        fbDir = 0;
        lrDir = 0;

        if (rb.linearVelocity.magnitude <= 1f)
        {
            anim?.SetBool("IsRunning", false);
        }
        else
        {
            anim?.SetBool("IsRunning", true);
        }
    }

    public void Move(bool inputFront, bool inputBack, bool inputLeft, bool inputRight)
    {
        fbDir = inputBack ? -1 : (inputFront ? 1 : 0);
        lrDir = inputLeft ? -1 : (inputRight ? 1 : 0);
    }
}
