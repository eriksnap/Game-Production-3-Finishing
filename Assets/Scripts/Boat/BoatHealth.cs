using UnityEngine;
using UnityEngine.Events;

public class BoatHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float minImpactForce = 3f; // impacts below this are ignored
    public float damageMultiplier = 10f;

    [Header("Events")]
    public UnityEvent onEliminated;

    private float currentHP;
    private bool isEliminated = false;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isEliminated) return;

        //Only take damage from other boats
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

        if (currentHP <= 0f)
        {
            Eliminate();
        }
    }

    private void Eliminate()
    {
        if (isEliminated) return;
        isEliminated = true;

        GetComponent<BoatController>()?.SetEliminated();
        onEliminated?.Invoke();
    }

    // Called externally by the out-of-bounds trigger
    public void EliminateByBoundary()
    {
        Eliminate();
    }

    public float GetCurrentHP() => currentHP;
    public float GetMaxHP() => maxHP;
    public bool IsEliminated() => isEliminated;
}
