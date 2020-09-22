using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIcon : MonoBehaviour
{
    public Transform followTarget = null;

    // Update is called once per frame
    void Update()
    {

        transform.position = followTarget.position;

    }
}
