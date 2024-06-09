using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 계곡 숲 노말 몬스터 01 - 루푸스
public class Lupus : NormalMonster
{
    public enum LupusState
    {
        Idle = 0,     // 기본
        Walk,         // 걷기
        Chase,        // 추적
        ChaseFailure, // 추적 실패
        Attack,       // 공격
        Damage,       // 피격
        Die           // 죽음                                                   
    }

    [SerializeField] LupusState state;            // 루푸스 상태
    [SerializeField] LupusState prevDamageState;  // 피격 받기 전의 상태

    [SerializeField] NormalMonsterData lupusData; // 루푸스 데이터 (체력 관련 데이터 포함)

    // 컴포넌트로 추가된 클래스들
    LupusAnimation lupusAnimation;            // 루푸스 애니메이션 담당 클래스
    LupusAi lupusAi;                          // 루푸스 AI 담당 클래스

    LupusSkill_Ripping skill_Ripping;         // 루푸스 스킬(갈기갈기 찢기) 담당 클래스

    Transform player;                         // 플레이어 (타겟)
    Transform lupus;                          // 루푸스 (몬스터)

    [SerializeField] bool isPreemptiveAttack; // 선공 유무

    [SerializeField]
    Vector3 walkDestination;                  // 걷기 상태일 때의 목적지

    float walkDistanceMax;                    // 걷기로 이동하는 최대 거리

    [SerializeField]
    bool isWalkHalt;                          // 걷기가 중단됨 (걷기 도중 추적 상태로 전이됨)
    [SerializeField]
    bool isChaseHalt;

    [SerializeField]
    float currentHp;

    bool isWaitWalkRandomPosition;
    bool isWaitMove;
    bool isWateDamage;
    bool isWaitDieAnimation;

    bool isDie;   // 죽었을 때 한번만 처리되도록 하는 플래그
    bool isDamage;

    private void Awake()
    {
        // 관련 리소스 불러오기
        LoadResources();
    }

    void Start()
    {
        // 초기화
        Initialize();
    }

    void Update()
    {
        isPreemptiveAttack = (PlayerManager.instance.Level <= lupusData.PreemptiveAttackMaxLevel) ? true : false;
        currentHp = lupusData.CurrentHp;

        switch (state)
        {
            case LupusState.Idle:
                {
                    UpdateIdle();           // 기본 상태 업데이트
                    break;
                }
            case LupusState.Walk:
                {
                    UpdateWalk();           // 걷기 상태 업데이트
                    break;
                }
            case LupusState.Chase:
                {
                    UpdateChase();          // 추적 상태 업데이트
                    break;
                }
            case LupusState.ChaseFailure:
                {
                    UpdateChaseFailure();   // 추적 실패 상태 업데이트
                    break;
                }
            case LupusState.Attack:
                {
                    UpdateAttack();         // 공격 상태 업데이트
                    break;
                }
            case LupusState.Damage:
                {
                    UpdateDamage();         // 피격 상태 업데이트
                    break;
                }
            case LupusState.Die:
                {
                    UpdateDie();            // 죽음 상태 업데이트
                    break;
                }
        }
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        lupusData = ScriptableObject.Instantiate(Resources.Load<NormalMonsterData>("GameData/Monster/Field03_ValleyForest/LupusData"));

        lupusAnimation = GetComponent<LupusAnimation>();
        lupusAi = GetComponent<LupusAi>();
        skill_Ripping = GetComponent<LupusSkill_Ripping>();

        player = GameObject.FindWithTag("Player").transform;
        lupus = this.transform;
    }

    // 초기화
    void Initialize()
    {
        state = LupusState.Idle;

        isPreemptiveAttack = true; // 기본적으로 선공

        walkDestination = lupus.position;

        walkDistanceMax = 5.0f;

        isWalkHalt = false;

        lupusData.CurrentHp = lupusData.Hp; // 몬스터의 현재 체력을 몬스터 체력 최대값으로 초기화

        // 몬스터 AI 속력 세팅
        lupusAi.SetSpeeds(lupusData.MoveSpeed, lupusData.RunSpeed, lupusData.ReturnSpeed);
    }

    // 기본 상태 업데이트
    void UpdateIdle()
    {
        if (isChaseHalt)
        {
            lupusAi.MoveBeforeChasePosition();
            lupusAnimation.ChangeRunAnimation();
            state = LupusState.ChaseFailure;
        }
        else
        {
            if (CalcPlayerDistance() <= lupusData.SensingMaxRange)
            {
                if (isPreemptiveAttack || (!isPreemptiveAttack && lupusData.CurrentHp < lupusData.Hp))
                {
                    // 선공형인 경우
                    // 또는 후공형인데 피격 당해 체력이 깎인 경우
                    lupusAi.SetBeforeChasePosition();
                    lupusAi.MoveChaseDestination(player.position);
                    lupusAnimation.ChangeRunAnimation();
                    state = LupusState.Chase;
                }
                else
                {
                    if (isWalkHalt)
                    {
                        lupusAnimation.ChangeWalkAnimation();
                        state = LupusState.Walk;
                    }
                    else
                    {
                        StartCoroutine(WaitWalkRandomPosition());
                    }
                }
            }
            else
            {
                if (isWalkHalt)
                {
                    lupusAnimation.ChangeWalkAnimation();
                    state = LupusState.Walk;
                }
                else
                {
                    StartCoroutine(WaitWalkRandomPosition());
                }
            }
        }
    }

    // 랜덤한 위치로 주변 배회하기
    IEnumerator WaitWalkRandomPosition()
    {
        if(!isWaitWalkRandomPosition)
        {
            isWaitWalkRandomPosition = true;

            yield return new WaitForSeconds(2.0f);

            isWaitWalkRandomPosition = false;

            float x = Random.Range(-walkDistanceMax, walkDistanceMax);
            float z = Random.Range(-walkDistanceMax, walkDistanceMax);

            walkDestination = lupus.position + new Vector3(x, 0, z);

            lupusAi.SetBeforeWalkPosition();
            lupusAi.MoveWalkDestination(walkDestination);
            lupusAnimation.ChangeWalkAnimation();

            state = LupusState.Walk;

            StopCoroutine("RandomPosition");
        }
    }

    // 걷기 상태 업데이트
    void UpdateWalk()
    {
        if (CalcPlayerDistance() <= lupusData.SensingMaxRange)
        {
            if (isPreemptiveAttack || (!isPreemptiveAttack && lupusData.CurrentHp < lupusData.Hp))
            {
                // 선공형인 경우
                // 또는 후공형인데 피격 당해 체력이 깎인 경우
                isWalkHalt = true;

                lupusAi.SetBeforeChasePosition();
                lupusAi.MoveChaseDestination(player.position);
                lupusAnimation.ChangeRunAnimation();
                state = LupusState.Chase;
            }
            else
            {
                if (isWalkHalt)
                {
                    lupusAi.MoveBeforeWalkPosition();

                    if (lupusAi.ArriveBeforeWalkPosition())
                    {
                        isWalkHalt = false;

                        lupusAnimation.ChangeIdleAnimation();
                        state = LupusState.Idle;
                    }
                }
                else
                {
                    if (lupusAi.ArriveWalkDestination()) StartCoroutine(WaitMove());

                    if (lupusAi.ArriveBeforeWalkPosition())
                    {
                        lupusAnimation.ChangeIdleAnimation();
                        state = LupusState.Idle;
                    }
                }
            }
        }
        else
        {
            if (isWalkHalt)
            {
                lupusAi.MoveBeforeWalkPosition();

                if (lupusAi.ArriveBeforeWalkPosition())
                {
                    isWalkHalt = false;

                    lupusAnimation.ChangeIdleAnimation();
                    state = LupusState.Idle;
                }
            }
            else
            {
                if (lupusAi.ArriveWalkDestination()) StartCoroutine(WaitMove());

                if (lupusAi.ArriveBeforeWalkPosition())
                {
                    lupusAnimation.ChangeIdleAnimation();
                    state = LupusState.Idle;
                }
            }
        }
    }

    // 이동하던 것을 멈추고 잠시 대기 후 원래 위치로 되돌아가기
    IEnumerator WaitMove()
    {
        if (!isWaitMove)
        {
            isWaitMove = true;

            lupusAnimation.ChangeIdleAnimation();

            yield return new WaitForSeconds(2.0f);

            isWaitMove = false;

            lupusAnimation.ChangeWalkAnimation();
            lupusAi.MoveBeforeWalkPosition();                                                                                

            StopCoroutine("WaitMove");
        }
    }

    // 추적 상태 업데이트
    void UpdateChase()
    {
        lupusAi.MoveChaseDestination(player.position);

        // 플레이어가 감지 범위 밖으로 벗어난 경우
        if (CalcPlayerDistance() > lupusData.SensingMaxRange)
        {
            // 추적 직전 위치로 되돌아 가고 추적 실패 상태로 전이
            lupusAi.MoveBeforeChasePosition();
            lupusAnimation.ChangeRunAnimation();
            state = LupusState.ChaseFailure;
            isChaseHalt = true;
        }

        // 플레이어가 스킬 범위 내에 있는 경우
        if (CalcPlayerDistance() <= skill_Ripping.GetMonsterSkillData().MaxRange)
        {
            // 공격 상태로 전이
            lupusAi.StopMove();
            state = LupusState.Attack;
        }
    }

    // 추적 실패 상태 업데이트
    void UpdateChaseFailure()
    {
        lupusAi.MoveBeforeChasePosition();

        // 원래 자리로 이동 전까지 항상 풀피
        lupusData.CurrentHp = lupusData.Hp;

        if(lupusAi.ArriveBeforeChasePosition())
        {
            lupusAnimation.ChangeIdleAnimation();
            isChaseHalt = false;
            state = LupusState.Idle;
        }
    }

    // 공격 상태 업데이트
    void UpdateAttack()
    {
        lupus.LookAt(new Vector3(player.position.x, lupus.position.y, player.position.z));
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (skill_Ripping.UseSkillPossible())
        {
            lupusAnimation.ChangeAttackAnimation();
            skill_Ripping.UseSkill();
        }
        else
        {
            lupusAnimation.ChangeIdleAnimation();

            if (CalcPlayerDistance() > skill_Ripping.GetMonsterSkillData().MaxRange)
            {
                // 공격 거리에서 멀어졌을 경우

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                lupusAi.MoveChaseDestination(player.position);
                lupusAnimation.ChangeRunAnimation();
                state = LupusState.Chase;
            }
        }
    }

    // 피격 상태 업데이트
    void UpdateDamage()
    {
        StartCoroutine(WaitDamage());
    }

    // 피격 애니메이션이 다 끝난 이후 동작 처리
    IEnumerator WaitDamage()
    {
        if (!isWateDamage)
        {
            isWateDamage = true;
            
            yield return new WaitForSeconds(lupusAnimation.GetCurrentAnimationLength());

            if (lupusData.CurrentHp <= 0)
            {
                // 현재 체력이 0 이하일 경우 죽음 상태로 전환
                lupusAi.StopMove();
                lupusAnimation.ChangeDieAnimation();
                state = LupusState.Die;
            }
            else
            {
                // 아닌경우 피격 상태로 전환
                isDamage = false;
                state = prevDamageState;
            }

            isWateDamage = false;

            StopCoroutine("Wait");
        }
    }

    // 죽음 상태 업데이트
    void UpdateDie()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitDieAnimation());
    }

    // 죽음 애니매이션이 다 끝난 이후 죽음 처리
    IEnumerator WaitDieAnimation()
    {
        if (!isWaitDieAnimation)
        {
            isWaitDieAnimation = true;

            yield return new WaitForSeconds(lupusAnimation.GetCurrentAnimationLength());

            DropExpAndGold(); // 경험치 및 재화 드랍
            DropItems();      // 아이템 드랍

            // 진행 중인 퀘스트들 중 해당 몬스터가 조건인지 확인
            QuestManager.instance.CheckQuestMonster(this);

            if (ValleyForestMapManager.instance != null) ValleyForestMapManager.instance.LupusCurrentGenCount--;

            // 5초 뒤 몬스터 제거
            Destroy(gameObject, 5.0f);

            StopCoroutine("WaitAni");
        }
    }

    // 몬스터 몸에 닿았을 때의 데미지       
    private void OnCollisionEnter(Collision collision)
    {
  
    }

    // 플레이어 스킬로 입은 피해 처리 (플레이어 스킬에 맞았을 때)
    public override void DamagedPlayerSkill(float skillDamageValue)
    {
        if (state != LupusState.ChaseFailure)
        {
            // 플레이어 스킬 데미지만큼 몬스터의 체력 깎기
            lupusData.CurrentHp -= skillDamageValue;

            // 연달아 맞을 경우 한번만 저장되도록
            if(!isDamage)
            {
                prevDamageState = state; // 이전 상태 저장 (피격 후 이전 상태로 돌아감)                                      
                isDamage = true;
            }

            // 피격 상태로 전이
            lupusAnimation.ChangeDamageAnimation();
            
            state = LupusState.Damage;
        }
    }

    // 경험치 및 재화 드랍
    protected override void DropExpAndGold()
    {
        PlayerManager.instance.GainExp(lupusData.Exp);                  // 획득한 경험치 처리
        InventoryManager.instance.Gold.CurrentAmount += lupusData.Gold; // 획득한 재화(골드) 처리
    }

    // 아이템 드랍
    protected override void DropItems()
    {
        for (int i = 0; i < lupusData.DropItemPrefabs.Count; i++)
        {
            // 드랍 아이템이 존재하는 경우
            if(lupusData.DropItemPrefabs[i] != null)
            {
                // 0부터 100까지 중 랜덤하게 뽑기
                float probability = Random.Range(0.0f, 100.0f);

                //Debug.Log("probability: " + probability);

                // 뽑은 숫자가 아이템 드랍 확률 이하인 경우 (아이템이 드랍되는 경우)
                if (probability <= lupusData.ItemDropProbability[i])
                {
                    // 몬스터 위치에 드랍 아이템 생성
                    Instantiate(lupusData.DropItemPrefabs[i], transform.position + new Vector3(0, 1.0f, 0), transform.rotation);
                }
            }
        }
    }

    // 해당 몬스터와 플레이어 사이의 거리 계산
    private float CalcPlayerDistance()
    {
        Vector3 lupusPosition = lupus.position;
        Vector3 playerPosition = player.position;

        float distance = Vector3.Distance(lupusPosition, playerPosition);

        return distance;
    }

    // 몬스터 데이터 반환
    public override MonsterData GetMonsterData()
    {
        return lupusData;
    }
}
