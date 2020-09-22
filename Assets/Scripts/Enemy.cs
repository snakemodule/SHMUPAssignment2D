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

    private int xInvert = 1;
    private int yInvert = 1;

    private PooledObject pooled = null;

    private SpriteRenderer sprite = null;
    public float SpawnTime { get; private set; }
    public Action<Enemy> DeactivateCallback { private get; set; }

    private Coroutine damageAnimation;

    private void Awake()
    {
        pooled = GetComponent<PooledObject>();
        sprite = GetComponent<SpriteRenderer>();
    }   

    private void OnEnable()
    {
        xInvert = (invertXMovement) ? -1 : 1;
        yInvert = (invertYMovement) ? -1 : 1;

        SpawnTime = Time.time;
        transform.position = new Vector3(0, -20, 10);//offscreen//new Vector2(movementPath.curveX.Evaluate(0)*xInvert, movementPath.curveY.Evaluate(0)*yInvert);
    }
    


    private void Update()
    {
        float animationTime = Time.time - SpawnTime;
        transform.position = new Vector2(
                movementPath.curveX.Evaluate(animationTime)*xInvert,
                movementPath.curveY.Evaluate(animationTime)*yInvert);
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
        else
        {
            if (damageAnimation != null)
            {
                StopCoroutine(damageAnimation);
            }
            damageAnimation = StartCoroutine(animateDamageColor(sprite));
        }
    }

    private void Die()
    {
        if (damageAnimation != null)
        {
            StopCoroutine(damageAnimation);
        }
        sprite.color = Color.white;
        pooled.returnToPool();
    }

    private IEnumerator animateDamageColor(SpriteRenderer sprite)
    {
        float duration = 0.3f;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            sprite.color = Color.Lerp(Color.red, Color.white, 
                Mathf.InverseLerp(startTime, startTime + duration, Time.time));
            yield return null;
        }
        sprite.color = Color.white;

        
    }

}
