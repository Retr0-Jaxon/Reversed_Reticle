/*
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlowSequence))]
public class GlowControllerEditor : Editor
{
    private GlowSequence sequence;

    private void OnEnable()
    {
        sequence = (GlowSequence)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < sequence.commands.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            var cmd = sequence.commands[i];
            EditorGUILayout.LabelField($"Command {i}", EditorStyles.boldLabel);

            DrawCommand(cmd);

            if (GUILayout.Button("Remove"))
            {
                sequence.commands.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Glow Units"))
        {
            sequence.commands.Add(new GlowUnitsCommand());
        }

        if (GUILayout.Button("Add Wait"))
        {
            sequence.commands.Add(new WaitCommand());
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(sequence);
    }

    private void DrawCommand(GlowCommand cmd)
    {
        switch (cmd.commandType)
        {
            case GlowCommandType.GlowUnits:
                var glowCmd = cmd as GlowUnitsCommand;
                int count = Mathf.Max(0,
                    EditorGUILayout.IntField("Unit Count", glowCmd.units.Count));

                while (count > glowCmd.units.Count)
                    glowCmd.units.Add(null);

                while (count < glowCmd.units.Count)
                    glowCmd.units.RemoveAt(glowCmd.units.Count - 1);

                for (int i = 0; i < glowCmd.units.Count; i++)
                {
                    glowCmd.units[i] = (Unit)EditorGUILayout.ObjectField(
                        $"Unit {i}",
                        glowCmd.units[i],
                        typeof(Unit),
                        true
                    );
                }
                break;

            case GlowCommandType.Wait:
                var waitCmd = cmd as WaitCommand;
                waitCmd.waitTime = EditorGUILayout.FloatField(
                    "Wait Time (sec)",
                    waitCmd.waitTime
                );
                break;
        }
    }
}
*/
