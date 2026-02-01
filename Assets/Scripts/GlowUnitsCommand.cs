using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GlowUnitsCommand : GlowCommand
{
    [SerializeField]
    private List<Tile> tiles = new List<Tile>();


    [SerializeField]
    private float glowTime;

    public float GlowTime
    {
        get => glowTime;
        set => glowTime = value;
    }

    public List<Tile> Tiles
    {
        get => tiles;
        set => tiles = value;
    }

    public GlowUnitsCommand() : this(0,1f){}
    
    public GlowUnitsCommand(int count, float glowTime)
    {
        this.glowTime = glowTime;
        CommandType = GlowCommandType.GlowUnits;
        while (count > tiles.Count)
        {
            tiles.Add(null);
        }
    }
}