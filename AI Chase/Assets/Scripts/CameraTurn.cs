using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurn : MonoBehaviour
{
    public float turnSpeed;

    public void Turn(float vertical)
    {
        transform.Rotate(new Vector3(vertical, 0, 0) * turnSpeed * 500 * Time.deltaTime);
        float clampedRot = transform.rotation.eulerAngles.x;
        if (clampedRot > 180) {
            clampedRot = Mathf.Clamp(clampedRot, 275, 360);
        }
        else {
            clampedRot = Mathf.Clamp(clampedRot, 0, 85);
        }
        transform.localRotation = Quaternion.Euler(clampedRot, 0, 0);
    }
}
