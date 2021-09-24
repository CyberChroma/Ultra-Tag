using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHide : MonoBehaviour
{

    // Start is called before the first frame update
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
