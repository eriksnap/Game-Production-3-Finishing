using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Boat")) return;

        BoatHealth health = other.GetComponent<BoatHealth>();
        if (health == null)
            health = other.GetComponentInParent<BoatHealth>();

        health?.EliminateByBoundary();
    }
}