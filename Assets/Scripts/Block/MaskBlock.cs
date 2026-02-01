using System.Collections.Generic;
using UnityEngine;
using com.startech.Buttons;

public class MaskBlock : Buttons
{
    [Header("L 方块的子格（每个 1×1）")]
    public List<Transform> subBlocks;
    
    private Vector3 lastValidPosition;
    private Quaternion lastValidRotation;

    private List<Tile> coveredTiles;
    
    private Camera mainCamera;

    private bool onDragging;
    
    
    public override void NextStart()
    {
        mainCamera=Camera.main;
        onDragging = false;
    }

    protected override void Update()
    {
        base.Update();

        // 按 R 旋转整个 L 方块
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0, 0, -90f);
            HighlightCoveredTiles();
        }
        
        // 松开鼠标，判定是否合法
        if (onDragging&&Input.GetMouseButtonUp(0))
        {
            onDragging = false;
            
            
            if (!TryPlace())
            {
                transform.position = lastValidPosition;
                transform.rotation = lastValidRotation;
            }
        }
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
        if (coveredTiles==null)
        {
            return;
        }
        foreach (Tile tile in coveredTiles)
        {
            tile.unSelect();
            tile.ParentMaskBlock = null;
        }
        coveredTiles = null;
    }

    private void LateUpdate()
    {
        // 拖动
        if (onDragging)
        {
            var mouseWorldPos = GetMouseWorldPos();
            var transformPos = mouseWorldPos;
            transformPos.z -= 0.5f;
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
            Vector3 subPos = sub.position;

            foreach (Tile tile in Chessboard.instance.Tiles)
            {
                // tileSize = 1，所以 0.45 是安全阈值
                if (Vector2.Distance(subPos, tile.transform.position) < 0.45f)
                {
                    result.Add(tile);
                    break;
                }
            }
        }

        return result;
    }

    private bool IsPlaceable()
    {
        // 必须所有子块都正好落在 Tile 上
        var coveredTiles = GetCoveredTiles();
        var count = coveredTiles.Count;
        if (count>0&&count!=subBlocks.Count)
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
        return true;
    }

    private bool TryPlace()
    {
        if (!IsPlaceable())
        {
            return false;
        }
        coveredTiles = GetCoveredTiles();
        foreach (var tile in coveredTiles)
        {
            tile.onClick();
            tile.ParentMaskBlock = this;
        }
        SnapToTiles();


        return true;
    }

    void HighlightCoveredTiles()
    {
        // 清空之前的高亮
        foreach (Tile tile in Chessboard.instance.Tiles)
        {
            tile.stopGlow();
        }

        // 高亮当前覆盖的 Tile
        foreach (Tile tile in GetCoveredTiles())
        {
            if (!tile.IsSelected)
            {
                tile.glow();
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
        transformPosition.z +=0.15f;
        transform.position = transformPosition;
    }
    
    
    
    
    
    
    
}
