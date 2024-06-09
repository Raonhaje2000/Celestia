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

    // �⺻ �ִϸ��̼����� ��ȯ
    public void ChangeIdleAnimation()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // �޸��� �ִϸ��̼����� ��ȯ
    public void ChangeRunAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", true);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // ���� �ִϸ��̼����� ��ȯ
    public void ChangeDashAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", true);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // ����(������) �ִϸ��̼����� ��ȯ
    public void ChangeBiteAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", true);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // ����(���� �ֵθ���) �ִϸ��̼����� ��ȯ
    public void ChangeTailSwingAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", true);
        animator.SetBool("Skill_JumpAttack", false);
    }

    // ����(�������) �ִϸ��̼����� ��ȯ
    public void ChangeJumpAttackAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Skill_Bite", false);
        animator.SetBool("Skill_TailSwing", false);
        animator.SetBool("Skill_JumpAttack", true);
    }

    // �ǰ� �ִϸ��̼����� ��ȯ
    public void ChangeHitAnimation()
    {
        animator.SetTrigger("Hit");
    }

    // ���� �ִϸ��̼����� ��ȯ
    public void ChangeDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    // ���� ��� ���� �ִϸ��̼��� ���� ��ȯ
    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public bool CheckAnimation(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);                                                              
    }
}
