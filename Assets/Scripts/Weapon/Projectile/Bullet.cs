using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region //inspector

    [SerializeField] private float BulletSpeed = 5f;
    [SerializeField] private int Damage = 5;
    [SerializeField] private float Lifetime = 3;
    #endregion

    #region //exposed
    public float BulletSpeedProp { get => BulletSpeed; private set => BulletSpeed = value; }
    public Rigidbody2D RigidBody { get => m_rigidBody; private set => m_rigidBody = value; }
    #endregion


    #region //Awake
    private PooledObject m_pooled = null;
    private Rigidbody2D m_rigidBody;
    #endregion


    private void Awake()
    {
        m_pooled = GetComponent<PooledObject>();
        RigidBody = GetComponent<Rigidbody2D>();
        RigidBody.velocity = new Vector2(0, BulletSpeedProp);
    }

    private void OnEnable()
    {
        StartCoroutine(lifeTimer());
    }

    private IEnumerator lifeTimer()
    {
        yield return new WaitForSeconds(Lifetime);
        m_pooled.ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Enemy>().Damage(Damage);
        m_pooled.ReturnToPool();
    }





}
