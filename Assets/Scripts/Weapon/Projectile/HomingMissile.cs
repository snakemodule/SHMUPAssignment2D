using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    #region //exposed
    public Transform TargetLock { private get; set; }
    public Rigidbody2D Body { get => m_body; private set => m_body = value; }
    #endregion

    #region //Awake
    private Rigidbody2D m_body = null;
    private PooledObject m_pooled = null;
    #endregion

    #region //inspector
    [SerializeField] private int Damage = 5;
    [SerializeField] private float MissileThrustForce = 0.5f;
    [SerializeField] private float LingerLifetime = 1f;
    [SerializeField] private float ChaseDelay = 0.5f;
    #endregion

    #region //internal
    private Coroutine m_chasing = null;
    private float m_defaultDrag = 1f;
    #endregion

    private void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_pooled = GetComponent<PooledObject>();
        m_defaultDrag = m_body.drag;
    }

    private void OnEnable()
    {
        m_body.drag = m_defaultDrag;
    }

    public IEnumerator ChaseTargetLock()
    {
        yield return new WaitForSeconds(ChaseDelay);
        while (TargetLock.gameObject.activeSelf)
        {
            Debug.DrawLine(transform.position, TargetLock.position - transform.position);
            transform.rotation = Quaternion.LookRotation(
                Vector3.back, TargetLock.position - transform.position);

            m_body.AddForce(transform.up * MissileThrustForce, ForceMode2D.Force);
            yield return null;
        }
        var saveDrag = m_body.drag;
        m_body.drag = 0f;
        yield return new WaitForSeconds(LingerLifetime);
        m_body.drag = saveDrag;
        DestroyMissile();
    }

    private void DestroyMissile()
    {
        m_pooled.ReturnToPool();
    }

    public void StartChase()
    {
        m_chasing = StartCoroutine(ChaseTargetLock());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>().Damage(Damage);
        StopCoroutine(m_chasing);
        DestroyMissile();
    }

}
