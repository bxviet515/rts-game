using UnityEngine;

public class PointToClick : MonoBehaviour
{
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private AnimationCurve m_ScaleCurve;
    private float m_Timer;
    private Vector3 m_InitialScale;
    private float m_FreqTimer;
    private PointToClickPool m_Pool;

    public void Init(PointToClickPool pool)
    {
        m_Pool = pool;
        m_Timer = 0;
        m_SpriteRenderer.color = Color.white;
        gameObject.SetActive(true);
    }

    private void Start() {
        m_InitialScale = transform.localScale;
    }
    private void Update()
    {
        m_Timer += Time.deltaTime;
        m_FreqTimer += Time.deltaTime;
        m_FreqTimer %= 1f;
        float scaleMultiplier = m_ScaleCurve.Evaluate(m_FreqTimer);
        transform.localScale = m_InitialScale * scaleMultiplier;
        if (m_Timer >= m_Duration * 0.9f)
        {
            float fadeProgress = (m_Timer - m_Duration * 0.9f) / (m_Duration * 0.1f);
            m_SpriteRenderer.color = new Color(1, 1, 1, 1 - fadeProgress);
        }
        if (m_Timer >= m_Duration)
        {
            m_Pool.ReturnToPool(this);
        }
    }
}
