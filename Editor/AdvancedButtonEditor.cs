using JFramework.Unity;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(AdvancedButton))]
public class AdvancedButtonEditor : ButtonEditor
{
    SerializedProperty minClickInterval;
    SerializedProperty longPressDuration;
    SerializedProperty triggerLongPressComplete;
    SerializedProperty advancedEvents;

    protected override void OnEnable()
    {
        base.OnEnable();
        minClickInterval = serializedObject.FindProperty("minClickInterval");
        longPressDuration = serializedObject.FindProperty("longPressDuration");
        triggerLongPressComplete = serializedObject.FindProperty("triggerLongPressComplete");
        advancedEvents = serializedObject.FindProperty("advancedEvents");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Advanced Button Settings", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(minClickInterval);
        EditorGUILayout.PropertyField(longPressDuration);
        EditorGUILayout.PropertyField(triggerLongPressComplete);
        EditorGUILayout.PropertyField(advancedEvents);

        serializedObject.ApplyModifiedProperties();
    }
}