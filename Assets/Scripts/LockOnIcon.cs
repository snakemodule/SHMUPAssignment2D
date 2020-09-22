using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIcon : MonoBehaviour
{
    public Transform followTarget = null;

    public HomingMissile missile = null;

    private PooledObject pooled = null;

    public bool missileLaunched = false;

    private void Awake()
    {
        pooled = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, -20, 10); //offscreen
    }

    void Update()
    {
        if ((missileLaunched && !missile.gameObject.activeSelf) || !followTarget.gameObject.activeSelf)
        {
            missileLaunched = false;
            pooled.returnToPool();
        }
        else 
        {
            transform.position = followTarget.position;
        } 
        
    }
}
