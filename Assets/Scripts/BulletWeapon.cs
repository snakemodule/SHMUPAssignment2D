using System;
using System.Collections;
using UnityEngine;

public class BulletWeapon : ContinousFireWeapon
{
    public PooledObject Bullet;
    
    public Vector2 bulletSpawnOffsetPosition = new Vector2(0f, 0.5f);
    public float damage = 10;
        
    protected SimplePool bulletPool;

    private void Awake()
    {
        bulletPool = new SimplePool(10, Bullet);
        shootCallback = Shoot;
    }

    protected override void Shoot()
    {
        var shot = bulletPool.getFromPool();
        shot.gameObject.SetActive(true);
        shot.transform.position = transform.position + (Vector3)bulletSpawnOffsetPosition;
        var bulletScript = shot.GetComponent<Bullet>();
        bulletScript.rb.velocity = new Vector2(0f, bulletScript.bulletSpeed);
    }
}

