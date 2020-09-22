using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(BulletWeapon))]
public class PlayerController : MonoBehaviour
{
    //set in inspector
    #region
    [SerializeField] private Transform m_followTarget = null;
    [SerializeField] private float m_followSpeed = 5.0f;
    [SerializeField] private Ship ship = null;
    #endregion



    private List<IWeapon> weapons = new List<IWeapon>();
    private int weaponsIterator = 0;

    private InputManager m_inputManager;

    Mouse mouseDevice = null;
    bool LMBPressed = false;

    private void Awake()
    {
        weapons.Add(GetComponent<MultiShotWeapon>());
        //weapons.Add(GetComponent<LaserWeapon>());
        //weapons.Add(GetComponent<HomingMissileWeapon>());
    }

    private void OnEnable()
    {
        mouseDevice = InputSystem.GetDevice<Mouse>();
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
        LMBPressed = true;
        weapons[weaponsIterator].PullTrigger();
    }

    private void FireReleased(InputAction.CallbackContext context)
    {
        LMBPressed = false;
        weapons[weaponsIterator].ReleaseTrigger();
    }

    private void ShieldPressed(InputAction.CallbackContext context)
    {
        ship.ActivateShield();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            m_followTarget.position, m_followSpeed * Time.deltaTime);

        if (!LMBPressed && weapons.Count > 1)
        {
            float scrollValue = mouseDevice.scroll.y.ReadValue();
            (weapons[weaponsIterator] as MonoBehaviour).enabled = false;
            if (scrollValue > 0)
            {
                
                if (++weaponsIterator > weapons.Count - 1)
                    weaponsIterator = 0;
            }
            else if (scrollValue < 0)
            {
                if (--weaponsIterator < 0)
                    weaponsIterator = weapons.Count - 1;
            }
            (weapons[weaponsIterator] as MonoBehaviour).enabled = true;
        }

    }

    public T AddWeapon<T>() where T : UnityEngine.Component, IWeapon
    {
        T newWeaponComponent = gameObject.AddComponent<T>();
        weapons.Add(newWeaponComponent);
        //set weapons iterator

        return newWeaponComponent;
    }

}
