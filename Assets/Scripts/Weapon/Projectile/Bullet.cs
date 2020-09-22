using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int damage = 5;

    [SerializeField] private float lifetime = 3;

    public float BulletSpeed { get => bulletSpeed; private set => bulletSpeed = value; }
    public Rigidbody2D rigidBody { get; private set; }

    private PooledObject pooled = null;

    private void Awake()
    {
        pooled = GetComponent<PooledObject>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(0, BulletSpeed);
    }

    private void OnEnable()
    {
        StartCoroutine(lifeTimer());
    }

    private IEnumerator lifeTimer()
    {
        yield return new WaitForSeconds(lifetime);
        pooled.returnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Enemy>().Damage(damage);
        pooled.returnToPool();
    }

 



}
