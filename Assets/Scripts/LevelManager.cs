using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class LevelManager
{
    
    
    /**
     * 判断当前关卡是否完成
     */
    public bool isLevelComplete()
    {
        Chessboard chessboard = Chessboard.instance;
        foreach (Tile tile in chessboard.tiles)
        {
            if (tile.isSelected&&tile.TileType==TileType.LIGHT)
            {
                return false;
            }

            if (!tile.isSelected&&tile.TileType==TileType.NORMAL)
            {
                return false;
            }
        }

        return true;
    }
    
    

}