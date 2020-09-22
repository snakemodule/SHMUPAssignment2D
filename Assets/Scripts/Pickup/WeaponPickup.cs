
using System.Collections;
using UnityEngine;

public abstract class WeaponPickup : MonoBehaviour
{
    [SerializeField] private float lifetime = 10;

    protected Coroutine lifetimer = null;

    public abstract void PickUpWeapon(PlayerController playerController);

    private void OnEnable()
    {
        StartCoroutine(LifetimeWait(lifetime));
    }

    private IEnumerator LifetimeWait(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}