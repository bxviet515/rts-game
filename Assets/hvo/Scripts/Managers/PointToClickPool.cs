using System.Collections.Generic;
using UnityEngine;

public class PointToClickPool : MonoBehaviour
{
    [SerializeField] private PointToClick m_Prefab;
    private Queue<PointToClick> m_Pool = new();

    public PointToClick Get()
    {
        PointToClick item;
        if (m_Pool.Count > 0)
        {
            item = m_Pool.Dequeue();
            item.gameObject.SetActive(true);
        }
        else
        {
            item = Instantiate(m_Prefab, transform);
        }
        return item;
    }

    public void ReturnToPool(PointToClick item)
    {
        item.gameObject.SetActive(false);
        m_Pool.Enqueue(item);
    }
}
