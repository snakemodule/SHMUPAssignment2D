using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// `PullTrigger` continously calls a shoot callback from a coroutine until `ReleaseTrigger` 
/// is called. To use this, derived classes should set the shootCallback in
/// their Awake methods.
/// </summary>
public abstract class ContinuousFireWeapon : MonoBehaviour, IWeapon
{
    #region //inspector
    [SerializeField] protected float ShotsPerSecond = 5;
    #endregion


    #region //internal
    private IEnumerator m_firing = null;
    private float m_lastShotTime = 0;
    protected Action m_shootCallback;
    #endregion

    public abstract string Name { get; }

    public void PullTrigger()
    {
        if (m_firing != null)
        {
            StopCoroutine(m_firing);
        }
        m_firing = ContinuousFire(m_shootCallback);
        StartCoroutine(m_firing);
    }

    public void ReleaseTrigger()
    {
        StopCoroutine(m_firing);
    }

    private IEnumerator ContinuousFire(Action shoot)
    {
        float shotDelay = 1 / ShotsPerSecond;
        while (isActiveAndEnabled)//todo is this correct?
        {
            yield return null;
            float t = Time.time - m_lastShotTime;
            if (Time.time - m_lastShotTime >= shotDelay)
            {
                m_lastShotTime = Time.time;
                m_shootCallback();
            }
        }
    }

    protected abstract void Shoot();
}

