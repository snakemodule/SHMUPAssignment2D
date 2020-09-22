using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public Transform TargetLock { private get; set; }
    public PooledObject LockOnIcon { private get; set; }

    private Rigidbody2D body = null;
    public Rigidbody2D Body { get => body; private set => body = value; }

    [SerializeField] private int damage = 5;
    [SerializeField] private float missileThrustForce = 0.5f; // make common projectile class?
    [SerializeField] private float lingerLifetime = 5f;
    [SerializeField] private float chaseDelay = 0.5f;

    private PooledObject pooled = null;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        pooled = GetComponent<PooledObject>();
    }
    

    public IEnumerator ChaseTargetLock()    
    {
        yield return new WaitForSeconds(chaseDelay);
        while (TargetLock.gameObject.activeSelf)
        {
            Debug.DrawLine(transform.position, TargetLock.position - transform.position);
            transform.rotation = Quaternion.LookRotation(                
                Vector3.back, TargetLock.position - transform.position);

            body.AddForce(transform.up*missileThrustForce, ForceMode2D.Force);
            yield return null;
        }
        LockOnIcon.returnToPool();
        var saveDrag = body.drag;
        body.drag = 0f;
        yield return new WaitForSeconds(lingerLifetime);
        body.drag = saveDrag;
        DestroyMissile(); 
    }

    private void DestroyMissile()
    {        
        pooled.returnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>().Damage(damage);
        LockOnIcon.returnToPool();
        DestroyMissile();
    }

}
