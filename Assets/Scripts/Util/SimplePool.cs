using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SimplePool
{
    public PooledObject Prefab = null;
    //private Stack<PooledObject> objectPool = null;
    private Queue<PooledObject> objectPool = null;

    //private List<>

    private Action<PooledObject> instanceInitializer = null;

    public SimplePool(int count, PooledObject prefab)
    {
        Prefab = prefab;
        fillPool(count);
    }

    public SimplePool(int count, PooledObject prefab, Action<PooledObject> initializer)
    {
        Prefab = prefab;
        instanceInitializer = initializer;
        fillPool(count);
    }

    private void fillPool(int count)
    {
        objectPool = new Queue<PooledObject>(count);
        for (int i = 0; i < count; i++)
        {
            objectPool.Enqueue(initPooled());
        }
    }

    public void addToPool(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }

    private PooledObject initPooled()
    {
        PooledObject instance = null;
        instance = GameObject.Instantiate(Prefab);
        if (instanceInitializer != null)
            instanceInitializer(instance);
        instance.myPool = this;
        return instance;
    }

    public PooledObject getFromPool()
    {
        PooledObject instance = null;
        if (objectPool.Count > 0)
        {
            instance = objectPool.Dequeue();
        }
        else
        {
            instance = initPooled();
        }

        instance.gameObject.SetActive(true);
        //Assert.IsTrue(instance.gameObject.activeSelf);
        return instance;
    }
}

