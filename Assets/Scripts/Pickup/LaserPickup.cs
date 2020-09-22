using UnityEngine;

public class LaserPickup : WeaponPickup
{
    #region //inspector
    [SerializeField] private int Damage = 3;
    [SerializeField] private float FireRate = 12f;
    [SerializeField] private float BeamLength = 12f;
    [SerializeField] private float MaxBeamWidth = 0.35f;
    [SerializeField] private float MinBeamWidth = 0.2f;
    [SerializeField] private Material BeamMaterial = null;
    [SerializeField] private Color BeamColor = Color.white;
    #endregion


    public override void PickUpWeapon(PlayerController playerController)
    {
        playerController.AddWeapon<LaserWeapon>()
            .Init(Damage, FireRate, BeamLength, MaxBeamWidth, MinBeamWidth, BeamMaterial, BeamColor);
        if (lifetimer != null)
        {
            StopCoroutine(lifetimer);
        }
        Destroy(gameObject);
    }
}
