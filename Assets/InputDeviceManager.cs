using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceManager : MonoBehaviour
{
    public static bool isUsingGamePad;
    
    private void Awake()
    {
        CheckForGamePads();
        InputSystem.onDeviceChange += CheckDeviceChanged;
    }

    private static void CheckForGamePads()
    {
        var gamePads = Gamepad.all;
        isUsingGamePad = gamePads.Count != 0;
    }

    private static void CheckDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        CheckForGamePads();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange += CheckDeviceChanged;
    }
}
