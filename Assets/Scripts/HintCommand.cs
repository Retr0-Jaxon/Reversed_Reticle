using System.Collections.Generic;
using Enums;
using UnityEngine;

[System.Serializable]
public class HintCommand : GlowCommand
{
    [SerializeField]
    private List<Tile> tiles = new List<Tile>();

    [SerializeField]
    private float hintTime = 0f;  // 0表示无时间限制，永久提示

    public float HintTime
    {
        get => hintTime;
        set => hintTime = value;
    }

    public List<Tile> Tiles
    {
        get => tiles;
        set => tiles = value;
    }

    public HintCommand() : this(0, 0f) { }

    public HintCommand(int count, float hintTime = 0f)
    {
        this.hintTime = hintTime;
        commandType = GlowCommandType.Hint;
        while (count > tiles.Count)
        {
            tiles.Add(null);
        }
    }
}
