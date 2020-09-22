using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PooledObjects that leave the bounds are returned to their pool.
/// </summary>
public class GameBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<PooledObject>()?.returnToPool();
    }
    
}
