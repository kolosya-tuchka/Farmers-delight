using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EnemyAnimations : Animations
{
    NavMeshAgent2D agent;
    Rigidbody2D rig;
    public Animator animator;
    Enemy enemy;
    [HideInInspector] public bool isAttack, hit;
    public MMFeedbacks damageFeedback;
    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>();
        enemy = GetComponent<Enemy>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetBool("IsMoving", agent.velocity.magnitude > 0 || rig.velocity.magnitude > 0);

        if (enemy.hp.healPoints <= 0)
        {
            animator.SetBool("IsDie", true);
        }

        if (isAttack)
        {
            animator.SetTrigger("Attack");
            isAttack = false;
        }

        if (hit)
        {
            //StartCoroutine(Hit());
            damageFeedback.PlayFeedbacks();
            hit = false;
        }
    }

    
}
