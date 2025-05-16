using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public bool isMoving;
    protected Animator m_Animator;
    protected AIPawn m_AIPawn;
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


    }
    public void MoveTo(Vector3 destination)
    {
        m_AIPawn.SetDestination(destination);
    }
}
