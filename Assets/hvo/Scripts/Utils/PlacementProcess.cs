using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementProcess
{
    private GameObject m_PlacementOutline;
    private BuildActionSO m_BuildActionSO;
    private Vector3Int[] m_HighlightPositions;
    private Tilemap m_WalkableTilemap;
    private Tilemap m_OverlayTilemap;
    private Tilemap[] m_UnreachableTilemaps;
    private Sprite m_PlaceholderTileSprite;
    private Color m_HighlightColor = new Color(0, 0.8f, 1, 0.4f);
    private Color m_BlockedColor = new Color(1, 0.2f, 0, 0.8f);
    public BuildActionSO BuildAction => m_BuildActionSO;
    public int GoldCost => m_BuildActionSO.GoldCost;
    public int WoodCost => m_BuildActionSO.WoodCost;
    public PlacementProcess(BuildActionSO buildAction, Tilemap walkableTilemap, Tilemap overlayTilemap, Tilemap[] unreachableTilemaps)
    {
        m_PlaceholderTileSprite = Resources.Load<Sprite>("Images/PlaceholderTileSprite");
        m_BuildActionSO = buildAction;
        m_WalkableTilemap = walkableTilemap;
        m_OverlayTilemap = overlayTilemap;
        m_UnreachableTilemaps = unreachableTilemaps;
    }
    public void Update()
    {
        if (m_PlacementOutline != null)
        {
            HighlightTiles(m_PlacementOutline.transform.position);
        }
        if (HvOUtils.IsPointerOverUIElement()) return;
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
    public void CleanUp()
    {
        Object.Destroy(m_PlacementOutline);
        ClearHighlights();
    }

    public bool TryFinalizePlacement(out Vector3 buildPosition)
    {
        if (IsPlacementAreaValid())
        {
            ClearHighlights();
            buildPosition = m_PlacementOutline.transform.position;
            Object.Destroy(m_PlacementOutline);
            return true;
        }
        Debug.Log("Invalid Placement Area");
        buildPosition = Vector3.zero;
        return false;
    }

    private bool IsPlacementAreaValid() {
        foreach (var tilePosition in m_HighlightPositions)
        {
            if (!CanPlaceTile(tilePosition)) return false;
        }
        return true;
    }
    private void HighlightTiles(Vector3 outlinePosition)
    {
        Vector3Int buildingSize = m_BuildActionSO.BuildingSize;
        Vector3 pivotPosition = outlinePosition + m_BuildActionSO.OriginOffSet;
        ClearHighlights();
        m_HighlightPositions = new Vector3Int[buildingSize.x * buildingSize.y];
        for (int x = 0; x < buildingSize.x; x++)
        {
            for (int y = 0; y < buildingSize.y; y++)
            {
                m_HighlightPositions[x + y * buildingSize.x] = new Vector3Int((int)pivotPosition.x + x, (int)pivotPosition.y + y, 0);
            }
        }
        foreach (var tilePosition in m_HighlightPositions)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = m_PlaceholderTileSprite;
            if (CanPlaceTile(tilePosition))
            {
                tile.color = m_HighlightColor;
            }
            else
            {
                tile.color = m_BlockedColor;
            }

            m_OverlayTilemap.SetTile(tilePosition, tile);
            // m_OverlayTilemap.SetTileFlags(tilePosition, TileFlags.None);
            // m_OverlayTilemap.SetColor(tilePosition, Color.green);
        }

    }
    private void ClearHighlights()
    {
        if (m_HighlightPositions == null) return;
        foreach (var tilePosition in m_HighlightPositions)
        {
            m_OverlayTilemap.SetTile(tilePosition, null);
        }
    }

    private bool CanPlaceTile(Vector3Int tilePosition)
    {
        return m_WalkableTilemap.HasTile(tilePosition) && !IsInUnreachableTilemap(tilePosition) && !IsBlockedByGameobject(tilePosition);
    }

    private bool IsInUnreachableTilemap(Vector3Int tilePosition)
    {
        foreach (var tilemap in m_UnreachableTilemaps)
        {
            if (tilemap.HasTile(tilePosition)) return true;
        }
        return false;
    }

    private bool IsBlockedByGameobject(Vector3Int tilePosition)
    {
        Vector3 tileSize = m_WalkableTilemap.cellSize;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(tilePosition + tileSize / 2, tileSize * 0.7f, 0);
        foreach (var collider in colliders)
        {
            var layer = collider.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        return false;
    }
}
