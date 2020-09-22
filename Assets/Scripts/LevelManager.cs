using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    Dictionary<PooledObject, SimplePool> PooledLevelObjects =
        new Dictionary<PooledObject, SimplePool>();

    //List<Enemy> activeEnemies = new List<Enemy>();
    //List<Enemy> toDeactivate = new List<Enemy>();

    [Serializable]
    public class TimelineEvent
    {
        public ScriptableObject Event = null;
        public float eventTime = 0;
        public int repeatAdditionalTimes = 0;
        public float repeatDelay = 0;
    }

    public List<TimelineEvent> timeline;

    private float levelStartTime;
    private int timelineIterator;

    private void Start()
    {
        levelStartTime = Time.time;
    }

    private void Update()
    {
        float levelTime = Time.time - levelStartTime;
        while (timelineIterator < timeline.Count
            && levelTime >= timeline[timelineIterator].eventTime)
        {
            TimelineEvent timelineEntry = timeline[timelineIterator];
            ILevelEvent levelEvent = (timelineEntry.Event as ILevelEvent);
            levelEvent.DoEvent(this);
            if (timelineEntry.repeatAdditionalTimes > 0)
            {
                StartCoroutine(repeatEvent(levelEvent, timelineEntry.repeatAdditionalTimes,
                    timelineEntry.repeatDelay));
            }
            timelineIterator++;
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

    private IEnumerator repeatEvent(ILevelEvent ev, int repeats, float delay)
    {
        Assert.IsTrue(repeats > 0);
        while (repeats > 0)
        {
            yield return new WaitForSeconds(delay);
            ev.DoEvent(this);
            repeats--;
        }
    }

    //private void UpdateEnemyPosition(Enemy enemy, float time)
    //{
    //    enemy.transform.position = new Vector2(
    //            enemy.movementPath.curveX.Evaluate(Time.time - enemy.SpawnTime),
    //            enemy.movementPath.curveY.Evaluate(Time.time - enemy.SpawnTime));
    //}

    public void SpawnEnemy(PooledObject prefab, PathNodes enemyPath)
    {
        Action<PooledObject> initializer = (PooledObject instance) =>
            { instance.GetComponent<Enemy>().movementPath = enemyPath; };

        if (!PooledLevelObjects.ContainsKey(prefab))
            PooledLevelObjects[prefab] = new SimplePool(20, prefab, initializer); //todo magic number
        PooledObject newEnemy = PooledLevelObjects[prefab].getFromPool();

        var enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.DeactivateCallback = (Enemy enemy) => { };
        //UpdateEnemyPosition(enemyScript, 0);

        //activeEnemies.Add(enemyScript);
    }

    public void DeactivateEnemy(Enemy enemy)
    {
        //toDeactivate.Add(enemy);
    }

    private void OnValidate()
    {
        for (int i = 0; i < timeline.Count; i++)
        {
            if (timeline[i].Event != null && !(timeline[i].Event is ILevelEvent))
            {
                timeline[i] = null;
                Debug.LogWarning("ScriptableObject in timeline must a level event (i.e. inherit from ILevelEvent)");
            }
        }
    }
}
