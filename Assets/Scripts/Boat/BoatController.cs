using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    [Header("Movement")]
    public float thrustForce = 15f;
    public float steerTorque = 8f;
    public float maxSpeed = 12f;

    [Header("Physics Feel")]
    public float dragOnWater = 1.5f;
    public float angularDragOnWater = 3f;
    public float driftFactor = 0.85f;

    private Rigidbody rb;
    private Vector2 inputVector;
    private bool isEliminated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = dragOnWater;
        rb.angularDamping = angularDragOnWater;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    // These are wired manually in the PlayerInput component's Unity Events
    public void OnThrust(InputAction.CallbackContext context)
    {
        inputVector.y = context.ReadValue<float>();
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        inputVector.x = context.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        if (isEliminated) return;
        HandleThrust();
        HandleSteering();
        LimitSpeed();
        ApplyDrift();
    }

    private void HandleThrust()
    {
        if (inputVector.y == 0) return;
        Vector3 force = transform.forward * inputVector.y * thrustForce;
        rb.AddForce(force, ForceMode.Acceleration);
    }

    private void HandleSteering()
    {
        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / 3f);
        float torque = inputVector.x * steerTorque * speedFactor;
        rb.AddTorque(Vector3.up * torque, ForceMode.Acceleration);
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 capped = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(capped.x, rb.linearVelocity.y, capped.z);
        }
    }

    private void ApplyDrift()
    {
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x *= driftFactor;
        rb.linearVelocity = transform.TransformDirection(localVel);
    }

    public void SetEliminated()
    {
        isEliminated = true;
            rb.isKinematic = false; // temporarily ensure it's not kinematic
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // now set kinematic
    }
}