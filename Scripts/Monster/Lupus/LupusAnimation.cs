using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LupusAnimation : MonoBehaviour                                                                                            
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        ChangeIdleAnimation();
    }

    // 기본 애니메이션으로 전환
    public void ChangeIdleAnimation()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
    }

    // 걷기 애니메이션으로 전환
    public void ChangeWalkAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
    }

    // 달리기 애니메이션으로 전환 (추적, 추적 해제 상태일 때)
    public void ChangeRunAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);
    }

    // 공격 애니메이션으로 전환
    public void ChangeAttackAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", true);
    }

    // 피격 애니메이션으로 전환
    public void ChangeDamageAnimation()
    {
        animator.SetTrigger("Damage");
    }

    // 죽음 애니메이션으로 전환
    public void ChangeDieAnimation()
    {
        animator.SetTrigger("Die");
    }

    // 현재 재생 중인 애니메이션의 길이 반환
    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
