using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    public Transform poolCenter;
    public float poolRadius = 8f;
    public float checkInterval = 0.2f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        GameObject[] boats = GameObject.FindGameObjectsWithTag("Boat");
        foreach (GameObject boat in boats)
        {
            float distance = Vector2.Distance(
                new Vector2(boat.transform.position.x, boat.transform.position.z),
                new Vector2(poolCenter.position.x, poolCenter.position.z)
            );

            if (distance > poolRadius)
            {
                boat.GetComponent<BoatHealth>()?.EliminateByBoundary();
            }
        }
    }
}