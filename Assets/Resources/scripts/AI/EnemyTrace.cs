using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    public ParticleSystem particle;
    NavMeshAgent2D agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>();
    }

    void Update()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            particle.Play();
        }
        else
        {
            particle.Stop();
        }
    }
}
