using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : ScriptableObject
{
    public enum MonsterType
    {
        Normal = 0, // �Ϲ� ���� 
        Boss = 1    // ���� ����
    }

    [SerializeField] int monsterId;                                                    // ���� ID
    [SerializeField] string monsterName;                                               // ���� �̸�

    [SerializeField] protected MonsterType type;                                       // ���� �з�

    [SerializeField] int level = 1;                                                    // ����

    [SerializeField] float hp;                                                         // ü��
    float currentHp;                                                                   // ���� ü��

    [SerializeField] float moveSpeed;                                                  // �̵� �ӵ�

    float preemptiveAttackMaxLevel;                                                    // ���� �� �÷��̾� �ִ� ���� (�ش� ������ ����� �İ����� ����)
    bool isPreemptiveAttack = true;                                                    // ���� ���� �÷���

    [SerializeField] protected float sensingMaxRange;                                  // �÷��̾� ���� ���� �ִ밪
    //[SerializeField] float chaseMaxRange;                                            // �÷��̾� ���� ���� �ִ밪

    [SerializeField] float reGenerationTime;                                           // ����� �ð�
    bool isWaitReGenTime;

    [SerializeField] float exp;                                                        // óġ �� ��� �Ǵ� ����ġ
    [SerializeField] int gold;                                                         // óġ �� ��� �Ǵ� ��ȭ

    [SerializeField] List<GameObject> dropItemPrefabs = new List<GameObject>();        // óġ �� ����ϴ� ������ ������
    [SerializeField] List<float> itemDropProbability  = new List<float>();             // ������ ��� Ȯ��

    public int MonsterId
    {
        get { return monsterId; }
    }

    public string MonsterName
    { 
        get { return monsterName; }
    }

    public MonsterType Type
    {
        get { return type; }
    }

    public int Level
    {
        get { return level; }
    }

    public float Hp
    {
        get { return hp; }
    }

    public float CurrentHp
    {
        get { return currentHp; }
        set
        { 
            currentHp = value;

            // 0�� ������ ü�� ���� ���̰��� �ǵ��� ����
            currentHp = Mathf.Clamp(currentHp, 0, Hp);
        }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float RunSpeed
    { 
        get { return moveSpeed * 2.0f; }
    }

    public float ReturnSpeed
    {
        get { return RunSpeed * 2.0f; }
    }

    public float PreemptiveAttackMaxLevel
    {
        get
        {
            // ���� �� �÷��̾� �ִ� ������ ���� ������ 10����, �ʰ��Ǹ� �İ����� ����
            preemptiveAttackMaxLevel = level + 10;

            return preemptiveAttackMaxLevel;
        }
    }

    public bool IsPreemptiveAttack
    {
        get { return isPreemptiveAttack; }
        set { isPreemptiveAttack = value; }
    }

    public float SensingMaxRange
    {
        get { return sensingMaxRange; }
    }

    //public float ChaseMaxRange
    //{
    //    get { return chaseMaxRange; }
    //}

    public float ReGenerationTime
    {
        get { return reGenerationTime; }
    }

    public bool IsWaitReGenTime
    { 
        get { return isWaitReGenTime; }
        set { isWaitReGenTime = value; }
    }

    public float Exp
    { 
        get { return exp; }
    }

    public int Gold
    {
        get { return gold; }
    }

    public List<GameObject> DropItemPrefabs
    {
        get { return dropItemPrefabs; }
    }

    public List<float> ItemDropProbability
    {
        get { return itemDropProbability; }
    }
}
