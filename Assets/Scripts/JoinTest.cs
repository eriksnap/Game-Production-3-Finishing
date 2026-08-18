using UnityEngine;
using UnityEngine.InputSystem;

public class JoinTest : MonoBehaviour
{
    private void Update()
    {
        if (Gamepad.current == null)
        {
            Debug.Log("No gamepad found");
            return;
        }

        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Cross/A pressed detected");
        }
    }
}