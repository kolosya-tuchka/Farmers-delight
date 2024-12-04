using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    public ParticleSystem dust;
    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rigidbody.velocity.magnitude > 0.1f)
        {
            dust.Play();
        }
        else
        {
            dust.Stop();
        }
    }
}
