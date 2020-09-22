using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    BoxCollider2D boundsCollider;


    private void Awake()
    {
        boundsCollider = GetComponent<BoxCollider2D>();
    }

}
