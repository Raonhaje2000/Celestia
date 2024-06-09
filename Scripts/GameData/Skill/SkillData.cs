using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public enum SkillUserType { Player = 0, Monster = 1 }

    [SerializeField] protected SkillUserType userType; // ����� (������)

    [SerializeField] int skillId;                      // ��ų ID
    [SerializeField] string skillName;                 // ��ų��

    [SerializeField] float duration;                   // ���� �ð�
    [SerializeField] float afterDelay;                 // ��� �� ������ (��ų ��� �� �������� ���ϴ� �ð�)

    [SerializeField]
    float cooldownTime;                                // ���� ���ð� (��Ÿ��)
    bool isCooldown;                                   // ���� ���ð� (��Ÿ��) �÷���

    [SerializeField] float maxRange;                   // �ִ� ���� ���� (�ִ� ���� �Ÿ�)

    [SerializeField] GameObject skillEffectPrefab;     // ��ų ����Ʈ ������
    //[SerializeField] GameObject attackEffectPrefab;    // �ǰ� ����Ʈ ������

    // ��ų �������� ��� �÷��̾� ��ų�� ���� ��ų�� ������ ���� ���(������ ���� ������)�� �ٸ��Ƿ� �� �ڽ� Ŭ�������� �ٷ�

    public SkillUserType UserType
    {
        get { return userType; }
    }

    public int SkillId
    {
        get { return skillId; }
    }

    public string SkillName
    {
        get { return skillName; }
    }

    public float Duration
    { 
        get { return duration; }
    }

    public float AfterDelay
    {
        get { return afterDelay; }
    }

    public float CooldownTime
    {
        get { return cooldownTime; }
    }

    public bool IsCooldown
    { 
        get { return isCooldown; }
        set { isCooldown = value; }
    }

    public float MaxRange
    {
        get { return maxRange; }
    }

    public GameObject SkillEffectPrefab
    {
        get { return skillEffectPrefab; }
    }

    //public GameObject AttackEffectPrefab
    //{
    //    get { return attackEffectPrefab; }
    //}
}
