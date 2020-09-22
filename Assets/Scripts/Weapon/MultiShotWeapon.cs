using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotWeapon : BulletWeapon
{
    #region //inspector
    [Range(1, 128)] public int BulletsFired = 3;
    [Range(0f, 90f)] public float SpreadAngleDeg = 45f;
    #endregion

    private void Awake()
    {
        m_bulletPool = new SimplePool(20, BulletPrefab);
        m_shootCallback = Shoot;
    }

    public override string Name => "Multi Shot";

    protected override void Shoot()
    {
        float angSpread = SpreadAngleDeg * Mathf.Deg2Rad;

        // angle between each bullet
        float angBetweenBullets = BulletsFired == 1 ? 0 : angSpread / (BulletsFired - 1f);

        // offset to center bullets
        float angOffset = BulletsFired == 1 ? 0 : angSpread / 2f;
        for (int i = 0; i < BulletsFired; i++)
        {
            // angle for this bullet
            float angle = angBetweenBullets * i - angOffset;

            // angle to direction
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var shot = m_bulletPool.GetFromPool();
            //shot.gameObject.SetActive(true);
            shot.transform.position = transform.position + (Vector3)BulletSpawnOffsetPosition;
            var bulletScript = shot.GetComponent<Bullet>();
            bulletScript.RigidBody.velocity = pos.yx() * bulletScript.BulletSpeedProp;
        }

    }
  
}
