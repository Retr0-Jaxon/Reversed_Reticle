using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    public List<Tile> tiles=new List<Tile>();
    public static Chessboard instance;
    

    private void Awake()
    {
        tiles.Clear();
        instance=this;
        foreach (Transform child in transform)
        {
            Tile tile = child.GetComponent<Tile>();


            if (tile!=null)
            {
                tiles.Add(tile);
            }

            
        }
        
    }
}