
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathNodes))]
public class PathNodesEditor : Editor
{
    private Tool _lastTool = Tool.None;
    private GUIStyle _style = new GUIStyle();

    private SerializedObject so;
    private SerializedProperty controlPointSerialProp;
    private SerializedProperty durationProp;

    private void OnEnable()
    {
        _lastTool = UnityEditor.Tools.current;
        Tools.current = Tool.None;

        _style.fontStyle = FontStyle.Bold;
        _style.normal.textColor = Color.black;

        so = serializedObject;
        controlPointSerialProp = so.FindProperty("controlPoints");
        durationProp = so.FindProperty("duration");

        SceneView.duringSceneGui += DuringSceneGUI; 
    }

    private void OnDisable()
    {
        Tools.current = _lastTool;

        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sceneView)
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
            float duration = durationProp.floatValue;
            float timeStep = 0;
            if (arrayLength > 1)
                timeStep = duration / (arrayLength - 1);
            float t = 0;
            for (int i = 0; i < arrayLength; i++)
            {
                item = controlPointSerialProp.GetArrayElementAtIndex(i);
                curveX.AddKey(new Keyframe(t, item.vector2Value.x));
                curveY.AddKey(new Keyframe(t, item.vector2Value.y));                
                t += timeStep;
            }

            for (int i = 0; i < arrayLength; i++)
            {
                curveX.SmoothTangents(i, 0f);
                curveY.SmoothTangents(i, 0f);
            }

            for (float timeCount = 0; timeCount <= duration; timeCount += 0.1f)
            {
                
                Handles.SphereHandleCap(1, new Vector3(curveX.Evaluate(timeCount),
                    curveY.Evaluate(timeCount), 0), Quaternion.identity, 0.2f, EventType.Repaint);
            }
        }
    }

}
