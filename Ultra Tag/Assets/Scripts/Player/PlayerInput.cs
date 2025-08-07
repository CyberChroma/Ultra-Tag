using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Key moveForwardKey = Key.W;
    public Key moveBackwardKey = Key.S;
    public Key moveLeftKey = Key.A;
    public Key moveRightKey = Key.D;
    public Key jumpKey = Key.Space;
    public Key interactKey = Key.LeftShift;

    private CharacterMove characterMove;
    private CharacterTurn characterTurn;
    private CameraTurn cameraTurn;
    private CharacterJump characterJump;
    private PlayerInteract playerInteract;

    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
        characterTurn = GetComponent<CharacterTurn>();
        cameraTurn = GetComponentInChildren<CameraTurn>();
        characterJump = GetComponent<CharacterJump>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
    }

    void Update()
    {
        HandleMoveInput();
        HandleTurnInput();
        HandleJumpInput();
        HandleInteractInput();
    }

    private void HandleMoveInput()
    {
        bool front = Keyboard.current[moveForwardKey].isPressed;
        bool back = Keyboard.current[moveBackwardKey].isPressed;
        bool left = Keyboard.current[moveLeftKey].isPressed;
        bool right = Keyboard.current[moveRightKey].isPressed;

        characterMove.Move(front, back, left, right);
    }

    private void HandleTurnInput()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = -Input.GetAxis("Mouse Y");

        characterTurn.Turn(mouseHorizontal);
        cameraTurn.Turn(mouseVertical);
    }

    private void HandleJumpInput()
    {
        bool jumpDown = Keyboard.current[jumpKey].wasPressedThisFrame;
        bool jumpHeld = Keyboard.current[jumpKey].isPressed;

        characterJump.Jump(jumpDown, jumpHeld);
    }

    private void HandleInteractInput()
    {
        bool interactDown = Keyboard.current[interactKey].wasPressedThisFrame;
        playerInteract.TryInteract(interactDown);
    }
}
