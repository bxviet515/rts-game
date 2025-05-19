using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementProcess
{
    private GameObject m_PlacementOutline;
    private BuildActionSO m_BuildActionSO;
    private Vector3Int[] m_HighlightPositions;
    public PlacementProcess(BuildActionSO buildAction)
    {
        m_BuildActionSO = buildAction;
    }
    public void Update()
    {
        if (m_PlacementOutline != null)
        {
            HighlightTiles(m_PlacementOutline.transform.position);
        }
        if (HvOUtils.TryGetHoldPosition(out Vector3 worldPosition))
        {
            m_PlacementOutline.transform.position = SnapToGrid(worldPosition);
        }
    }
    public void ShowPlacementOutline()
    {
        m_PlacementOutline = new GameObject("PlacementOutline");
        var renderder = m_PlacementOutline.AddComponent<SpriteRenderer>();
        renderder.sortingOrder = 999;
        renderder.color = new Color(1, 1, 1, 0.5f);
        renderder.sprite = m_BuildActionSO.PlacementSprite;
    }
    private Vector3 SnapToGrid(Vector3 worldPosition)
    {
        return new Vector3(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y), 0);
    }

    private void HighlightTiles(Vector3 outlinePosition)
    {
        Vector2Int buildingSize = new Vector2Int(2, 3);
        m_HighlightPositions = new Vector3Int[buildingSize.x * buildingSize.y];
        for (int x = 0; x < buildingSize.x; x++)
        {
            for (int y = 0; y < buildingSize.y; y++)
            {
                m_HighlightPositions[x + y * buildingSize.x] = new Vector3Int((int)outlinePosition.x + x, (int)outlinePosition.y + y, 0);
            }
        }
        foreach (var tilePosition in m_HighlightPositions)
        {
            Debug.Log(tilePosition);
        }
        Debug.Log("-------------");
    }
}
