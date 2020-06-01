using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float deathTime;
    //Explosion particle
    private void Start()
    {
        Invoke("Death",deathTime);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
