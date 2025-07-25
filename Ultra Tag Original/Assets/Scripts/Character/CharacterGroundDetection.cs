using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundDetection : MonoBehaviour
{
    public LayerMask jumpableLayers;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool justJumped;

    void OnTriggerStay(Collider other)
    {
        if (!isGrounded && !justJumped && ((1 << other.gameObject.layer) & jumpableLayers.value) != 0) {
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isGrounded && ((1 << other.gameObject.layer) & jumpableLayers.value) != 0) {
            isGrounded = false;
        }
    }
}
