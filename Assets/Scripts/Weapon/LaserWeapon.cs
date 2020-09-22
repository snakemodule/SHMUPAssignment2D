using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaserWeapon : ContinousFireWeapon
{
    [SerializeField] private float BeamLength = 12f;
    [SerializeField] private float maxBeamWidth = 0.35f;
    [SerializeField] private float minBeamWidth = 0.2f;

    [SerializeField] private List<int> HitsLayers = new List<int> { 9 };    

    private LineRenderer lineRenderer;
    private IEnumerator beamAnimation;
    private int hitDetectLayerMask;

    private void Awake()
    {
        shootCallback = Shoot;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0f;
        hitDetectLayerMask = 0;
        HitsLayers.ForEach((layerNumber) => { hitDetectLayerMask = hitDetectLayerMask | (1 << layerNumber); }); //test this
    }

    private void OnEnable()
    {
        lineRenderer.SetPositions(new Vector3[]
            { Vector3.zero, new Vector3(0f, BeamLength, 0f) });
        lineRenderer.widthMultiplier = 0f;        
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
        

        Ray laserRay = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(laserRay, out hit, BeamLength, hitDetectLayerMask))
        {
            //todo deal damage
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
}
