using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotWeapon : BulletWeapon
{

    //public int BulletsFired = 3;

    [Range(1, 128)] public int bulletsFired = 3;
    [Range(0f, 90f)] public float spreadAngleDeg = 45f;

    private void Awake()
    {
        bulletPool = new SimplePool(20, Bullet);
        shootCallback = Shoot;
    }

    public override string Name => "Multi Shot";

    protected override void Shoot()
    {
        float angSpread = spreadAngleDeg * Mathf.Deg2Rad;

        // angle between each bullet
        float angBetweenBullets = bulletsFired == 1 ? 0 : angSpread / (bulletsFired - 1f);

        // offset to center bullets
        float angOffset = bulletsFired == 1 ? 0 : angSpread / 2f;
        for (int i = 0; i < bulletsFired; i++)
        {
            // angle for this bullet
            float angle = angBetweenBullets * i - angOffset;

            // angle to direction
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var shot = bulletPool.getFromPool();
            //shot.gameObject.SetActive(true);
            shot.transform.position = transform.position + (Vector3)bulletSpawnOffsetPosition;
            var bulletScript = shot.GetComponent<Bullet>();
            bulletScript.rigidBody.velocity = pos.yx() * bulletScript.BulletSpeed;
        }

    }
  
}
