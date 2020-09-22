using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public PathNodes movementPath = null;

    [SerializeField] private bool invertXMovement = false;
    [SerializeField] private bool invertYMovement = false;

    [SerializeField] private int HP = 5;


    public PooledObject pooled = null;
    public float SpawnTime { get; private set; }
    public Action<Enemy> DeactivateCallback { private get; set; }

    private void Awake()
    {
        pooled = GetComponent<PooledObject>();
    }   

    private void OnEnable()
    {
        SpawnTime = Time.time;        
        transform.position = new Vector2(movementPath.curveX.Evaluate(0), movementPath.curveY.Evaluate(0));
        //this.Update();
    }
    


    private void Update()
    {
        float animationTime = Time.time - SpawnTime;
        transform.position = new Vector2(
                movementPath.curveX.Evaluate(animationTime),
                movementPath.curveY.Evaluate(animationTime));
        if (animationTime > movementPath.duration)
        {
            pooled.returnToPool();
        }
    }


    public void Damage(int incomingDamage)
    {
        HP -= incomingDamage;
        if (HP <= 0)
            Die();
    }

    private void Die()
    {
        pooled.returnToPool();
    }
}
