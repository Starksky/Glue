using _Project._Common.Scripts.Infrastructure.Configs;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnumDefinitionSO))]
public class EnumDefinitionSOEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var so = (EnumDefinitionSO)target;

        if (GUILayout.Button("Generate Enum Now"))
        {
            EnumFromSOGenerator.Generate();
        }
    }
}