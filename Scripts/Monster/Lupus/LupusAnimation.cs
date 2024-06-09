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

    // �⺻ �ִϸ��̼����� ��ȯ
    public void ChangeIdleAnimation()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
    }

    // �ȱ� �ִϸ��̼����� ��ȯ
    public void ChangeWalkAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
    }

    // �޸��� �ִϸ��̼����� ��ȯ (����, ���� ���� ������ ��)
    public void ChangeRunAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);
    }

    // ���� �ִϸ��̼����� ��ȯ
    public void ChangeAttackAnimation()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", true);
    }

    // �ǰ� �ִϸ��̼����� ��ȯ
    public void ChangeDamageAnimation()
    {
        animator.SetTrigger("Damage");
    }

    // ���� �ִϸ��̼����� ��ȯ
    public void ChangeDieAnimation()
    {
        animator.SetTrigger("Die");
    }

    // ���� ��� ���� �ִϸ��̼��� ���� ��ȯ
    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
