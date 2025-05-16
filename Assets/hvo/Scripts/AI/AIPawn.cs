using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPawn : MonoBehaviour
{
    [SerializeField] private float m_Speed = 3f;
    private Vector3? m_Destination;
    private Vector3? Destination => m_Destination;

    private void Start() {
        SetDestination(new Vector3(4.5f, 0, 0));
    }
    private void Update() {
        if (m_Destination.HasValue)
        {
            var dir = m_Destination.Value - transform.position;
            transform.position += dir.normalized * Time.deltaTime * m_Speed;
            var distinceToDestination = Vector3.Distance(transform.position, m_Destination.Value);
            if (distinceToDestination < 0.1f)
            {
                m_Destination = null;
            }
        }
    }
    public void SetDestination(Vector3 destination)
    {
        m_Destination = destination;
    }
}
