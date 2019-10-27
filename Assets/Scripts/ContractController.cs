using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public struct ContractNum
{
    public int article;
    public int clause;
}

public class ContractController : MonoBehaviour
{
    [SerializeField] private ContractNum myNum;
    [SerializeField] private List<ContractNum> group;

    [SerializeField] private Image box;
    [SerializeField] private List<Sprite> checkImage;

    private void Start()
    {
        Agree();
    }

    private void Update()
    {
        if (Contract.instance.GetContractState(myNum.article, myNum.clause)) box.sprite = checkImage[0];
        else box.sprite = checkImage[1];
    }

    public void CheckBox()
    {
        if (Contract.instance.GetContractState(myNum.article, myNum.clause)) Disagree();
        else Agree();
    }

    private void Agree()
    {
        Contract.instance.ChangeContractState(myNum.article, myNum.clause, true);

        for (int i = 0; i < group.Count; i++)
        {
            ContractNum groupNum = group[i];
            Contract.instance.ChangeContractState(groupNum.article, groupNum.clause, true);
        }
    }

    private void Disagree()
    {
        Contract.instance.ChangeContractState(myNum.article, myNum.clause, false);

        for (int i = 0; i < group.Count; i++)
        {
            ContractNum groupNum = group[i];
            Contract.instance.ChangeContractState(groupNum.article, groupNum.clause, false);
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ContractNum))]
public class ContractDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(new Rect(position.x, position.y, 30, position.height), property.FindPropertyRelative("article"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 30, position.y, 15, position.height), "조");
        EditorGUI.PropertyField(new Rect(position.x + 45, position.y, 30, position.height), property.FindPropertyRelative("clause"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 75, position.y, 15, position.height), "항");

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif