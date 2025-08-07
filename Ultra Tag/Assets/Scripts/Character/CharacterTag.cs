using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterRole
{
    Hunter,
    Evader
}

public class CharacterTag : MonoBehaviour
{
    [SerializeField] private CharacterRole role;

    public float waitToTagTime;

    [HideInInspector]
    public UnityEvent<CharacterRole> OnRoleChanged = new UnityEvent<CharacterRole>();
    public static event System.Action<Transform, Transform> OnTagged;
    public static event System.Action<bool> OnPlayerRoleChanged;

    private bool canTag;
    private bool isPlayer;
    private PlayerInput playerInput;
    private AIPathfindingNavMesh aiPathfinding;
    private PlayerInteract playerInteract;
    private AIAttemptTag aiAttemptTag;
    private Animator anim;

    public bool IsHunter => role == CharacterRole.Hunter;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        aiPathfinding = GetComponent<AIPathfindingNavMesh>();
        anim = GetComponentInChildren<Animator>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        aiAttemptTag = GetComponentInChildren<AIAttemptTag>();
        isPlayer = playerInput != null;
        canTag = IsHunter;
    }

    public void AttemptTag(Transform other)
    {
        if (canTag)
        {
            CharacterTag otherCharacterTag = other.GetComponent<CharacterTag>();
            if (otherCharacterTag != null)
            {
                canTag = false;
                TagCharacter(this, otherCharacterTag);
            }
        }
    }

    public IEnumerator Stun()
    {
        DeactivateCharacter();
        yield return new WaitForSeconds(waitToTagTime);
        ActivateCharacter();
        canTag = true;
    }

    public void DeactivateCharacter()
    {
        if (isPlayer)
        {
            if (playerInput != null)
                playerInput.enabled = false;
            if (playerInteract != null)
                playerInteract.enabled = false;
        }
        else
        {
            if (aiPathfinding != null)
                aiPathfinding.enabled = false;
            if (aiAttemptTag != null)
                aiAttemptTag.enabled = false;
        }

        anim?.SetBool("IsRunning", false);
    }

    public void ActivateCharacter()
    {
        if (isPlayer)
        {
            if (playerInput != null)
                playerInput.enabled = true;
            if (playerInteract != null)
                playerInteract.enabled = true;
        }
        else
        {
            if (aiPathfinding != null)
                aiPathfinding.enabled = true;
            if (aiAttemptTag != null)
                aiAttemptTag.enabled = true;
        }
    }

    public void SetRole(CharacterRole newRole)
    {
        role = newRole;
        OnRoleChanged.Invoke(newRole);

        if (isPlayer)
        {
            OnPlayerRoleChanged?.Invoke(IsHunter);
        }
    }

    public static void TagCharacter(CharacterTag hunter, CharacterTag target)
    {
        hunter.SetRole(CharacterRole.Evader);
        target.SetRole(CharacterRole.Hunter);

        hunter.canTag = false;
        target.StartCoroutine(target.Stun());

        OnTagged?.Invoke(hunter.transform, target.transform);
    }
}
