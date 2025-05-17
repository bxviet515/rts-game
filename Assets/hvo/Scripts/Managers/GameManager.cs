using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    [Header("UI")]
    [SerializeField] private PointToClickPool m_PointToClickPool;
    [SerializeField] private ActionBar m_ActionBar;

    public Unit ActiveUnit;
    private Vector2 m_InitialTouchPosition;
    public bool HasActiveUnit => ActiveUnit != null;
    private void Start() {
        ClearAtionBarUI();
    }
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
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (HasClickedOnUnit(hit, out var unit))
        {
            HandleClickOnUnit(unit);
        }
        else
        {
            HandleClickOnGround(worldPoint);

        }
    }

    private bool HasClickedOnUnit(RaycastHit2D hit, out Unit unit)
    {
        if (hit.collider != null && hit.collider.TryGetComponent<Unit>(out var clickedUnit))
        {
            unit = clickedUnit;
            return true;
        }
        unit = null;
        return false;
    }
    private void HandleClickOnGround(Vector2 worldPoint)
    {

        if (HasActiveUnit && isHumanoid(ActiveUnit))
        {
            DisplayClickEffect(worldPoint);
            ActiveUnit.MoveTo(worldPoint);

        }
    }

    private void HandleClickOnUnit(Unit unit)
    {
        if (HasActiveUnit)
        {
            if (HasClickedOnActiveUnit(unit))
            {
                CancelActiveUnit();
                return;
            }
        }
        SelectNewUnit(unit);
    }
    private void SelectNewUnit(Unit unit)
    {
        if (HasActiveUnit)
        {
            ActiveUnit.DeSelect();
        }
        ActiveUnit = unit;
        ActiveUnit.Select();
        ShowUnitAction();
    }
    private void DisplayClickEffect(Vector2 worldPoint)
    {
        PointToClick clickEffect = m_PointToClickPool.Get();
        clickEffect.transform.position = worldPoint;
        clickEffect.Init(m_PointToClickPool);
    }

    private bool isHumanoid(Unit unit)
    {
        return unit is HumanoidUnit;
    }
    private void CancelActiveUnit()
    {
        ActiveUnit.DeSelect();
        ActiveUnit = null;
    }
    private bool HasClickedOnActiveUnit(Unit clickedUnit)
    {
        return clickedUnit == ActiveUnit;
    }

    private void ShowUnitAction()
    {
        ClearAtionBarUI();
        var hardCodedActions = 2;
        for (int i = 0; i < hardCodedActions; i++)
        {
            m_ActionBar.RegisterAction();
        }
        m_ActionBar.Show();
    }

    private void ClearAtionBarUI()
    {
        m_ActionBar.ClearAction();
        m_ActionBar.Hide();
    }
}
