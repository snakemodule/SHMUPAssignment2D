
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public SimplePool myPool;

    public void ReturnToPool()
    {
        myPool.AddToPool(this);
    }
}
