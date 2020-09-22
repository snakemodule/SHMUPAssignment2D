using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEnemyEvent", menuName = "ScriptableObjects/LevelEvents/SpawnEnemyEvent", order = 1)]
class SpawnEnemyEvent : ScriptableObject, ILevelEvent
{
    public PooledObject enemyPrefab = null;
    public PathNodes enemyPath = null;

    public void DoEvent(LevelManager levelManager)
    {
        levelManager.SpawnEnemy(enemyPrefab, enemyPath);
    }
}

