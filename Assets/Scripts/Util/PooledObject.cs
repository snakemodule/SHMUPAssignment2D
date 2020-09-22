
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    //this object belongs to this pool
    public SimplePool myPool;

    //return to the pool and become inactive
    public void returnToPool()
    {
        //gameObject.SetActive(false);
        myPool.addToPool(this);
    }
}
