using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("Particle Prefabs")]
    public GameObject collisionSplashPrefab;
    public GameObject deathSplashPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayCollisionSplash(Vector3 position)
    {
        if (collisionSplashPrefab == null) return;
        Instantiate(collisionSplashPrefab, position, Quaternion.identity);
    }

    public void PlayDeathSplash(Vector3 position)
    {
        if (deathSplashPrefab == null) return;
        Instantiate(deathSplashPrefab, position, Quaternion.identity);
    }
}