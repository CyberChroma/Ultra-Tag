using UnityEngine;

public class CoinSpinAndHover : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float hoverAmplitude = 0.25f;
    [SerializeField] private float hoverFrequency = 2f;

    private Vector3 baseLocalPosition;

    private void Start()
    {
        baseLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Spin
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Hover relative to parent
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.localPosition = baseLocalPosition + -Vector3.forward * hoverOffset;
    }
}
