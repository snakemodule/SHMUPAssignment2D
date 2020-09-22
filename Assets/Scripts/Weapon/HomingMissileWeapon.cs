using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HomingMissileWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private PooledObject missilePrefab = null;
    [SerializeField] private float lockOnDistance = 15f;
    private enum EWeaponState
    {
        neutral,
        locking,
        firing
    }
    private EWeaponState weaponState = EWeaponState.neutral;

    private HashSet<Transform> lockOns = new HashSet<Transform>();
    private LineRenderer lineRenderer = null;

    private Coroutine lockingCoroutine;

    private SimplePool missilePool = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0f;

        missilePool = new SimplePool(10, missilePrefab);
    }

    private void OnEnable()
    {
        lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, 15f, 0f) });
        lineRenderer.widthMultiplier = 0f;
    }

    private void OnDisable()
    {
        lineRenderer.widthMultiplier = 0f;
    }

    public void PullTrigger()
    {
        if (weaponState == EWeaponState.neutral)
        {
            weaponState = EWeaponState.locking;
            lineRenderer.widthMultiplier = 0.15f;
            lockingCoroutine = StartCoroutine(locking());
        }
    }

    public void ReleaseTrigger()
    {
        if (weaponState == EWeaponState.locking)
        {
            weaponState = EWeaponState.firing;
            lineRenderer.widthMultiplier = 0.0f;
            if (lockingCoroutine != null)
                StopCoroutine(lockingCoroutine);
            StartCoroutine(firing());
        }
    }

    private IEnumerator locking()
    {
        RaycastHit2D[] hits;
        LayerMask layer = 1 << LayerMask.NameToLayer("Enemies");
        while (weaponState == EWeaponState.locking)
        {
            hits = Physics2D.RaycastAll(transform.position, Vector2.up, lockOnDistance, layer);
            foreach (var item in hits)
            {
                lockOns.Add(item.transform);
            }
            yield return null;
        }
    }

    private IEnumerator firing()
    {
        float fireDelay = 0.15f; //todo field
        foreach (Transform targetTransform in lockOns)
        {            
            var homing = missilePool.getFromPool().GetComponent<HomingMissile>();
            homing.TargetLock = targetTransform;
            StartCoroutine(homing.ChaseTargetLock());
            
            yield return new WaitForSeconds(fireDelay);
        }
        lockOns.Clear();
        weaponState = EWeaponState.neutral;
    }
}
