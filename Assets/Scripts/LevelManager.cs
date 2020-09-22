using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    #region //inspector, exposed for editor script
    [Serializable] public class TimelineEvent
    {
        public ScriptableObject Event = null;
        public float eventTime = 0;
        public int repeatAdditionalTimes = 0;
        public float repeatDelay = 0;
    }
    [SerializeField] public List<TimelineEvent> Timeline;
    #endregion

    #region //internal
    private readonly Dictionary<PooledObject, SimplePool> m_pooledLevelObjects =
        new Dictionary<PooledObject, SimplePool>(); 
    private float m_levelStartTime;
    private int m_timelineIterator;
    #endregion

    private void Start()
    {
        m_levelStartTime = Time.time;
    }

    private void Update()
    {
        float levelTime = Time.time - m_levelStartTime;
        while (m_timelineIterator < Timeline.Count
            && levelTime >= Timeline[m_timelineIterator].eventTime)
        {
            TimelineEvent timelineEntry = Timeline[m_timelineIterator];
            ILevelEvent levelEvent = (timelineEntry.Event as ILevelEvent);
            levelEvent.DoEvent(this);
            if (timelineEntry.repeatAdditionalTimes > 0)
            {
                StartCoroutine(RepeatEvent(levelEvent, timelineEntry.repeatAdditionalTimes,
                    timelineEntry.repeatDelay));
            }
            m_timelineIterator++;
        }

        //for (int i = activeEnemies.Count - 1; i > 0; i--)
        //{
        //    if (toDeactivate.Contains(activeEnemies[i]))
        //    {
        //        activeEnemies[i].pooled.returnToPool();
        //        activeEnemies.RemoveAt(i);
        //    }
        //}
        //toDeactivate.Clear();

        //foreach (var enemy in activeEnemies)
        //{
        //    var t = enemy.transform.position;
        //    UpdateEnemyPosition(enemy, Time.time - enemy.SpawnTime);
        //    if (Vector3.Distance(enemy.transform.position, t) > 0.5f)
        //    {
        //        Debug.LogError("discontinuity");
        //    }
        //}
    }

    private IEnumerator RepeatEvent(ILevelEvent ev, int repeats, float delay)
    {
        Assert.IsTrue(repeats > 0);
        while (repeats > 0)
        {
            yield return new WaitForSeconds(delay);
            ev.DoEvent(this);
            repeats--;
        }
    }    

    public void SpawnEnemy(PooledObject prefab, PathNodes enemyPath)
    {        
        if (!m_pooledLevelObjects.ContainsKey(prefab))
        {            
            m_pooledLevelObjects[prefab] = new SimplePool(20, prefab);
        }
        PooledObject spawnEnemy = m_pooledLevelObjects[prefab].GetFromPool();
        spawnEnemy.GetComponent<Enemy>().MovementPath = enemyPath;
    }

    private void OnValidate()
    {
        for (int i = 0; i < Timeline.Count; i++)
        {
            if (Timeline[i].Event != null && !(Timeline[i].Event is ILevelEvent))
            {
                Timeline[i] = null;
                Debug.LogWarning("ScriptableObject in timeline must a level event (i.e. inherit from ILevelEvent)");
            }
        }
    }
}
