using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Gamepads list
    public IReadOnlyList<Gamepad> ConnectedGamepads => UnityEngine.InputSystem.Gamepad.all;
    public List<bool> GamepadConnectedStatus = new List<bool>();
    protected override void Awake()
    {
        // Initialize the gamepad connection status list
        for (int i = 0; i < ConnectedGamepads.Count; i++)
        {
            GamepadConnectedStatus.Add(false);
        }
    }

  
}
