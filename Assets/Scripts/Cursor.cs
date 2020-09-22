using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Cursor : MonoBehaviour
{
    private InputManager m_inputManager;    
    private Camera m_mainCamera = null;

    private Transform thisTransform;

    Vector2 mousePosition;

    private void Awake()
    {
        thisTransform = transform;
        m_mainCamera = Camera.main;
    }
    
    private void OnEnable()
    {
        m_inputManager = InputManager.Instance;        
        m_inputManager.MousePositionRegister(updateCursorPosition);
    }

    private void OnDisable()
    {
        m_inputManager.MousePositionUnregister(updateCursorPosition);
    }

    private void Update()
    {        
        if (Application.isFocused && mousePosition.x > 0 && mousePosition.x < Screen.width
            && mousePosition.y > 0 && mousePosition.y < Screen.height)
        {
            thisTransform.position = m_mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1f));
        }
    }


    private void updateCursorPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        //if (Application.isFocused && position.x > 0 && position.x < Screen.width
        //    && position.y > 0 && position.y < Screen.height)
        //{
        //    thisTransform.position = m_mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 1f));
        //}
        //
    }


}
