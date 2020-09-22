using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HomingMissileWeapon : MonoBehaviour, IWeapon
{
    #region //inspector
    [SerializeField] private PooledObject MissilePrefab = null;
    [SerializeField] private PooledObject LockOnIconPrefab = null;
    [SerializeField] private float LockOnDistance = 15f;
    [SerializeField] private float LaunchImpulse = 0.5f;
    [SerializeField] private Material BeamMaterial = null;
    [SerializeField] private Color BeamColor = Color.white;
    #endregion

    #region //internal
    private enum EWeaponState
    {
        neutral,
        locking,
        firing
    }
    private EWeaponState m_weaponState = EWeaponState.neutral;
    private HashSet<Transform> m_lockOns = new HashSet<Transform>();
    private Dictionary<Transform, LockOnIcon> m_lockOnIcons =
            new Dictionary<Transform, LockOnIcon>();
    private Coroutine m_lockingCoroutine;
    private SimplePool m_missilePool = null;
    private SimplePool m_lockOnIconPool = null;
    #endregion

    #region //Awake
    private LineRenderer m_lineRenderer = null;
    #endregion

    public string Name => "Homing Missiles";

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.widthMultiplier = 0f;

        if (MissilePrefab != null)
        {
            m_missilePool = new SimplePool(10, MissilePrefab);
        }
    }

    private void OnEnable()
    {
        m_lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, 15f, 0f) });
        m_lineRenderer.widthMultiplier = 0f;

        m_lineRenderer.material = BeamMaterial;
        m_lineRenderer.startColor = BeamColor;
        m_lineRenderer.endColor = BeamColor;
    }

    private void OnDisable()
    {
        m_lineRenderer.widthMultiplier = 0f;
    }

    public void PullTrigger()
    {
        if (m_weaponState == EWeaponState.neutral)
        {
            m_weaponState = EWeaponState.locking;
            m_lineRenderer.widthMultiplier = 0.15f;
            m_lockingCoroutine = StartCoroutine(Locking());
        }
    }

    public void ReleaseTrigger()
    {
        if (m_weaponState == EWeaponState.locking)
        {
            m_weaponState = EWeaponState.firing;
            m_lineRenderer.widthMultiplier = 0.0f;
            if (m_lockingCoroutine != null)
                StopCoroutine(m_lockingCoroutine);
            StartCoroutine(Firing());
        }
    }

    private IEnumerator Locking()
    {
        RaycastHit2D[] hits;
        LayerMask layer = 1 << LayerMask.NameToLayer("Enemies");
        while (m_weaponState == EWeaponState.locking)
        {
            hits = Physics2D.RaycastAll(transform.position, Vector2.up, LockOnDistance, layer);
            foreach (var item in hits)
            {
                if (!m_lockOns.Contains(item.transform))
                {
                    m_lockOns.Add(item.transform);
                    var icon = m_lockOnIconPool.GetFromPool().GetComponent<LockOnIcon>();
                    icon.FollowTarget = item.transform;
                    m_lockOnIcons.Add(item.transform, icon);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Firing()
    {
        float fireDelay = 0.15f; //todo field
        foreach (Transform target in m_lockOns)
        {
            var homingMissile = m_missilePool.GetFromPool().GetComponent<HomingMissile>();
            homingMissile.TargetLock = target;
            m_lockOnIcons[target].MissileLaunched = true;
            m_lockOnIcons[target].Missile = homingMissile;

            Vector2 randomSpawnDirection = Random.insideUnitCircle.normalized;
            randomSpawnDirection = new Vector2(randomSpawnDirection.x, Mathf.Abs(randomSpawnDirection.y));
            homingMissile.transform.position = transform.position + (Vector3)randomSpawnDirection * 0.5f;

            homingMissile.Body.AddForce(randomSpawnDirection * LaunchImpulse, ForceMode2D.Impulse);

            homingMissile.transform.rotation = Quaternion.LookRotation(
                Vector3.back, target.position - homingMissile.transform.position);
            homingMissile.StartChase();

            yield return new WaitForSeconds(fireDelay);
        }
        m_lockOns.Clear();
        m_lockOnIcons.Clear();
        m_weaponState = EWeaponState.neutral;
    }

    public void Init(PooledObject prefab, float lockOnDistance, float launchImpulse,
        Material beamMaterial, Color beamColor, PooledObject lockOnIcon)
    {
        MissilePrefab = prefab;
        m_missilePool = new SimplePool(10, MissilePrefab);
        this.LockOnIconPrefab = lockOnIcon;
        m_lockOnIconPool = new SimplePool(10, this.LockOnIconPrefab);

        this.LockOnDistance = lockOnDistance;
        this.LaunchImpulse = launchImpulse;

        m_lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, 15f, 0f) });
        m_lineRenderer.useWorldSpace = false;

        m_lineRenderer.material = beamMaterial;
        m_lineRenderer.startColor = beamColor;
        m_lineRenderer.endColor = beamColor;

        this.BeamMaterial = beamMaterial;
        this.BeamColor = beamColor;
    }

}
