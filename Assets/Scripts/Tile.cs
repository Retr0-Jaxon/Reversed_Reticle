using System.Collections;
using System.Collections.Generic;
using com.startech.Buttons;
using Enums;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileType tileType;

    /**
     * 发光的组号
     */
    [SerializeField]
    private int glowGroup;
    

    [SerializeField]
    private int x;
    [SerializeField]
    private int y;

    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }

    public int X
    {
        get => x;
        set => x = value;
    }

    public int Y
    {
        get => y;
        set => y = value;
    }

    public int GlowGroup
    {
        get => glowGroup;
        set => glowGroup = value;
    }
    public bool isSelected { get;private set; }


    public void onClick()
    {
        isSelected = !isSelected;
        GameManager.instance.checkLevelComplete();
    }

    /**
     * 发光方法，待填充
     */
    public void glow()
    {
        
        
    }






}