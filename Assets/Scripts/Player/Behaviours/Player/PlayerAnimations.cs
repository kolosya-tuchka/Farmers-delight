using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerAnimations : MonoBehaviour
{
    public MMFeedbacks damageFeedback;
    [SerializeField] Animator animator;

    public void HitAhimation()
    {
        damageFeedback.PlayFeedbacks(transform.position);
    }

    public void MoveAnimation(Vector2 vel)
    {
        animator.SetBool("isMoving", vel != Vector2.zero);
    }
}
