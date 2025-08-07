using UnityEngine;

public class CharacterTurn : MonoBehaviour
{
    public float turnSpeed = 8f;

    private float horizontalInput = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (horizontalInput != 0f)
        {
            rb.AddTorque(Vector3.up * horizontalInput * turnSpeed);
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        horizontalInput = 0f;
    }

    public void Turn(float inputHorizontal)
    {
        horizontalInput = inputHorizontal;
    }
}
