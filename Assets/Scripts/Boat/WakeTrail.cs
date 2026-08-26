using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class WakeTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    public float noiseScale = 1f;
    public float noiseStrength = 0.1f;
    public float noiseSpeed = 1f;

    private TrailRenderer trailRenderer;
    private Rigidbody boatRigidbody;
    private float noiseOffset;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        boatRigidbody = GetComponentInParent<Rigidbody>();
        noiseOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        if (boatRigidbody == null) return;

        float speed = boatRigidbody.linearVelocity.magnitude;

        // Scale trail width with boat speed
        trailRenderer.startWidth = Mathf.Lerp(0f, 1f, speed / 12f);

        // Apply Perlin noise to trail width for organic feel
        float noise = Mathf.PerlinNoise(
            Time.time * noiseSpeed + noiseOffset,
            noiseScale
        );

        trailRenderer.startWidth += noise * noiseStrength;

        // Only emit trail when moving
        trailRenderer.emitting = speed > 0.5f;
    }
}