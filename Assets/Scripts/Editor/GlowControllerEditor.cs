using Enums;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlowController))]
public class GlowControllerEditor : Editor
{
    private GlowController controller;
    

    private void OnEnable()
    {
        controller = (GlowController)target;

    }

    public override void OnInspectorGUI()
    {
        var controllerSequence = controller.Sequence;

        for (int i = 0; i < controllerSequence.Commands.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            var cmd = controllerSequence.Commands[i];
            EditorGUILayout.LabelField($"第{i+1}步", EditorStyles.boldLabel);

            DrawCommand(cmd);

            if (GUILayout.Button("Remove"))
            {
                controllerSequence.Commands.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("添加发光tiles"))
        {
            controllerSequence.Commands.Add(new GlowUnitsCommand(1,1f));
        }

        if (GUILayout.Button("添加waitTime"))
        {
            controllerSequence.Commands.Add(new WaitCommand(1f));
        }
        
        if (GUILayout.Button("添加hint"))
        {
            controllerSequence.Commands.Add(new HintCommand(1, 0f));
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(controller);
    }

    private void DrawCommand(GlowCommand cmd)
    {
        if (cmd==null)
        {
            Debug.Log("null");
        }
        switch (cmd.commandType)
        {
            case GlowCommandType.GlowUnits:
                var glowCmd = cmd as GlowUnitsCommand;
                EditorGUILayout.BeginHorizontal();
                // 左半：发光 tile 数量
                int count = Mathf.Max(0,
                    EditorGUILayout.IntField(
                        "发光tile数量",
                        glowCmd.Tiles.Count,
                        GUILayout.Width(EditorGUIUtility.currentViewWidth / 2 - 20)
                    ));
                // 右半：发光时间
                glowCmd.GlowTime = EditorGUILayout.FloatField(
                    "发光时间",
                    glowCmd.GlowTime,
                    GUILayout.Width(EditorGUIUtility.currentViewWidth / 2 - 20)
                );
                EditorGUILayout.EndHorizontal();

                while (count > glowCmd.Tiles.Count)
                    glowCmd.Tiles.Add(null);

                while (count < glowCmd.Tiles.Count)
                    glowCmd.Tiles.RemoveAt(glowCmd.Tiles.Count - 1);

                for (int i = 0; i < glowCmd.Tiles.Count; i++)
                {
                    glowCmd.Tiles[i] = (Tile)EditorGUILayout.ObjectField(
                        $"Tile {i}",
                        glowCmd.Tiles[i],
                        typeof(Tile),
                        true
                    );
                }
                break;

            case GlowCommandType.Wait:
                var waitCmd = cmd as WaitCommand;
                waitCmd.WaitTime = EditorGUILayout.FloatField(
                    "Wait Time (sec)",
                    waitCmd.WaitTime
                );
                break;

            case GlowCommandType.Hint:
                var hintCmd = cmd as HintCommand;
                EditorGUILayout.BeginHorizontal();
                int hintCount = Mathf.Max(0,
                    EditorGUILayout.IntField(
                        "提示tile数量",
                        hintCmd.Tiles.Count,
                        GUILayout.Width(EditorGUIUtility.currentViewWidth / 2 - 20)
                    ));
                hintCmd.HintTime = EditorGUILayout.FloatField(
                    "提示时间(0=永久)",
                    hintCmd.HintTime,
                    GUILayout.Width(EditorGUIUtility.currentViewWidth / 2 - 20)
                );
                EditorGUILayout.EndHorizontal();

                while (hintCount > hintCmd.Tiles.Count)
                    hintCmd.Tiles.Add(null);
                while (hintCount < hintCmd.Tiles.Count)
                    hintCmd.Tiles.RemoveAt(hintCmd.Tiles.Count - 1);

                for (int i = 0; i < hintCmd.Tiles.Count; i++)
                {
                    hintCmd.Tiles[i] = (Tile)EditorGUILayout.ObjectField(
                        $"Tile {i}",
                        hintCmd.Tiles[i],
                        typeof(Tile),
                        true
                    );
                }
                break;
        }
    }
}
