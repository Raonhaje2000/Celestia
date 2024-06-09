using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : ScriptableObject
{
    public enum MonsterType
    {
        Normal = 0, // 일반 몬스터 
        Boss = 1    // 보스 몬스터
    }

    [SerializeField] int monsterId;                                                    // 몬스터 ID
    [SerializeField] string monsterName;                                               // 몬스터 이름

    [SerializeField] protected MonsterType type;                                       // 몬스터 분류

    [SerializeField] int level = 1;                                                    // 레벨

    [SerializeField] float hp;                                                         // 체력
    float currentHp;                                                                   // 현재 체력

    [SerializeField] float moveSpeed;                                                  // 이동 속도

    float preemptiveAttackMaxLevel;                                                    // 선공 시 플레이어 최대 레벨 (해당 레벨을 벗어나면 후공으로 변경)
    bool isPreemptiveAttack = true;                                                    // 선공 유무 플래그

    [SerializeField] protected float sensingMaxRange;                                  // 플레이어 감지 범위 최대값
    //[SerializeField] float chaseMaxRange;                                            // 플레이어 추적 범위 최대값

    [SerializeField] float reGenerationTime;                                           // 재생성 시간
    bool isWaitReGenTime;

    [SerializeField] float exp;                                                        // 처치 시 얻게 되는 경험치
    [SerializeField] int gold;                                                         // 처치 시 얻게 되는 재화

    [SerializeField] List<GameObject> dropItemPrefabs = new List<GameObject>();        // 처치 시 드랍하는 아이템 프리팹
    [SerializeField] List<float> itemDropProbability  = new List<float>();             // 아이템 드랍 확률

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

            // 0과 몬스터의 체력 설정 사이값이 되도록 변경
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
            // 선공 시 플레이어 최대 레벨은 몬스터 레벨의 10이하, 초과되면 후공으로 변경
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
