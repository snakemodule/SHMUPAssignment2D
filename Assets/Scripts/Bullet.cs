using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;


    public float bulletSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //todo deal damage
        gameObject.SetActive(false);
    }
}
