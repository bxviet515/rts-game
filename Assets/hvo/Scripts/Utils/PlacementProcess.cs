using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementProcess
{
    private GameObject m_PlacementOutline;
    private BuildActionSO m_BuildActionSO;
    public PlacementProcess(BuildActionSO buildAction)
    {
        m_BuildActionSO = buildAction;
    }
    public void Update()
    {
        
    }
    public void ShowPlacementOutline()
    {
        m_PlacementOutline = new GameObject("PlacementOutline");
        var renderder = m_PlacementOutline.AddComponent<SpriteRenderer>();
        renderder.sortingOrder = 999;
        renderder.color = new Color(1, 1, 1, 0.5f);
        renderder.sprite = m_BuildActionSO.PlacementSprite;
    }
}
