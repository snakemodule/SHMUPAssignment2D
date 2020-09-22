using System.Collections.Generic;
using UnityEngine;


public class SimplePool
{
    public PooledObject Prefab = null;
    private Stack<PooledObject> objectPool = null;

    public SimplePool(int count, PooledObject prefab)
    {
        Prefab = prefab;
        fillPool(count);
    }

    private void fillPool(int count)
    {
        objectPool = new Stack<PooledObject>(count);
        for (int i = 0; i < count; i++)
        {
            PooledObject instance = GameObject.Instantiate(Prefab);
            instance.myPool = this;
            objectPool.Push(instance);
        }
    }

    public void addToPool(PooledObject obj)
    {
        objectPool.Push(obj);
    }
    
    public PooledObject getFromPool()
    {
        if (objectPool.Count > 0)
        {
           return objectPool.Pop();
        }
        PooledObject instance = GameObject.Instantiate(Prefab);
        instance.myPool = this;
        return instance;
    }
}

