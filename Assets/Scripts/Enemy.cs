using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class Enemy : MonoBehaviour
{
    #region //exposed
    [HideInInspector] public PathNodes MovementPath = null;
    #endregion

    #region //inspector
    [SerializeField] private bool InvertXMovement = false;
    [SerializeField] private bool InvertYMovement = false;
    [SerializeField] private int HP = 5;
    #endregion

    #region //Awake
    private PooledObject m_pooled = null;
    private SpriteRenderer m_sprite = null;
    #endregion

    #region //internal
    private int m_xInvert = 1;
    private int m_yInvert = 1;
    private Coroutine m_damageAnimation;
    private float m_spawnTime = 0;
    #endregion

    private void Awake()
    {
        m_pooled = GetComponent<PooledObject>();
        m_sprite = GetComponent<SpriteRenderer>();
    }   

    private void OnEnable()
    {
        m_xInvert = (InvertXMovement) ? -1 : 1;
        m_yInvert = (InvertYMovement) ? -1 : 1;

        m_spawnTime = Time.time;
        transform.position = new Vector3(0, -20, 10);//offscreen//new Vector2(movementPath.curveX.Evaluate(0)*xInvert, movementPath.curveY.Evaluate(0)*yInvert);
    }
    


    private void Update()
    {
        float animationTime = Time.time - m_spawnTime;
        transform.position = new Vector2(
                MovementPath.curveX.Evaluate(animationTime)*m_xInvert,
                MovementPath.curveY.Evaluate(animationTime)*m_yInvert);
        if (animationTime > MovementPath.duration)
        {
            m_pooled.ReturnToPool();
        }
    }


    public void Damage(int incomingDamage)
    {
        HP -= incomingDamage;
        if (HP <= 0)
            Die();
        else
        {
            if (m_damageAnimation != null)
            {
                StopCoroutine(m_damageAnimation);
            }
            m_damageAnimation = StartCoroutine(AnimateDamageColor(m_sprite));
        }
    }

    private void Die()
    {
        if (m_damageAnimation != null)
        {
            StopCoroutine(m_damageAnimation);
        }
        m_sprite.color = Color.white;
        m_pooled.ReturnToPool();
    }

    private IEnumerator AnimateDamageColor(SpriteRenderer sprite)
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
