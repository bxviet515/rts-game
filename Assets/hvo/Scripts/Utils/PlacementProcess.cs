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
        Vector3 worldPosition = HvOUtils.InputHoldWorldPosition;
        if (worldPosition != Vector3.zero)
        {
            m_PlacementOutline.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
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
}
