using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    private void Start()
    {
        StartCoroutine(GlowByGroup());
        
    }
    IEnumerator GlowByGroup()
    {
        var groups = tiles
            .GroupBy(t => t.GlowGroup)
            .OrderBy(g => g.Key);
        foreach (var group in groups)
        {
            foreach (Tile tile in group)
            {
                tile.glow();
            }
            yield return new WaitForSeconds(2f);
        }
    }



    public void generateUnit()
    {
        
    }
}