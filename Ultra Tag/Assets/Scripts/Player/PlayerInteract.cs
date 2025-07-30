using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 4f;
    public float fieldOfViewAngle = 60f;
    public LayerMask agentMask;

    private CharacterTag characterTag;

    private void Start()
    {
        characterTag = GetComponentInParent<CharacterTag>();
    }

    private void Update()
    {
        if (!characterTag.IsHunter)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryTagNearbyAgent();
        }
    }

    private void TryTagNearbyAgent()
    {
        Vector3 origin = characterTag.transform.position;
        Vector3 forward = characterTag.transform.forward;

        Collider[] nearbyAgents = Physics.OverlapSphere(origin, interactDistance, agentMask);

        for (int i = 0; i < nearbyAgents.Length; i++)
        {
            Transform other = nearbyAgents[i].transform;
            if (other == characterTag.transform)
                continue;

            CharacterTag otherTag = other.GetComponent<CharacterTag>();
            if (otherTag == null || otherTag.IsHunter)
                continue;

            Vector3 toOther = (other.position - origin).normalized;
            float angle = Vector3.Angle(forward, toOther);

            if (angle > fieldOfViewAngle * 0.5f)
                continue;

            if (Physics.Raycast(origin + Vector3.up * 0.5f, forward, out RaycastHit hit, interactDistance))
            {
                if (hit.transform == other || hit.transform.root == other)
                {
                    characterTag.AttemptTag(other);
                    break;
                }
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (characterTag == null)
            return;

        Vector3 origin = characterTag.transform.position;
        Vector3 forward = characterTag.transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, interactDistance);

        Vector3 left = Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0) * forward;
        Vector3 right = Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0) * forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(origin, left * interactDistance);
        Gizmos.DrawRay(origin, right * interactDistance);
    }
}
