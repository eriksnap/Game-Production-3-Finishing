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

        Rigidbody myRb = GetComponent<Rigidbody>();
        Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRb == null) return;

        float mySpeed = myRb.linearVelocity.magnitude;
        float otherSpeed = otherRb.linearVelocity.magnitude;

        if (mySpeed >= otherSpeed) return;

        float damage = otherSpeed * damageMultiplier;
        if (damage < minImpactForce * damageMultiplier) return;

        //Play collision VFX at the contact point
        VFXManager.Instance?.PlayCollisionSplash(collision.contacts[0].point);

        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        //Update the HUD
        if (playerIndex >= 0)
            FindAnyObjectByType<GameHUD>()?.UpdateHP(playerIndex, currentHP, maxHP);

        if (currentHP <= 0f)
            Eliminate();
    }

    private void Eliminate()
    {
        if (isEliminated) return;
        isEliminated = true;

        //Play death VFX
        VFXManager.Instance?.PlayDeathSplash(transform.position);

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