using System;
using System.Collections.Generic;
using UnityEngine;

public class SimplePool
{
    public PooledObject Prefab = null;

    private Queue<PooledObject> m_objectPool = null;
    private Action<PooledObject> m_instanceInitializer = null;

    public SimplePool(int count, PooledObject prefab)
    {
        Prefab = prefab;
        FillPool(count);
    }

    public SimplePool(int count, PooledObject prefab, Action<PooledObject> initializer)
    {
        Prefab = prefab;
        m_instanceInitializer = initializer;
        FillPool(count);
    }

    private void FillPool(int count)
    {
        m_objectPool = new Queue<PooledObject>(count);
        for (int i = 0; i < count; i++)
        {
            m_objectPool.Enqueue(InitPooled());
        }
    }

    public void AddToPool(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        m_objectPool.Enqueue(obj);
    }

    private PooledObject InitPooled()
    {
        PooledObject instance = null;
        instance = GameObject.Instantiate(Prefab);
        m_instanceInitializer?.Invoke(instance);
        instance.myPool = this;
        return instance;
    }

    public PooledObject GetFromPool()
    {
        PooledObject instance = null;
        if (m_objectPool.Count > 0)
        {
            instance = m_objectPool.Dequeue();
        }
        else
        {
            instance = InitPooled();
        }

        instance.gameObject.SetActive(true);        
        return instance;
    }
}

