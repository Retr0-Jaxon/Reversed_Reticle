using System.Collections;
using System.Collections.Generic;
using com.startech.Buttons;
using Enums;
using UnityEngine;

public class Tile
{
    [SerializeField]
    private TileType tileType;

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

    public bool isSelected { get;private set; }


    public void onClick()
    {
        isSelected = !isSelected;
        GameManager.instance.checkLevelComplete();
    }
    
    
    
    
    
    

}