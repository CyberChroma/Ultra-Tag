using UnityEngine;

public class CameraTurn : MonoBehaviour
{
    public float turnSpeed = 1f;

    public void Turn(float vertical)
    {
        return;
        float rotationAmount = vertical * turnSpeed * 500f * Time.deltaTime;
        transform.Rotate(Vector3.right * rotationAmount);

        float xAngle = transform.localEulerAngles.x;

        // Clamp between 0–80 (looking up) and 280–360 (looking down past 360 wrap)
        if (xAngle > 180f)
        {
            xAngle = Mathf.Clamp(xAngle, 280f, 360f);
        }
        else
        {
            xAngle = Mathf.Clamp(xAngle, 0f, 80f);
        }

        transform.localRotation = Quaternion.Euler(xAngle, 0f, 0f);
    }
}
