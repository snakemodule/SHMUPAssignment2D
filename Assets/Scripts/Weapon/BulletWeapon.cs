using UnityEngine;

public class BulletWeapon : ContinuousFireWeapon
{
    #region //inspector
    [SerializeField] protected PooledObject BulletPrefab;
    [SerializeField] protected Vector2 BulletSpawnOffsetPosition = new Vector2(0f, 0.5f);
    #endregion

    #region //internal
    protected SimplePool m_bulletPool;
    #endregion

    public override string Name => "Standard";

    private void Awake()
    {
        m_bulletPool = new SimplePool(10, BulletPrefab);
        m_shootCallback = Shoot;
    }

    protected override void Shoot()
    {
        var shot = m_bulletPool.GetFromPool();        
        shot.transform.position = transform.position + (Vector3)BulletSpawnOffsetPosition;
        var bulletScript = shot.GetComponent<Bullet>();
        bulletScript.RigidBody.velocity = new Vector2(0f, bulletScript.BulletSpeedProp);
    }
}

