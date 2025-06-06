using UnityEngine;
[CreateAssetMenu(fileName = "BuildAction", menuName = "HvO/Actions/BuildAction")]
public class BuildActionSO : ActionSO
{
    [SerializeField] private Sprite m_PlacementSprite;
    [SerializeField] private Sprite m_FoundationSprite;
    [SerializeField] private Sprite m_CompletionSprite;
    [SerializeField] private int m_GoldCost;
    [SerializeField] private int m_WoodCost;
    [SerializeField] private Vector3Int m_BuildingSize;
    [SerializeField] private Vector3Int m_OriginOffset;
    public Sprite PlacementSprite => m_PlacementSprite;
    public Sprite FoundationSprite => m_FoundationSprite;
    public Sprite CompletionSprite => m_CompletionSprite;
    public Vector3Int BuildingSize => m_BuildingSize;
    public Vector3Int OriginOffSet => m_OriginOffset;
    public int GoldCost => m_GoldCost;
    public int WoodCost => m_WoodCost;
    public override void Execute(GameManager gameManager)
    {
        gameManager.StartBuildProcess(this);
    }
}