using UnityEngine;

public class AIAttemptTag : MonoBehaviour
{
    public float interactDistance = 2f;
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

        TryTagPlayer();
    }

    private void TryTagPlayer()
    {
        Collider[] nearbyAgents = Physics.OverlapSphere(transform.position, interactDistance, agentMask);

        for (int i = 0; i < nearbyAgents.Length; i++)
        {
            Transform other = nearbyAgents[i].transform;
            if (other == transform.parent)
                continue;

            CharacterTag otherTag = other.GetComponent<CharacterTag>();
            if (otherTag == null || otherTag.IsHunter)
                continue;

            Vector3 toOther = (other.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toOther);

            if (angle > fieldOfViewAngle * 0.5f)
                continue;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance))
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);

        // Show FOV cone
        Vector3 left = Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0) * transform.forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, left * interactDistance);
        Gizmos.DrawRay(transform.position, right * interactDistance);
    }
}
