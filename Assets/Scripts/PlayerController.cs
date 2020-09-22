using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(BulletWeapon))]
public class PlayerController : MonoBehaviour
{

    #region //inspector
    [SerializeField] private Transform FollowTarget = null;
    [SerializeField] private float FollowSpeed = 5.0f;
    [SerializeField] private Ship PlayerShip = null;
    #endregion

    #region //internal
    private List<IWeapon> m_weapons = new List<IWeapon>();
    private int m_weaponsIterator = 0;
    private InputManager m_inputManager;
    private Mouse m_mouseDevice = null;
    private bool m_LMBPressed = false;
    #endregion


    private void Awake()
    {
        m_weapons.Add(GetComponent<MultiShotWeapon>());        
    }

    private void OnEnable()
    {
        m_mouseDevice = InputSystem.GetDevice<Mouse>();
        m_inputManager = InputManager.Instance;
        m_inputManager.LMBRegister(FirePressed, FireReleased);
        m_inputManager.RMBRegister(ShieldPressed);
    }


    private void OnDisable()
    {
        m_inputManager.LMBUnregister(FirePressed, FireReleased);
        m_inputManager.RMBUnregister(ShieldPressed);
    }

    private void FirePressed(InputAction.CallbackContext context)
    {
        m_LMBPressed = true;
        m_weapons[m_weaponsIterator].PullTrigger();
    }

    private void FireReleased(InputAction.CallbackContext context)
    {
        m_LMBPressed = false;
        m_weapons[m_weaponsIterator].ReleaseTrigger();
    }

    private void ShieldPressed(InputAction.CallbackContext context)
    {
        PlayerShip.ActivateShield();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            FollowTarget.position, FollowSpeed * Time.deltaTime);

        if (!m_LMBPressed && m_weapons.Count > 1)
        {
            float scrollValue = m_mouseDevice.scroll.y.ReadValue();
            (m_weapons[m_weaponsIterator] as MonoBehaviour).enabled = false;
            if (scrollValue > 0)
            {
                
                if (++m_weaponsIterator > m_weapons.Count - 1)
                    m_weaponsIterator = 0;
            }
            else if (scrollValue < 0)
            {
                if (--m_weaponsIterator < 0)
                    m_weaponsIterator = m_weapons.Count - 1;
            }
            (m_weapons[m_weaponsIterator] as MonoBehaviour).enabled = true;
        }

    }

    public T AddWeapon<T>() where T : UnityEngine.Component, IWeapon
    {
        T newWeaponComponent = gameObject.AddComponent<T>();
        m_weapons.Add(newWeaponComponent);
        return newWeaponComponent;
    }

}
