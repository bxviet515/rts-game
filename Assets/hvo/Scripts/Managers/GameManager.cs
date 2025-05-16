using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public Unit ActiveUnit;
    private Vector2 m_InitialTouchPosition;
    private void Update()
    {
        Vector2 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            m_InitialTouchPosition = inputPosition;
        }
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if (Vector2.Distance(m_InitialTouchPosition, inputPosition) < 10)
            {
                DetechClick(inputPosition);
            }
        }
    }
    private void DetechClick(Vector2 inputPosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        HandleClickOnGround(worldPoint);
    }
    private void HandleClickOnGround(Vector2 worldPoint)
    {
        ActiveUnit.MoveTo(worldPoint);
    }
}
