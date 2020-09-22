using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private SpriteRenderer sprite;

    private PlayerController playerController;

    [SerializeField] private int hullHP = 3;
    [SerializeField] private float iframeDuration = 1f;
    [SerializeField] private float iframeFlashFrequency = 5f;

    private bool inIframe = false;
    private bool shieldIsActive;
    private bool shieldOnCooldown;

    [SerializeField] private GameObject shield = null;
    [SerializeField] private float shieldDuration = 2f;
    [SerializeField] private float shieldCooldown = 5f;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && !inIframe && !shieldIsActive)
        {
            inIframe = true;
            hullHP--;
            if (hullHP <= 0)
            {
                //die
                gameObject.SetActive(false);
                playerController.enabled = false;
            }
            else
            {
                StartCoroutine(animateDamageColor(sprite));
                StartCoroutine(activateIframes(sprite));
            }
        }
        else if (collision.gameObject.layer == 10)
        {
            collision.GetComponent<WeaponPickup>().PickUpWeapon(playerController);
        }
    }

    public void ActivateShield()
    {
        if (!shieldIsActive && !shieldOnCooldown)
        {
            StartCoroutine(ShieldUse());
            StartCoroutine(ShieldCooldown());
        }
    }

    private IEnumerator ShieldUse()
    {
        shieldIsActive = true;
        shield.SetActive(shieldIsActive);
        yield return new WaitForSeconds(shieldDuration);
        shieldIsActive = false;
        shield.SetActive(shieldIsActive);
    }

    private IEnumerator ShieldCooldown()
    {
        shieldOnCooldown = true;
        yield return new WaitForSeconds(shieldCooldown);
        shieldOnCooldown = false;
    }


    private IEnumerator animateDamageColor(SpriteRenderer sprite)
    {
        float duration = 0.3f;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            sprite.color = Color.Lerp(Color.red, Color.white, Mathf.InverseLerp(startTime, startTime + duration, Time.time));
            yield return null;
        }
        sprite.color = Color.white;
    }

    

    private IEnumerator activateIframes(SpriteRenderer sprite)
    {
        float startTime = Time.time;
        while (Time.time - startTime < iframeDuration)
        {
            var rad = Mathf.Lerp(0, iframeFlashFrequency * iframeDuration * 2 * Mathf.PI,
                Mathf.InverseLerp(startTime, startTime + iframeDuration, Time.time));
            if (Mathf.Sin(rad) >= 0)
                sprite.enabled = true;
            else
                sprite.enabled = false;
            yield return null;
        }
        inIframe = false;
        sprite.enabled = true;
    }
}
