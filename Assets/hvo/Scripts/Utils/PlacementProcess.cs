using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementProcess
{
    private BuildActionSO m_BuildActionSO;
    public PlacementProcess(BuildActionSO buildAction)
    {
        m_BuildActionSO = buildAction;
    }
    public void Update()
    {
        Debug.Log("PlacementProcess Update()");
    }
    public void ShowPlacementOutline()
    {
        Debug.Log("ShowPlacementOutline");
    }
}
