using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisable : MonoBehaviour
{
    private PlayerInput playerInput;
    private AIPathfindingInput aiPathfindingInput;
    private AITriggerInteractionInput aiTriggerInteractionInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        aiPathfindingInput = GetComponent<AIPathfindingInput>();
        aiTriggerInteractionInput = GetComponent<AITriggerInteractionInput>();
    }

    public void DeactivateCharacter ()
    {
        if (playerInput != null) {
            playerInput.enabled = false;
        }
        if (aiPathfindingInput != null) {
            aiPathfindingInput.enabled = false;
        }
        if (aiTriggerInteractionInput != null) {
            aiTriggerInteractionInput.enabled = false;
        }
    }

    public void ActivateCharacter()
    {
        if (playerInput != null) {
            playerInput.enabled = true;
        }
        if (aiPathfindingInput != null) {
            aiPathfindingInput.enabled = true;
        }
        if (aiTriggerInteractionInput != null) {
            aiTriggerInteractionInput.enabled = true;
        }
    }
}
