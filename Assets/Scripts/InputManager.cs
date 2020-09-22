using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviorSingleton<InputManager>
{
    private Controls m_controls;
    
    public void MousePositionRegister(Action<InputAction.CallbackContext> callback)
    {
        m_controls.Ship.MousePosition.performed += callback;
    }

    public void MousePositionUnregister(Action<InputAction.CallbackContext> callback)
    {
        m_controls.Ship.MousePosition.performed -= callback;
    }

    public void LMBRegister(Action<InputAction.CallbackContext> press, 
        Action<InputAction.CallbackContext> release)
    {
        m_controls.Ship.LMB.performed += press;
        m_controls.Ship.LMB.canceled += release;
    }

    public void LMBUnregister(Action<InputAction.CallbackContext> press, 
        Action<InputAction.CallbackContext> release)
    {
        m_controls.Ship.LMB.performed -= press;
        m_controls.Ship.LMB.canceled -= release;
    }

    public void RMBRegister(Action<InputAction.CallbackContext> press)
    {
        m_controls.Ship.RMB.performed += press;
    }

    public void RMBUnregister(Action<InputAction.CallbackContext> press)
    {
        m_controls.Ship.RMB.performed -= press;
    }
       

    private void Awake()
    {
        RegisterSingleton();
        m_controls = new Controls();
        m_controls.Ship.LMB.Enable();
        m_controls.Ship.RMB.Enable();
        m_controls.Ship.MousePosition.Enable();
    }
    
}
