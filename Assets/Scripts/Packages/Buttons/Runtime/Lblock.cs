using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBlock : MonoBehaviour
{
    public List<Vector2Int> cells = new List<Vector2Int>()
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 0),
    };

    public float gridSize = 1f;
    private Vector3 dragOffset;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
            UpdateVisual();
        }
    }

    void OnMouseDown()
    {
        dragOffset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + dragOffset;
    }

    void OnMouseUp()
    {
        SnapToGrid();
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    void Rotate()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            Vector2Int c = cells[i];
            cells[i] = new Vector2Int(-c.y, c.x);
        }
    }

    void SnapToGrid()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.y = Mathf.Round(pos.y / gridSize) * gridSize;
        transform.position = pos;
    }

    void UpdateVisual()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2Int cell = cells[i];
            transform.GetChild(i).localPosition =
                new Vector3(cell.x * gridSize, cell.y * gridSize, 0);
        }
    }
}
