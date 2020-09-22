
using System.Collections;
using UnityEngine;

public abstract class WeaponPickup : MonoBehaviour
{
    public abstract void PickUpWeapon(PlayerController playerController);

    [SerializeField] private float lifetime = 10;

    protected Coroutine lifetimer = null;

    private void OnEnable()
    {
        StartCoroutine(lifetimeWait(lifetime));
    }

    private IEnumerator lifetimeWait(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}