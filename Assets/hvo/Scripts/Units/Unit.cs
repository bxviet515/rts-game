using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{

    [SerializeField] private ActionSO[] m_Actions;

    public bool isMoving;
    public bool isTargeted;
    protected Animator m_Animator;
    protected AIPawn m_AIPawn;
    protected SpriteRenderer m_SpriteRenderer;
    protected Material m_OriginalMaterial;
    protected Material m_HightlightMaterial;
    public ActionSO[] Actions => m_Actions; 
    protected void Awake()
    {
        if (TryGetComponent<Animator>(out var animator))
        {
            m_Animator = animator;
        }
        if (TryGetComponent<AIPawn>(out var aIPawn))
        {
            m_AIPawn = aIPawn;
        }

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalMaterial = m_SpriteRenderer.material;
        m_HightlightMaterial = Resources.Load<Material>("Materials/Outline");

    }
    public void MoveTo(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;
        m_SpriteRenderer.flipX = direction.x < 0;
        m_AIPawn.SetDestination(destination);
    }
    public void Select()
    {
        Highlight();
        isTargeted = true;
    }

    public void DeSelect()
    {
        UnHighlight();
        isTargeted = false;
    }

    private void Highlight()
    {
        m_SpriteRenderer.material = m_HightlightMaterial;
    }
    private void UnHighlight()
    {
        m_SpriteRenderer.material = m_OriginalMaterial;
    }
}
