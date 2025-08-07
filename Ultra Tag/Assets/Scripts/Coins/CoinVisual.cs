using UnityEngine;

public class CoinVisual : MonoBehaviour
{
    [SerializeField] private Material evaderMaterial;
    [SerializeField] private Material hunterMaterial;

    private Renderer[] coinRenderers;

    private void OnEnable()
    {
        CharacterTag.OnPlayerRoleChanged += HandlePlayerRoleChange;
    }

    private void OnDisable()
    {
        CharacterTag.OnPlayerRoleChanged -= HandlePlayerRoleChange;
    }

    private void Start()
    {
        coinRenderers = GetComponentsInChildren<Renderer>();

        CharacterTag playerTag = CharacterStateTracker.Instance.player.GetComponent<CharacterTag>();
        if (playerTag != null)
        {
            HandlePlayerRoleChange(playerTag.IsHunter);
        }
    }

    private void HandlePlayerRoleChange(bool isHunter)
    {
        Material targetMaterial = isHunter ? hunterMaterial : evaderMaterial;

        for (int i = 0; i < coinRenderers.Length; i++)
        {
            if (coinRenderers[i] != null)
            {
                coinRenderers[i].material = targetMaterial;
            }
        }
    }
}
