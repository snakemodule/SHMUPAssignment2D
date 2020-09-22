using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LevelManager lm = target as LevelManager;
        base.OnInspectorGUI();
        if (GUILayout.Button("Sort event timeline"))
        {
            Comparison<LevelManager.TimelineEvent> cmp =
                (LevelManager.TimelineEvent x, LevelManager.TimelineEvent y) =>
                {
                    if (x.eventTime==y.eventTime)
                        return 0;
                    return (x.eventTime < y.eventTime) ? -1 : 1;
                };
            lm.timeline.Sort(cmp);
        }
    }



}
