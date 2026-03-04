using Game.Common;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ExtendImage))]
[CanEditMultipleObjects]
public class ExtendImageEditor : ImageEditor
{
    private SerializedProperty m_SlicedClipModeProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_SlicedClipModeProp = serializedObject.FindProperty("m_SlicedClipMode");
    }

    public override void OnInspectorGUI()
    {
        // 调用基类，绘制Image的Inspector
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_SlicedClipModeProp, new GUIContent("Sliced Clip Mode"));

        serializedObject.ApplyModifiedProperties();
    }
}