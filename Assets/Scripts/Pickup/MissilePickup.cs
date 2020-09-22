using UnityEngine;

public class MissilePickup : WeaponPickup
{
    [SerializeField] private PooledObject MissilePrefab = null;
    [SerializeField] private PooledObject lockOnIcon = null;
    [SerializeField] private float lockOnDistance = 15f;
    [SerializeField] private float launchImpulse = 0.5f;
    [SerializeField] private Material beamMaterial = null;
    [SerializeField] private Color beamColor = Color.white;

    public override void PickUpWeapon(PlayerController playerController)
    {
        playerController.AddWeapon<HomingMissileWeapon>()
            .Init(MissilePrefab, lockOnDistance, launchImpulse, beamMaterial, beamColor, lockOnIcon);
        if (lifetimer != null)
        {
            StopCoroutine(lifetimer);
        }
        Destroy(gameObject);
    }
}
