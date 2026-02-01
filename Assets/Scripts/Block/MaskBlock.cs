using System.Collections.Generic;
using UnityEngine;
using com.startech.Buttons;

public class MaskBlock : Buttons
{
    [Header("L 方块的子格（每个 1×1）")]
    public List<Transform> subBlocks;
    
    private Vector3 lastValidPosition;
    private Quaternion lastValidRotation;
    
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
        if (Input.GetMouseButtonUp(0))
        {
            onDragging = false;
            if (!IsValidPlacement())
            {
                transform.position = lastValidPosition;
                transform.rotation = lastValidRotation;
            }
        }
    }

    // Buttons 系统里的点击回调
    public override void OnClick()
    {
        onDragging = true;
        lastValidPosition = transform.position;
        lastValidRotation = transform.rotation;
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

    private bool IsValidPlacement()
    {
        // 必须所有子块都正好落在 Tile 上
        var count = GetCoveredTiles().Count;
        if (count>0&&count!=subBlocks.Count)
        {
            return false;
        }
        
        //不能发生碰撞

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
            tile.glow();
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
}
