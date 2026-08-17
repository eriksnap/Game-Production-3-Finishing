using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LobbyPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Camera")]
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpPressed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Move relative to camera direction
        Vector3 move = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            move = forward * moveInput.y + right * moveInput.x;
        }
        else
        {
            move = new Vector3(moveInput.x, 0, moveInput.y);
        }

        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Rotate to face movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Jumping
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpPressed = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // Wired via PlayerInput Unity Events - Lobby Action Map
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpPressed = true;
    }

    public void OnReady(InputAction.CallbackContext context)
    {
        if (context.performed)
            ReadyManager.Instance?.ToggleReady(this);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            TryInteract();
    }

    private void TryInteract()
    {
        // Sphere cast to find nearby selection stations
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider hit in hits)
        {
            SelectionStation station = hit.GetComponent<SelectionStation>();
            if (station != null)
            {
                station.Interact(this);
                return;
            }
        }
    }

    public int PlayerIndex { get; set; }
    public GameObject SelectedCharacterPrefab { get; set; }
    public GameObject SelectedBoatPrefab { get; set; }
}