
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    //this object belongs to this pool
    public SimplePool myPool;

    //return to the pool and become inactive
    public void returnToPool()
    {
        myPool.addToPool(this);
        gameObject.SetActive(false);
    }

}

public class Example : MonoBehaviour
{
    private Example getThisComponent()
    {
        return this;
    }
}