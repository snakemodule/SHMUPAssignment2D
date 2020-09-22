using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Cursor : MonoBehaviour
{
    private InputManager m_inputManager;    
    private Camera m_mainCamera;

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_inputManager = InputManager.Instance;
        m_mainCamera = MainManager.Instance.MainCamera;
        m_inputManager.MousePositionRegister(updateCursorPosition);
    }

    private void updateCursorPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 position = context.ReadValue<Vector2>();
        if (Application.isFocused && position.x > 0 && position.x < Screen.width
            && position.y > 0 && position.y < Screen.height)
        {
            thisTransform.position = m_mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 1f));
        }

    }


}
