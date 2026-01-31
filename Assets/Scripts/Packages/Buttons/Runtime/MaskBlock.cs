using System.Collections.Generic;
using UnityEngine;
using com.startech.Buttons;

public class MaskBlock : Buttons
{
    [Header("L 方块的子格（每个 1×1）")]
    public List<Transform> subBlocks;

    private Vector3 dragOffset;
    private Vector3 lastValidPosition;
    private Quaternion lastValidRotation;

    protected override void Update()
    {
        base.Update();

        // 按 R 旋转整个 L 方块
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0, 0, -90f);
            HighlightCoveredTiles();
        }
    }

    // Buttons 系统里的点击回调
    public override void OnClick()
    {
        dragOffset = transform.position - GetMouseWorldPos();
        lastValidPosition = transform.position;
        lastValidRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 拖动
        if (Input.GetMouseButton(0))
        {
            transform.position = GetMouseWorldPos() + dragOffset;
            HighlightCoveredTiles();
        }

        // 松开鼠标，判定是否合法
        if (Input.GetMouseButtonUp(0))
        {
            if (!IsValidPlacement())
            {
                transform.position = lastValidPosition;
                transform.rotation = lastValidRotation;
            }
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

    bool IsValidPlacement()
    {
        // 必须所有子块都正好落在 Tile 上
        return GetCoveredTiles().Count == subBlocks.Count;
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
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}
