using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BeachBallBomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public float countdownDuration = 10f;
    public float explosionRadius = 3f;
    public float explosionDamage = 40f;
    public float respawnDelay = 3f;

    [Header("Danger Zone Visual")]
    public GameObject dangerZoneVisual;   // flat disc child, matched to explosionRadius
    public Color safeColour = new Color(1f, 1f, 0f, 0.35f);
    public Color dangerColour = new Color(1f, 0f, 0f, 0.6f);

    [Header("References")]
    public Renderer ballRenderer;
    public Collider ballCollider;

    private bool armed = false;
    private float timer;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private Renderer dangerZoneRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (dangerZoneVisual != null)
        {
            dangerZoneRenderer = dangerZoneVisual.GetComponent<Renderer>();

            // Scale the disc to match the explosion radius.
            // Default Unity Cylinder primitive has radius 0.5, so scale = radius / 0.5
            float scaleFactor = explosionRadius / 0.5f;
            dangerZoneVisual.transform.localScale = new Vector3(scaleFactor, dangerZoneVisual.transform.localScale.y, scaleFactor);

            dangerZoneVisual.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (armed) return;
        if (!collision.gameObject.CompareTag("Boat")) return;

        Arm();
    }

    private void Arm()
    {
        armed = true;
        timer = countdownDuration;
        dangerZoneVisual?.SetActive(true);
    }

    private void Update()
    {
        if (!armed) return;

        timer -= Time.deltaTime;

        UpdateDangerZoneColour();

        if (timer <= 0f)
        {
            Explode();
        }
    }

    private void LateUpdate()
    {
        if (dangerZoneVisual == null) return;
        dangerZoneVisual.transform.rotation = Quaternion.identity;
    }

    private void UpdateDangerZoneColour()
    {
        if (dangerZoneRenderer == null) return;

        float t = 1f - Mathf.Clamp01(timer / countdownDuration); // 0 at start, 1 at explosion
        Color current = Color.Lerp(safeColour, dangerColour, t);

        // Extra urgency: flash faster in the last 3 seconds
        if (timer <= 3f)
        {
            float flash = Mathf.PingPong(Time.time * 6f, 1f);
            current.a = Mathf.Lerp(0.3f, 0.8f, flash);
        }

        dangerZoneRenderer.material.color = current;
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Boat")) continue;

            BoatHealth boatHealth = hit.GetComponent<BoatHealth>();
            boatHealth?.ApplyExplosionDamage(explosionDamage);
        }

        VFXManager.Instance?.PlayDeathSplash(transform.position); // reuse existing VFX as explosion stand-in

        StartRespawnCycle();
    }

    private void StartRespawnCycle()
    {
        armed = false;
        dangerZoneVisual?.SetActive(false);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Hide the ball itself during the respawn delay
        if (ballRenderer != null) ballRenderer.enabled = false;
        if (ballCollider != null) ballCollider.enabled = false;

        Invoke(nameof(RespawnBall), respawnDelay);
    }

    private void RespawnBall()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        if (ballRenderer != null) ballRenderer.enabled = true;
        if (ballCollider != null) ballCollider.enabled = true;
    }
}