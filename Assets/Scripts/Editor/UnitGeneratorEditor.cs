using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Chessboard))]
public class UnitGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 先画原本的 Inspector
        DrawDefaultInspector();

        Chessboard generator = (Chessboard)target;

        GUILayout.Space(10);
        // 一行：棋盘 + 两个输入框
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("棋盘大小：", GUILayout.Width(70));
        generator.BoardX = EditorGUILayout.IntField(generator.BoardX,GUILayout.Width(40));
        GUILayout.Label("x", GUILayout.Width(12));
        generator.BoardY = EditorGUILayout.IntField(generator.BoardY,GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        if (GUILayout.Button("生成unit"))
        {
            generator.generateUnit();
        }
    }
}