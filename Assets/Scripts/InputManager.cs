using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviorSingleton<InputManager>
{
    private Controls m_controls;
    private List<Action<InputAction.CallbackContext>> m_mousePositionListeners = new List<Action<InputAction.CallbackContext>>();

    private List<Action<InputAction.CallbackContext>> m_firePressListeners = new List<Action<InputAction.CallbackContext>>();
    private List<Action<InputAction.CallbackContext>> m_fireReleaseListeners = new List<Action<InputAction.CallbackContext>>();

    public void MousePositionRegister(Action<InputAction.CallbackContext> callback)
    {
        m_mousePositionListeners.Add(callback);
        m_controls.Ship.MousePosition.performed += callback;
    }

    public void FireRegister(Action<InputAction.CallbackContext> press, Action<InputAction.CallbackContext> release)
    {
        m_firePressListeners.Add(press);
        m_controls.Ship.Fire.performed += press;

        m_firePressListeners.Add(release);
        m_controls.Ship.Fire.canceled += release;
    }

    private void Awake()
    {
        RegisterSingleton();
        m_controls = new Controls();
        m_controls.Ship.Fire.Enable();
        m_controls.Ship.MousePosition.Enable();
    }

    private void OnDisable()
    {
        //mouse position
        m_controls.Ship.MousePosition.Disable();
        m_mousePositionListeners.ForEach((Action<InputAction.CallbackContext> callback) => 
            m_controls.Ship.MousePosition.performed -= callback);

        //fire button
        m_controls.Ship.Fire.Disable();
        //press
        m_firePressListeners.ForEach((Action<InputAction.CallbackContext> callback) =>
            m_controls.Ship.Fire.performed -= callback);
        //release
        m_fireReleaseListeners.ForEach((Action<InputAction.CallbackContext> callback) =>
            m_controls.Ship.Fire.canceled -= callback);
    }

    private void MouseMoved(InputAction.CallbackContext obj)
    {
        Debug.Log("mouse");
    }
    
    private void FirePerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("test");
    }

    private void test()
    {

    }
}
