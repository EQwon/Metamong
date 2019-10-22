using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditorInternal;

[CustomEditor(typeof(Contract))]
public class ContractsInspector : Editor
{
    ReorderableList contractList;

    float lineHeight;
    float lineHeightSpace;

    private void OnEnable()
    {
        lineHeight = EditorGUIUtility.singleLineHeight;
        lineHeightSpace = lineHeight + 3;
        contractList = new ReorderableList(serializedObject, serializedObject.FindProperty("contracts"));

        contractList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Contracts");
        };

        contractList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            float nameSpace = 80;
            float dx = (rect.width - nameSpace) / 5;
            var element = contractList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, nameSpace / 4, lineHeight), element.FindPropertyRelative("article"), GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x + nameSpace / 4, rect.y, nameSpace / 4, lineHeight), "조");
            EditorGUI.PropertyField(new Rect(rect.x + nameSpace / 2, rect.y, nameSpace / 4, lineHeight), element.FindPropertyRelative("clause"), GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x + 3 * nameSpace / 4, rect.y, nameSpace / 4, lineHeight), "항");
            EditorGUI.PropertyField(new Rect(rect.x + nameSpace, rect.y, dx, lineHeight), element.FindPropertyRelative("conditionClass"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + nameSpace + dx, rect.y, dx, lineHeight), element.FindPropertyRelative("conditionType"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + nameSpace + 2 * dx, rect.y, dx, lineHeight), element.FindPropertyRelative("conditionValue"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + nameSpace + 3 * dx, rect.y, dx, lineHeight), element.FindPropertyRelative("resultClass"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + nameSpace + 4 * dx, rect.y, dx, lineHeight), element.FindPropertyRelative("resultValue"), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        contractList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif