using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    #region //Awake
    private SpriteRenderer m_sprite;
    private PlayerController m_playerController;
    #endregion

    #region //inspector
    [SerializeField] private int HullHP = 3;
    [SerializeField] private float IframeDuration = 1f;
    [SerializeField] private float IframeFlashFrequency = 5f;
    [SerializeField] private GameObject Shield = null;
    [SerializeField] private float ShieldDuration = 2f;
    [SerializeField] private float ShieldCooldown = 5f;
    #endregion

    #region //internal
    private bool m_inIframe = false;
    private bool m_shieldIsActive;
    private bool m_shieldOnCooldown;
    #endregion

    private void Awake()
    {
        m_playerController = GetComponentInParent<PlayerController>();
        m_sprite = GetComponent<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && !m_inIframe && !m_shieldIsActive)
        {
            m_inIframe = true;
            HullHP--;
            if (HullHP <= 0)
            {
                //die
                gameObject.SetActive(false);
                m_playerController.enabled = false;
            }
            else
            {
                StartCoroutine(AnimateDamageColor(m_sprite));
                StartCoroutine(ActivateIframes(m_sprite));
            }
        }
        else if (collision.gameObject.layer == 10)
        {
            collision.GetComponent<WeaponPickup>().PickUpWeapon(m_playerController);
        }
    }

    public void ActivateShield()
    {
        if (!m_shieldIsActive && !m_shieldOnCooldown)
        {
            StartCoroutine(ShieldUse());
            StartCoroutine(ShieldCooldownCount());
        }
    }

    private IEnumerator ShieldUse()
    {
        m_shieldIsActive = true;
        Shield.SetActive(m_shieldIsActive);
        yield return new WaitForSeconds(ShieldDuration);
        m_shieldIsActive = false;
        Shield.SetActive(m_shieldIsActive);
    }

    private IEnumerator ShieldCooldownCount()
    {
        m_shieldOnCooldown = true;
        yield return new WaitForSeconds(ShieldCooldown);
        m_shieldOnCooldown = false;
    }

    private IEnumerator AnimateDamageColor(SpriteRenderer sprite)
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
    
    private IEnumerator ActivateIframes(SpriteRenderer sprite)
    {
        float startTime = Time.time;
        while (Time.time - startTime < IframeDuration)
        {
            var rad = Mathf.Lerp(0, IframeFlashFrequency * IframeDuration * 2 * Mathf.PI,
                Mathf.InverseLerp(startTime, startTime + IframeDuration, Time.time));
            if (Mathf.Sin(rad) >= 0)
                sprite.enabled = true;
            else
                sprite.enabled = false;
            yield return null;
        }
        m_inIframe = false;
        sprite.enabled = true;
    }
}
