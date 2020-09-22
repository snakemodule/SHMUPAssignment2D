using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathNodes", menuName = "ScriptableObjects/PathNodes", order = 1)]
public class PathNodes : ScriptableObject
{
    public float duration = 1;
    public List<Vector2> controlPoints = new List<Vector2>();
    [HideInInspector] public AnimationCurve curveX = null;
    [HideInInspector] public AnimationCurve curveY = null;

    private void OnEnable()
    {
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();
        int arrayLength = controlPoints.Count;
        float timeStep = 0;
        if (arrayLength > 1)
            timeStep = duration / (arrayLength - 1);
        float t = 0;
        Vector2 item;
        for (int i = 0; i < arrayLength; i++)
        {
            item = controlPoints[i];
            curveX.AddKey(new Keyframe(t, item.x));
            curveY.AddKey(new Keyframe(t, item.y));
            t += timeStep;
        }
        for (int i = 0; i < arrayLength; i++)
        {
            curveX.SmoothTangents(i, 0f);
            curveY.SmoothTangents(i, 0f);
        }
    }
}
