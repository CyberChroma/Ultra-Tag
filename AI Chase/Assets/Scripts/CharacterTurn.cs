using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurn : MonoBehaviour
{
    public float turnSpeed;

    private float horizontal = 0;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Turn(float inputHorizontal)
    {
        horizontal = inputHorizontal;
        rb.AddTorque(new Vector3(0, horizontal, 0) * turnSpeed);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
