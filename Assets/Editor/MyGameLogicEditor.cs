using UnityEngine;
using UnityEditor; // 必须引用这个命名空间

// 告诉编辑器，这个脚本是为 MyGameLogic 类定制的
[CustomEditor(typeof(TileVisualStateManager))]
public class MyGameLogicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 1. 绘制原有的 Inspector 界面（保留变量显示）
        DrawDefaultInspector();

        // 获取目标脚本的引用
        TileVisualStateManager GameObject = (TileVisualStateManager)target;

        GUILayout.Space(10); // 加点空隙，好看一点
        GUILayout.Label("调试面板", EditorStyles.boldLabel);

        // 2. 绘制按钮
        if (GUILayout.Button("SetLuminous", GUILayout.Height(30)))
        {
            // 当按钮被按下时调用目标函数
            GameObject.SetLuminous(true);
        }

        if (GUILayout.Button("SetHint", GUILayout.Height(30)))
        {
            GameObject.SetHint(true);
        }
    }
}