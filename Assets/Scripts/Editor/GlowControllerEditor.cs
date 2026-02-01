using System.Collections.Generic;
using Enums;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(GlowController))]
public class GlowControllerEditor : Editor
{
    private GlowController controller;
    private Dictionary<GlowCommand, ReorderableList> glowTileLists;
        
    

    private void OnEnable()
    {
        controller = (GlowController)target;
        glowTileLists= new Dictionary<GlowCommand, ReorderableList>();

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        

        for (int i = 0; i < controller.Commands.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            var cmd = controller.Commands[i];
            EditorGUILayout.LabelField($"第{i+1}步", EditorStyles.boldLabel);

            DrawCommand(cmd);

            if (GUILayout.Button("Remove"))
            {
                controller.Commands.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("添加发光tiles"))
        {
            controller.Commands.Add(new GlowUnitsCommand(0,1f));
        }

        if (GUILayout.Button("添加waitTime"))
        {
            controller.Commands.Add(new WaitCommand(1f));
        }
        
        if (GUILayout.Button("添加hint"))
        {
            controller.Commands.Add(new HintCommand(1, 0f));
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
        switch (cmd.CommandType)
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
                var waitCmd= (WaitCommand)cmd;
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
    
    private ReorderableList GetTileList(
        GlowCommand cmd,
        List<Tile> tiles,
        Dictionary<GlowCommand, ReorderableList> cache,
        string header)
    {
        if (cache.TryGetValue(cmd, out var list))
            return list;
        list = new ReorderableList(tiles, typeof(Tile), true, true, true, true);
        list.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, header);
        };
        list.drawElementCallback = (rect, index, active, focused) =>
        {
            rect.y += 2;
            tiles[index] = (Tile)EditorGUI.ObjectField(
                rect,
                tiles[index],
                typeof(Tile),
                true
            );
        };
        cache[cmd] = list;
        return list;
    }
}
