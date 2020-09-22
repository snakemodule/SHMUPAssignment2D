using UnityEngine;

public class MissilePickup : WeaponPickup
{
    #region //inspector
    [SerializeField] private PooledObject MissilePrefab = null;
    [SerializeField] private PooledObject LockOnIcon = null;
    [SerializeField] private float LockOnDistance = 15f;
    [SerializeField] private float LaunchImpulse = 0.5f;
    [SerializeField] private Material BeamMaterial = null;
    [SerializeField] private Color BeamColor = Color.white;
    #endregion

    public override void PickUpWeapon(PlayerController playerController)
    {
        playerController.AddWeapon<HomingMissileWeapon>()
            .Init(MissilePrefab, LockOnDistance, LaunchImpulse, BeamMaterial, BeamColor, LockOnIcon);
        if (lifetimer != null)
        {
            StopCoroutine(lifetimer);
        }
        Destroy(gameObject);
    }
}
