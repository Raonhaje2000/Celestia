using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistrisAnimation : MonoBehaviour
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
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // 달리기 애니메이션으로 전환
    public void ChangeRunAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", true);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // 돌진 애니메이션으로 전환
    public void ChangeDashAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", true);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // 공격(물어뜯기) 애니메이션으로 전환
    public void ChangeBiteAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", true);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // 공격(꼬리 휘두르기) 애니메이션으로 전환
    public void ChangeTailSwingAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", true);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // 공격(내려찍기) 애니메이션으로 전환
    public void ChangeJumpAttackAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", true);
    }

    // 피격 애니메이션으로 전환
    public void ChangeHitAnimation()
    {
        animator.SetTrigger("Hit");
    }

    // 죽음 애니메이션으로 전환
    public void ChangeDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    // 현재 재생 중인 애니메이션의 길이 반환
    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public bool CheckAnimation(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);                                                              
    }
}
