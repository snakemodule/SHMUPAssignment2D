using UnityEngine;

public class LaserPickup : WeaponPickup
{
    [SerializeField] private int damage = 3;
    [SerializeField] private float fireRate = 12f;
    [SerializeField] private float beamLength = 12f;
    [SerializeField] private float maxBeamWidth = 0.35f;
    [SerializeField] private float minBeamWidth = 0.2f;
    [SerializeField] private Material beamMaterial = null;
    [SerializeField] private Color beamColor = Color.white;

    public override void PickUpWeapon(PlayerController playerController)
    {
        playerController.AddWeapon<LaserWeapon>()
            .Init(damage, fireRate, beamLength, maxBeamWidth, minBeamWidth, beamMaterial, beamColor);
        if (lifetimer != null)
        {
            StopCoroutine(lifetimer);
        }
        Destroy(gameObject);
    }
}
