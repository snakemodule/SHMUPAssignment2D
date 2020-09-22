using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserWeapon : ContinuousFireWeapon
{
    #region //inspector
    [SerializeField] private int DamagePerPulse = 3;
    [SerializeField] private float BeamLength = 12f;
    [SerializeField] private float MaxBeamWidth = 0.35f;
    [SerializeField] private float MinBeamWidth = 0.2f;
    [SerializeField] private Material BeamMaterial = null;
    [SerializeField] private Color BeamColor = Color.white;
    #endregion

    #region //Awake
    private LineRenderer m_lineRenderer;
    #endregion

    #region //internal
    private IEnumerator m_beamAnimation;
    #endregion

    public override string Name => "Pulse Laser";

    private void Awake()
    {
        m_shootCallback = Shoot;
        m_lineRenderer = gameObject.GetComponent<LineRenderer>();
        m_lineRenderer.widthMultiplier = 0f;
    }

    private void OnEnable()
    {
        m_lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, BeamLength, 0f) });
        m_lineRenderer.widthMultiplier = 0f;

        m_lineRenderer.material = BeamMaterial;
        m_lineRenderer.startColor = BeamColor;
        m_lineRenderer.endColor = BeamColor;
    }

    private void OnDisable()
    {
        m_lineRenderer.widthMultiplier = 0f;
    }

    protected override void Shoot()
    {
        float delay = 1 / ShotsPerSecond;
        if(m_beamAnimation != null)
            StopCoroutine(m_beamAnimation);
        m_beamAnimation = AnimateBeam(delay);
        StartCoroutine(m_beamAnimation);

        int enemiesLayerMask = 1 << LayerMask.NameToLayer("Enemies");
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, BeamLength, enemiesLayerMask);
        foreach (var item in hits)
        {
            item.transform.GetComponent<Enemy>().Damage(DamagePerPulse);
        }
    }

    private IEnumerator AnimateBeam(float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time <= endTime)
        {
            m_lineRenderer.widthMultiplier = Mathf.Lerp(MinBeamWidth, MaxBeamWidth, 
                Mathf.InverseLerp(endTime, startTime, Time.time));
            yield return null;
        }
        m_lineRenderer.widthMultiplier = 0f;
    }

    public void Init(int damage, float fireRate, float beamLength, 
        float maxWidth, float minWidth, Material beamMaterial, Color beamColor)
    {
        ShotsPerSecond = fireRate;
        DamagePerPulse = damage;
        BeamLength = beamLength;
        MaxBeamWidth = maxWidth;
        MinBeamWidth = minWidth;

        m_lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, BeamLength, 0f) });
        m_lineRenderer.useWorldSpace = false;
        m_lineRenderer.material = beamMaterial;
        m_lineRenderer.startColor = beamColor;
        m_lineRenderer.endColor = beamColor;

        this.BeamMaterial = beamMaterial;
        this.BeamColor = beamColor;
    }


}
