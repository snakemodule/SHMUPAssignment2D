﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    private Tool _lastTool = Tool.None;
    private GUIStyle _style = new GUIStyle();

    private SerializedObject so;
    private SerializedProperty controlPointSerialProp;

    private void OnEnable()
    {
        _lastTool = UnityEditor.Tools.current;
        Tools.current = Tool.None;

        _style.fontStyle = FontStyle.Bold;
        _style.normal.textColor = Color.black;

        so = serializedObject;
        controlPointSerialProp = so.FindProperty("controlPoints");
    }

    private void OnDisable()
    {
        Tools.current = _lastTool;
    }


    private void OnSceneGUI()
    {        
        so.Update();
        int arrayLength = controlPointSerialProp.arraySize;
        SerializedProperty item;
        for (int i = 0; i < arrayLength; i++)
        {
            item = controlPointSerialProp.GetArrayElementAtIndex(i);
            item.vector2Value = Handles.PositionHandle(item.vector2Value, Quaternion.identity);
        }        
        so.ApplyModifiedProperties();

        if (Event.current.type == EventType.Repaint)
        {
            AnimationCurve curveX = new AnimationCurve();
            AnimationCurve curveY = new AnimationCurve();
            float timeStep = 0;
            if (arrayLength > 1)
                timeStep = 1f / (arrayLength - 1);
            float t = 0;
            for (int i = 0; i < arrayLength; i++)
            {
                item = controlPointSerialProp.GetArrayElementAtIndex(i);
                curveX.AddKey(new Keyframe(t, item.vector2Value.x));
                curveY.AddKey(new Keyframe(t, item.vector2Value.y));
                Assert.IsTrue(t <= 1);
                t += timeStep;
            }

            for (int i = 0; i < arrayLength; i++)
            {
                curveX.SmoothTangents(i, 0f);
                curveY.SmoothTangents(i, 0f);
            }

            for (float timeCount = 0; timeCount <= 1f; timeCount += 0.02f)
            {
                //Gizmos.DrawSphere(new Vector3(curveX.Evaluate(timeCount), curveY.Evaluate(timeCount), 0), 0.2f);
                Handles.SphereHandleCap(1, new Vector3(curveX.Evaluate(timeCount), 
                    curveY.Evaluate(timeCount), 0), Quaternion.identity, 0.2f, EventType.Repaint);
            }
        }
    }

    public static void LogProperties(SerializedObject so, bool includeChildren = true)
    {
        // Shows all the properties in the serialized object with name and type
        // You can use this to learn the structure
        so.Update();
        SerializedProperty propertyLogger = so.GetIterator();
        while (true)
        {
            Debug.Log("name = " + propertyLogger.name + " type = " + propertyLogger.type);
            if (!propertyLogger.Next(includeChildren)) break;
        }
    }

}
