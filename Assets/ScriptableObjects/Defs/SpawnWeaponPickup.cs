using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnWeaponPickupEvent", menuName = "ScriptableObjects/LevelEvents/SpawnWeaponPickupEvent", order = 1)]
public class SpawnWeaponPickup : ScriptableObject, ILevelEvent
{
    public WeaponPickup pickupPrefab = null;
    public Vector2 spawnPoint = Vector2.zero;
    public Vector2 velocity = Vector2.zero;


    public void DoEvent(LevelManager levelManager)
    {
        LevelManager.Instantiate(pickupPrefab, spawnPoint, quaternion.identity)
            .GetComponent<Rigidbody2D>().velocity = velocity;
    }

}
