using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GameManager : SingletonManager<GameManager>
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap m_WalkableTilemap;
    [SerializeField] private Tilemap m_OverlayTilemap;
    [SerializeField] private Tilemap[] m_UnreachableTilemaps;
    [Header("UI")]
    [SerializeField] private PointToClickPool m_PointToClickPool;
    [SerializeField] private ActionBar m_ActionBar;
    [SerializeField] private ConfirmationBar m_BuildConfirmationBar;

    public Unit ActiveUnit;

    private PlacementProcess m_PlacementProcess;
    private int m_Gold = 1000;
    private int m_Wood = 1000;
    public int Gold => m_Gold;
    public int Wood => m_Wood;
    public bool HasActiveUnit => ActiveUnit != null;


    private void Start()
    {
        ClearAtionBarUI();
    }
    private void Update()
    {
        if (m_PlacementProcess != null)
        {
            m_PlacementProcess.Update();
        }
        else if (HvOUtils.TryGetShortClickPosition(out Vector2 inputPosition))
        {
            DetechClick(inputPosition);
        }

    }
    public void StartBuildProcess(BuildActionSO buildAction)
    {
        if (m_PlacementProcess != null) return;

        m_PlacementProcess = new PlacementProcess(buildAction, m_WalkableTilemap, m_OverlayTilemap, m_UnreachableTilemaps);
        m_PlacementProcess.ShowPlacementOutline();
        m_BuildConfirmationBar.Show(buildAction.GoldCost, buildAction.WoodCost);
        m_BuildConfirmationBar.SetupHooks(ConfirmBuildPlacement, CancelBuildPlacement);
    }
    private void DetechClick(Vector2 inputPosition)
    {
        if (HvOUtils.IsPointerOverUIElement())
        {
            return;
        }
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
        ShowUnitAction(unit);
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
        ClearAtionBarUI();
    }
    private bool HasClickedOnActiveUnit(Unit clickedUnit)
    {
        return clickedUnit == ActiveUnit;
    }

    private void ShowUnitAction(Unit unit)
    {
        ClearAtionBarUI();
        if (unit.Actions.Length == 0)
        {
            return;
        }
        m_ActionBar.Show();
        foreach (var action in unit.Actions)
        {
            m_ActionBar.RegisterAction(
                action.Icon,
                () => action.Execute(this));
        }
    }

    private void ClearAtionBarUI()
    {
        m_ActionBar.ClearAction();
        m_ActionBar.Hide();
    }

    private void ConfirmBuildPlacement()
    {
        if (!TryDeductResources(m_PlacementProcess.GoldCost, m_PlacementProcess.WoodCost))
        {
            Debug.Log("Not enough resources");
            return;
        }
        if (m_PlacementProcess.TryFinalizePlacement(out Vector3 buildPosition))
        {
            m_BuildConfirmationBar.Hide();
            m_PlacementProcess = null;
            Debug.Log("Foundation layer out: " + buildPosition);
        }
        else
        {
            RevertResources(m_PlacementProcess.GoldCost, m_PlacementProcess.WoodCost);
        }
    }

    private void RevertResources(int gold, int wood)
    {
        m_Gold += gold;
        m_Wood += wood;
    }
    private void CancelBuildPlacement()
    {
        m_BuildConfirmationBar.Hide();
        m_PlacementProcess.CleanUp();
        m_PlacementProcess = null;

    }

    private bool TryDeductResources(int goldCost, int woodCost)
    {
        if (m_Gold >= goldCost && m_Wood >= woodCost)
        {
            m_Gold -= goldCost;
            m_Wood -= woodCost;
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "Gold: " + m_Gold.ToString(), new GUIStyle { fontSize = 14 });
        GUI.Label(new Rect(20, 40, 200, 20), "Wood: " + m_Wood.ToString(), new GUIStyle { fontSize = 14 });
    }
}
