using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaserWeapon : ContinuousFireWeapon
{
    [SerializeField] private int DamagePerPulse = 3;
    [SerializeField] private float BeamLength = 12f;
    [SerializeField] private float maxBeamWidth = 0.35f;
    [SerializeField] private float minBeamWidth = 0.2f;
    [SerializeField] private Material beamMaterial = null;
    [SerializeField] private Color beamColor = Color.white;


    private LineRenderer lineRenderer;
    private IEnumerator beamAnimation;

    public override string Name => "Pulse Laser";

    private void Awake()
    {
        shootCallback = Shoot;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0f;
    }

    private void OnEnable()
    {
        lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, BeamLength, 0f) });
        lineRenderer.widthMultiplier = 0f;

        lineRenderer.material = beamMaterial;
        lineRenderer.startColor = beamColor;
        lineRenderer.endColor = beamColor;
    }

    private void OnDisable()
    {
        lineRenderer.widthMultiplier = 0f;
    }


    protected override void Shoot()
    {
        float delay = 1 / ShotsPerSecond;
        if(beamAnimation != null)
            StopCoroutine(beamAnimation);
        beamAnimation = AnimateBeam(delay);
        StartCoroutine(beamAnimation);


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
            lineRenderer.widthMultiplier = Mathf.Lerp(minBeamWidth, maxBeamWidth, 
                Mathf.InverseLerp(endTime, startTime, Time.time));
            yield return null;
        }
        lineRenderer.widthMultiplier = 0f;
    }

    public void Init(int damage, float fireRate, float beamLength, 
        float maxWidth, float minWidth, Material beamMaterial, Color beamColor)
    {
        ShotsPerSecond = fireRate;
        DamagePerPulse = damage;
        BeamLength = beamLength;
        maxBeamWidth = maxWidth;
        minBeamWidth = minWidth;

        lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, BeamLength, 0f) });
        lineRenderer.useWorldSpace = false;
        lineRenderer.material = beamMaterial;
        lineRenderer.startColor = beamColor;
        lineRenderer.endColor = beamColor;

        this.beamMaterial = beamMaterial;
        this.beamColor = beamColor;
    }


}
