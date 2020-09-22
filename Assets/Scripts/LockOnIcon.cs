using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIcon : MonoBehaviour
{
    #region //exposed
    [HideInInspector] public Transform FollowTarget = null;
    [HideInInspector] public bool MissileLaunched = false;
    [HideInInspector] public HomingMissile Missile = null;
    #endregion

    #region //Awake
    private PooledObject m_pooled = null;
    #endregion


    private void Awake()
    {
        m_pooled = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, -20, 10); //offscreen
    }

    void Update()
    {
        if ((MissileLaunched && !Missile.gameObject.activeSelf) || !FollowTarget.gameObject.activeSelf)
        {
            MissileLaunched = false;
            m_pooled.ReturnToPool();
        }
        else 
        {
            transform.position = FollowTarget.position;
        }         
    }
}
