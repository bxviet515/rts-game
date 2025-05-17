using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private PointToClick m_Prefab;
    [SerializeField] private int m_PoolSize = 10;

    private Queue<PointToClick> m_Pool = new();
    private void Awake()
    {
        for (int i = 0; i < m_PoolSize; i++)
        {
            PointToClick ptc = Instantiate(m_Prefab, transform);
            ptc.gameObject.SetActive(false);
            m_Pool.Enqueue(ptc);
        }
    }
    public void Spawn(Vector2 position)
    {
        PointToClick ptc = m_Pool.Count > 0 ? m_Pool.Dequeue() : Instantiate(m_Prefab);
        ptc.transform.position = position;
        ptc.gameObject.SetActive(true);
        ptc.Play(() => ReturnToPool(ptc));
    }

    private void ReturnToPool(PointToClick ptc)
    {
        ptc.gameObject.SetActive(false);
        m_Pool.Enqueue(ptc);
    }
}
