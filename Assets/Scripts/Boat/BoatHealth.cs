using UnityEngine;
using UnityEngine.Events;

public class BoatHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float minImpactForce = 3f;
    public float damageMultiplier = 10f;

    [Header("Events")]
    public UnityEvent onEliminated;

    private float currentHP;
    private bool isEliminated = false;
    private int playerIndex = -1;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isEliminated) return;
        if (!collision.gameObject.CompareTag("Boat")) return;

        float impactForce = collision.relativeVelocity.magnitude;
        if (impactForce < minImpactForce) return;

        float damage = impactForce * damageMultiplier;
        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        // Update HUD
        if (playerIndex >= 0)
            FindAnyObjectByType<GameHUD>()?.UpdateHP(playerIndex, currentHP, maxHP);

        if (currentHP <= 0f)
            Eliminate();
    }

    private void Eliminate()
    {
        if (isEliminated) return;
        isEliminated = true;

        if (playerIndex >= 0)
            FindAnyObjectByType<GameHUD>()?.ShowEliminatedOnHUD(playerIndex);

        GetComponent<BoatController>()?.SetEliminated();
        onEliminated?.Invoke();
    }

    public void EliminateByBoundary()
    {
        Eliminate();
    }

    public float GetCurrentHP() => currentHP;
    public float GetMaxHP() => maxHP;
    public bool IsEliminated() => isEliminated;
}