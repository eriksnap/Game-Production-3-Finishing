using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugger : MonoBehaviour
{
    private void Update()
    {
        if (Gamepad.current == null)
        {
            Debug.Log("No gamepad detected");
            return;
        }

        float thrust = -Gamepad.current.leftTrigger.ReadValue() + Gamepad.current.rightTrigger.ReadValue();
        float steer = Gamepad.current.leftStick.x.ReadValue();

        /*if (thrust != 0 || steer != 0)
        {
            Debug.Log($"Thrust: {thrust} | Steer: {steer}");
        } */ //Used to confirm control values for the boat
    }
}