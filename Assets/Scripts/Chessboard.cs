using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    private List<Tile> tiles=new List<Tile>();
    public static Chessboard instance;
    private int boardX;
    private int boardY;
    
    [SerializeField]
    private GameObject tilePrefab;

    public int BoardX
    {
        get => boardX;
        set => boardX = value;
    }

    [SerializeField]
    private float tileSize = 1.0f;

    public int BoardY
    {
        get => boardY;
        set => boardY = value;
    }

    public List<Tile> Tiles
    {
        get => tiles;
        set => tiles = value;
    }

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



    /*IEnumerator GlowByGroup()
    {
        var groups = tiles
            .GroupBy(t => t.GlowGroup)
            .OrderBy(g => g.Key);
        yield return new WaitForSeconds(0.5f);
        foreach (var group in groups)
        {
            foreach (Tile tile in group)
            {
                if (tile.TileType==TileType.LIGHT)
                {
                    tile.glow();
                }

                
            }
            yield return new WaitForSeconds(1f);
            foreach (Tile tile in group)
            {
                if (tile.TileType==TileType.LIGHT)
                {
                    tile.stopGlow();
                }

                
            }
            yield return new WaitForSeconds(0.3f);
        }
    }*/



    
    
    public void generateUnit()
    {
        Debug.Log("generate");
        Debug.Log("boardX:"+boardX);
        Debug.Log("boardY:"+boardY);
        ClearChildren();

        GenerateTiles();

    }

    private void ClearChildren()
    {
        // 逆序删除，避免索引问题
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }
        }
    }
    private void GenerateTiles()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("tilePrefab is null!");
            return;
        }
        float offsetX = (boardX - 1) * 0.5f;
        float offsetY = (boardY - 1) * 0.5f;
        for (int i = 0; i < boardX; i++)
        {
            for (int j = 0; j < boardY; j++)
            {
                Vector3 localPos = new Vector3(
                    (i - offsetX) * tileSize,
                    (offsetY-j) * tileSize,
                    0f
                );
                GameObject tile = Instantiate(tilePrefab, transform);
                var tileComponent = tile.GetComponent<Tile>();
                tileComponent.X = i;
                tileComponent.Y = j;
                tile.transform.localPosition = localPos;
                tile.transform.SetParent(transform); // 设为当前物体的子物体
            }
        }
    }
    
    
    
    
    
    
    
}



