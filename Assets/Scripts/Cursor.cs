using UnityEngine;

public class Cursor : MonoBehaviour
{

    #region //Awake
    private Camera m_mainCamera = null;
    private Transform m_transform;
    #endregion

    #region //Enable
    private InputManager m_inputManager;
    #endregion

    #region //internal
    private Vector2 m_mousePosition;
    #endregion

    private void Awake()
    {
        m_transform = transform;
        m_mainCamera = Camera.main;
    }
    
    private void OnEnable()
    {
        m_inputManager = InputManager.Instance;        
        m_inputManager.MousePositionRegister(UpdateCursorPosition);
    }

    private void OnDisable()
    {
        m_inputManager.MousePositionUnregister(UpdateCursorPosition);
    }

    private void Update()
    {        
        if (Application.isFocused && m_mousePosition.x > 0 && m_mousePosition.x < Screen.width
            && m_mousePosition.y > 0 && m_mousePosition.y < Screen.height)
        {
            m_transform.position = m_mainCamera.ScreenToWorldPoint(new Vector3(m_mousePosition.x, m_mousePosition.y, 1f));
        }
    }

    private void UpdateCursorPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_mousePosition = context.ReadValue<Vector2>();        
    }


}
