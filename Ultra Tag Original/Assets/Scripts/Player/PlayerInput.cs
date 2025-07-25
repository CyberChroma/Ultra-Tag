using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;

    public KeyCode jumpKey;

    private bool front;
    private bool back;
    private bool left;
    private bool right;

    private float mouseHorizontal;
    private float mouseVertical;

    private bool jumpDown;
    private bool jumpHeld;

    private CharacterMove characterMove;
    private CharacterTurn characterTurn;
    private CameraTurn cameraTurn;
    private CharacterJump characterJump;

    // Start is called before the first frame update
    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
        characterTurn = GetComponent<CharacterTurn>();
        cameraTurn = GetComponentInChildren<CameraTurn>();
        characterJump = GetComponent<CharacterJump>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput();
        TurnInput();
        JumpInput();
    }

    void MoveInput()
    {
        if (Keyboard.current[Key.W].isPressed) {
            front = true;
        }
        else {
            front = false;
        }
        if (Keyboard.current[Key.S].isPressed) {
            back = true;
        }
        else {
            back = false;
        }
        if (Keyboard.current[Key.A].isPressed) {
            left = true;
        }
        else {
            left = false;
        }
        if (Keyboard.current[Key.D].isPressed) {
            right = true;
        }
        else {
            right = false;
        }
        characterMove.Move(front, back, left, right);
    }

    void TurnInput()
    {
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = -Input.GetAxis("Mouse Y");
        characterTurn.Turn(mouseHorizontal);
        cameraTurn.Turn(mouseVertical);
    }

    void JumpInput()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame) {
            jumpDown = true;
        }
        else {
            jumpDown = false;
        }
        if (Keyboard.current[Key.Space].isPressed) {
            jumpHeld = true;
        }
        else {
            jumpHeld = false;
        }
        characterJump.Jump(jumpDown, jumpHeld);
    }
}
