using System.Collections.Generic;
using UnityEngine;
using com.startech.Buttons;
using Enums;

public class MaskBlock : Buttons
{
    [Header("L 方块的子格（每个 1×1）")]
    public List<Transform> subBlocks;
    
    private Vector3 lastValidPosition;
    private Quaternion lastValidRotation;

    private List<Tile> placedTiles;
    
    private Camera mainCamera;

    private bool onDragging;

    private float startZ;
    
    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool isPlaced;
    
    
    public override void NextStart()
    {
        mainCamera=Camera.main;
        onDragging = false;
        isPlaced = false;
        startZ = transform.position.z;
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        
    }
    
    protected override void Update()
    {
        base.Update();

        
        
        // 松开鼠标，判定是否合法
        if (onDragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                onDragging = false;
                if (!TryPlace())
                {
                    backToPreviousLocation();
                }
            }
            // 按 R 旋转整个 L 方块
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (!isPlaced)
                {
                    transform.Rotate(0, 0, -90f);
                    HighlightCoveredTiles();
                }
            
            }

            
            
            
            
            
        }
    }

    private void backToPreviousLocation()
    {
        ClearChessboardGlow();
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    // Buttons 系统里的点击回调
    public override void OnClick()
    {
        if (!onDragging)
        {
            clearPlacedTiles();
        }
        onDragging = true;
        lastValidPosition = transform.position;
        lastValidRotation = transform.rotation;
        
    }

    private void clearPlacedTiles()
    {
        ClearChessboardGlow();
        isPlaced = false;
        
        if (placedTiles!=null)
        {
            foreach (Tile tile in placedTiles)
            {
                tile.unSelect();
                tile.ParentMaskBlock = null;
            }
            placedTiles = null;
        }
        
    }

    private void LateUpdate()
    {
        // 拖动
        if (onDragging)
        {
            var mouseWorldPos = GetMouseWorldPos();
            var transformPos = mouseWorldPos;
            transformPos.z = startZ;
            transform.position = transformPos;
            HighlightCoveredTiles();
        }

        
    }

    // ===================== 核心：坐标映射 =====================

    List<Tile> GetCoveredTiles()
    {
        List<Tile> result = new List<Tile>();
        

        foreach (Transform sub in subBlocks)
        {
            float curDistance = 99999f;
            Tile chooseTile = null;
            Vector3 subPos = sub.position;

            foreach (Tile tile in Chessboard.instance.Tiles)
            {
                var distance = Vector2.Distance(subPos, tile.transform.position);
                if (distance < 0.30f)
                {
                    if (Vector2.Distance(subPos, tile.transform.position) < curDistance)
                    {
                        curDistance = Vector2.Distance(subPos, tile.transform.position);
                        chooseTile = tile;
                    }
                    
                }
            }

            if (chooseTile==null)
            {
                continue;
            }
            if (result.Contains( chooseTile))
            {
                return new List<Tile>();
            }
            else
            {
                result.Add(chooseTile);
            }
            
            
            
            
        }

        return result;
    }

    private bool IsZeroTilePlaced()
    {
        return GetCoveredTiles().Count == 0;
    }
    
    private bool TryPlace()
    {
        // 必须所有子块都正好落在 Tile 上
        var coveredTiles = GetCoveredTiles();
        var count = coveredTiles.Count;
        if (coveredTiles.Count!=0&&count!=subBlocks.Count)
        {
            return false;
        }
        
        //不能发生碰撞=>所有字块都不能先前已被选择
        foreach (var tile in coveredTiles)
        {
            
            if (tile.IsSelected)
            {
                return false;
            }
        }
        
        if (coveredTiles.Count==subBlocks.Count)
        {
            foreach (var tile in coveredTiles)
            {
                tile.onSelected();
                tile.ParentMaskBlock = this;
            }
            placedTiles = coveredTiles;
            isPlaced = true;
            SnapToTiles();
        }
        return true;
    }

    void HighlightCoveredTiles()
    {
        ClearChessboardGlow();

        // 高亮当前覆盖的 Tile
        foreach (Tile tile in GetCoveredTiles())
        {
            if (!tile.IsSelected)
            {
                tile.glow();
            }

            
        }
    }

    private static void ClearChessboardGlow()
    {
        // 清空之前的高亮
        foreach (Tile tile in Chessboard.instance.Tiles)
        {
            if (!tile.IsSelected && tile.TileVisualStateManager.CurrentState.StateType==BaseVisualStateType.Luminous) 
            {
                tile.stopGlow();
            }

            
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Z = -0.01 的平面
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, -0.01f));
        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        return transform.position;
    }
    
    
    
    private void SnapToTiles()
    {
        var coveredTiles = GetCoveredTiles();
        // 没有覆盖 Tile，不吸附
        if (coveredTiles.Count == 0)
            return;
        // 用第一个子块作为参考
        Transform refSub = subBlocks[0];
        // 找到它对应的 Tile
        Tile targetTile = null;
        float minDist = float.MaxValue;
        foreach (Tile tile in Chessboard.instance.Tiles)
        {
            float dist = Vector2.Distance(refSub.position, tile.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetTile = tile;
            }
        }
        if (targetTile == null)
            return;
        // 计算偏移量（吸附核心）
        Vector3 offset = targetTile.transform.position - refSub.position;
        transform.position += offset;
        var transformPosition = transform.position;
        transformPosition.z = 0.15f;
        transform.position = transformPosition;
    }
    
    
    
    
    
    
    
}
