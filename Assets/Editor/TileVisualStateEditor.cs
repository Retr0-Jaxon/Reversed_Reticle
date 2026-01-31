using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileVisualStateManager))]
public class TileVisualStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 绘制原有的所有变量
        DrawDefaultInspector();

        TileVisualStateManager manager = (TileVisualStateManager)target;

        // 加一条分割线
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("--- 当前Visual State ---", MessageType.None);

        // 获取私有的 currentState 字段
        var currentStateField = typeof(TileVisualStateManager).GetField("currentState",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        object stateValue = currentStateField?.GetValue(manager);
        string stateName = stateValue != null ? stateValue.GetType().Name : "Null";

        // 根据状态选择颜色
        MessageType messageType = GetMessageTypes(stateName);
        string stateIcon = GetStateIcon(stateName);

        // 绘制状态信息框
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField($"{stateIcon} {stateName}", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        // 额外状态信息
        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox($"当前处于: {stateName} 状态", messageType);
        }
        else
        {
            EditorGUILayout.HelpBox("进入播放模式后查看实时状态", MessageType.Info);
        }

        // 如果在运行模式，每一帧重绘
        if (Application.isPlaying)
        {
            Repaint();
        }
    }

    private MessageType GetMessageTypes(string stateName)
    {
        switch (stateName)
        {
            case "IdleState":
                return MessageType.Info;
            case "SelectedState":
                return MessageType.Warning;
            case "HintState":
                return MessageType.None;
            case "LuminousState":
                return MessageType.Error;
            default:
                return MessageType.None;
        }
    }

    private string GetStateIcon(string stateName)
    {
        switch (stateName)
        {
            case "IdleState":
                return "💤";
            case "SelectedState":
                return "⭐";
            case "HintState":
                return "💡";
            case "LuminousState":
                return "✨";
            default:
                return "❓";
        }
    }
}