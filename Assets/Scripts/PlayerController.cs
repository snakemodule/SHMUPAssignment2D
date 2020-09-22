using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(BulletWeapon))]
public class PlayerController : MonoBehaviour
{
    //set in inspector
    #region
    //[SerializeField] private InputManager m_inputManager;
    [SerializeField] private Transform m_followTarget = null;
    [SerializeField] private float m_followSpeed = 5.0f;
    #endregion


    private List<IWeapon> weapons = new List<IWeapon>();
    private int weaponsIterator = 0;

    private InputManager m_inputManager;

    private void Awake()
    {
        weapons.Add(GetComponent<HomingMissileWeapon>());
    }

    private void Start()
    {
        m_inputManager = InputManager.Instance;
        m_inputManager.FireRegister(FirePressed, FireReleased);
    }

    private void FirePressed(InputAction.CallbackContext context)
    {
        weapons[0].PullTrigger();
    }

    private void FireReleased(InputAction.CallbackContext context)
    {
        weapons[0].ReleaseTrigger();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_followTarget.position, m_followSpeed*Time.deltaTime);
    }

}
