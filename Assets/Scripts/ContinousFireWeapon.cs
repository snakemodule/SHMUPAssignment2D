using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// `PullTrigger` continously calls a shoot callback from a coroutine until `ReleaseTrigger` 
/// is called. To use this, derived classes should set the shootCallback in
/// their Awake methods.
/// </summary>
public abstract class ContinousFireWeapon : MonoBehaviour, IWeapon
{
    public float ShotsPerSecond = 2;
    protected Action shootCallback;

    private IEnumerator firing = null;
    private float lastShotTime = 0;

    public void PullTrigger()
    {
        if (firing != null)
        {
            StopCoroutine(firing);
        }
        firing = ContinuousFire(shootCallback);
        StartCoroutine(firing);
    }

    public void ReleaseTrigger()
    {
        StopCoroutine(firing);
    }

    private IEnumerator ContinuousFire(Action shoot)
    {
        float shotDelay = 1 / ShotsPerSecond;
        while (isActiveAndEnabled)//todo is this correct?
        {
            yield return null;
            float t = Time.time - lastShotTime;
            if (Time.time - lastShotTime >= shotDelay)
            {
                lastShotTime = Time.time;
                shootCallback();
            }
        }
    }

    protected abstract void Shoot();
}

