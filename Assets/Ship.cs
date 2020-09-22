using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    [SerializeField] private int HullHP;

    private PlayerController playerController;

    private void Awake()
    {
        GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //fucking die
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

}
