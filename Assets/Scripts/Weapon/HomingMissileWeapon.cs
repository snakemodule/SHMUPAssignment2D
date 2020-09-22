using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HomingMissileWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private PooledObject missilePrefab = null;
    [SerializeField] private PooledObject lockOnIcon = null;
    [SerializeField] private float lockOnDistance = 15f;
    [SerializeField] private float launchImpulse = 0.5f;
    [SerializeField] private Material beamMaterial = null;
    [SerializeField] private Color beamColor = Color.white;
    private enum EWeaponState
    {
        neutral,
        locking,
        firing
    }
    private EWeaponState weaponState = EWeaponState.neutral;

    private Dictionary<Transform, PooledObject> lockOns = new Dictionary<Transform, PooledObject>();
    private LineRenderer lineRenderer = null;

    private Coroutine lockingCoroutine;

    private SimplePool missilePool = null;
    private SimplePool lockOnIconPool = null;

    public string Name => "Homing Missiles";

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0f;

        if (missilePrefab != null)
        {
            missilePool = new SimplePool(10, missilePrefab);
        }
    }

    private void OnEnable()
    {
        lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, 15f, 0f) });
        lineRenderer.widthMultiplier = 0f;

        lineRenderer.material = beamMaterial;
        lineRenderer.startColor = beamColor;
        lineRenderer.endColor = beamColor;
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
                if (!lockOns.ContainsKey(item.transform))
                {
                    var icon = lockOnIconPool.getFromPool();
                    icon.GetComponent<LockOnIcon>().followTarget = item.transform;
                    lockOns.Add(item.transform, icon);
                }
            }
            yield return null;
        }
    }

    private IEnumerator firing()
    {
        float fireDelay = 0.15f; //todo field
        foreach (var keyValuePair in lockOns)
        {            
            var homingMissile = missilePool.getFromPool().GetComponent<HomingMissile>();
            homingMissile.TargetLock = keyValuePair.Key;
            homingMissile.LockOnIcon = keyValuePair.Value;

            Vector2 randomSpawnDirection = Random.insideUnitCircle.normalized;
            randomSpawnDirection = new Vector2(randomSpawnDirection.x, Mathf.Abs(randomSpawnDirection.y));
            homingMissile.transform.position = transform.position+(Vector3)randomSpawnDirection * 0.5f;

            homingMissile.Body.AddForce(randomSpawnDirection*launchImpulse, ForceMode2D.Impulse);

            homingMissile.transform.rotation = Quaternion.LookRotation(
                Vector3.back, keyValuePair.Key.position - homingMissile.transform.position);
            StartCoroutine(homingMissile.ChaseTargetLock());
            
            yield return new WaitForSeconds(fireDelay);
        }
        lockOns.Clear();
        weaponState = EWeaponState.neutral;
    }

    public void Init(PooledObject prefab, float lockOnDistance, float launchImpulse, 
        Material beamMaterial, Color beamColor, PooledObject lockOnIcon)
    {
        missilePrefab = prefab;
        missilePool = new SimplePool(10, missilePrefab);
        this.lockOnIcon = lockOnIcon;
        lockOnIconPool = new SimplePool(10, lockOnIcon);

        this.lockOnDistance = lockOnDistance;
        this.launchImpulse = launchImpulse;

        lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, 15f, 0f) });
        lineRenderer.useWorldSpace = false;

        lineRenderer.material = beamMaterial;
        lineRenderer.startColor = beamColor;
        lineRenderer.endColor = beamColor;

        this.beamMaterial = beamMaterial;
        this.beamColor = beamColor;
    }

}
