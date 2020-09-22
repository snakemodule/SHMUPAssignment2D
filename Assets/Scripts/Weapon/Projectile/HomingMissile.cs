using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    //private Transform targetLock;
    public Transform TargetLock { private get; set; }

    private Rigidbody2D body = null;

    [SerializeField] private float speed; // make common projectile class?

    [SerializeField] private float unlockedLifetime = 5;

    private PooledObject pooled = null;

    private Coroutine chasing = null;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        pooled = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        //if (chasing != null)
        //{
        //    StopCoroutine(chasing);
        //}
        //chasing = StartCoroutine(ChaseTargetLock());
    }

    public IEnumerator ChaseTargetLock()    
    {
                
        
        while (TargetLock.gameObject.activeSelf)
        {
            Debug.DrawLine(transform.position, TargetLock.position - transform.position);
            transform.rotation = Quaternion.LookRotation(
                TargetLock.position - transform.position,
                Vector3.forward);

            body.AddForce(transform.TransformDirection(transform.up), ForceMode2D.Force);
            yield return null;
        }
        //yield return new WaitForSeconds(unlockedLifetime);
        //pooled.returnToPool(); //todo death function
    }
    
}
