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

    private bool canTag;
    private PlayerInput playerInput;
    private AIPathfindingInput aiPathfindingInput;
    private Animator anim;

    public bool IsHunter => role == CharacterRole.Hunter;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        aiPathfindingInput = GetComponent<AIPathfindingInput>();
        anim = GetComponentInChildren<Animator>();
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
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
        if (aiPathfindingInput != null)
        {
            aiPathfindingInput.enabled = false;
        }
        anim?.SetBool("IsRunning", false);
    }

    public void ActivateCharacter()
    {
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
        if (aiPathfindingInput != null)
        {
            aiPathfindingInput.enabled = true;
        }
    }

    public void SetRole(CharacterRole newRole)
    {
        role = newRole;
        OnRoleChanged.Invoke(newRole);
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