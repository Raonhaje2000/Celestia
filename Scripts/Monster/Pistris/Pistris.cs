using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계곡 숲 보스 몬스터 - 피스트리스
public class Pistris : BossMonster
{
    public enum PistrisState
    {
        Idle = 0,     // 기본
        Chase,        // 추적
        Dash,         // 돌진 (플레이어가 멀리 떨어져있을 경우)
        Attack,       // 공격
        Hit,          // 피격
        Death         // 죽음
    }

    [SerializeField] PistrisState state;

    [SerializeField] BossMonsterData pistrisData; // 피스트리스 데이터 (체력 관련 데이터 포함)

    // 컴포넌트로 추가된 클래스들
    PistrisAnimation pistrisAnimation;            // 피스트리스 애니메이션 담당 클래스
    PistrisAi pistrisAi;                          // 피스트리스 AI 담당 클래스

    PistrisSkill_Bite skill_Bite;                 // 피스트리스 스킬(물어뜯기) 담당 클래스
    PistrisSkill_TailSwing skill_TailSwing;       // 피스트리스 스킬(꼬리 휘두르기) 담당 클래스
    PistrisSkill_JumpAttack skill_JumpAttack;     // 피스트리스 스킬(내려찍기) 담당 클래스

    Transform player;                             // 플레이어 (타겟)
    Transform pistris;                            // 피스트리스 (몬스터)

    float dashCriterionDistance;                  // 돌진 기준 거리 (해당 범위 미만은 일반 추적)
    float attackCriterionDistance;                // 공격 기준 거리 (스킬 물어뜯기, 꼬리 휘두르기 중 최소 거리)

    bool isDeath;                                 // 죽었을 때 한번만 처리되도록 하는 플래그
    bool isDamage;

    bool isBodyAttackDelay;
    bool isWaitHitAnimation;
    bool isWaitDeathAnimation;

    BoxCollider boxCollider;

    // 코루틴 관련 변수
    [SerializeField] bool isWaitDashDelay;
    [SerializeField] bool isChaseDelay;
    [SerializeField] bool isDashDelay;

    bool isWaitNextFrame;

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
        switch (state)
        {
            case PistrisState.Idle:
                {
                    UpdateIdle();
                    break;
                }
            case PistrisState.Chase:
                {
                    UpdateChase();
                    break;
                }
            case PistrisState.Dash:
                {
                    UpdateDash();
                    break;
                }
            case PistrisState.Attack:
                {
                    UpdateAttack();
                    break;
                }
            case PistrisState.Hit:
                {
                    UpdateHit();
                    break;
                }
            case PistrisState.Death:
                {
                    UpdateDeath();
                    break;
                }
        }
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        pistrisData = ScriptableObject.Instantiate(Resources.Load<BossMonsterData>("GameData/Monster/Field03_ValleyForest/PistrisData"));

        pistrisAnimation = GetComponent<PistrisAnimation>();
        pistrisAi = GetComponent<PistrisAi>();

        skill_Bite = GetComponent<PistrisSkill_Bite>();
        skill_TailSwing = GetComponent<PistrisSkill_TailSwing>();
        skill_JumpAttack = GetComponent<PistrisSkill_JumpAttack>();

        player = GameObject.FindWithTag("Player").transform;
        pistris = this.transform;

        boxCollider = GetComponent<BoxCollider>();
    }

    // 초기화
    void Initialize()
    {
        state = PistrisState.Idle;

        dashCriterionDistance = 50.0f;
        attackCriterionDistance = Mathf.Max(skill_Bite.GetMonsterSkillData().MaxRange, skill_TailSwing.GetMonsterSkillData().MaxRange);

        pistrisData.CurrentHp = pistrisData.Hp;

        isDeath = false;

        isChaseDelay = false;
        isDashDelay = false;

        // 몬스터 AI 속력 세팅
        pistrisAi.SetSpeeds(pistrisData.RunSpeed, pistrisData.RunSpeed * 4.0f);
    }

    // 기본 상태 업데이트
    void UpdateIdle()
    {
        if (CalcPlayerDistance() >= dashCriterionDistance)
        {
            // 돌진 가능 거리에 있는 경우
            StartCoroutine(WaitDash());
        }
        else if (CalcPlayerDistance() > attackCriterionDistance)
        {
            // 공격 범위 밖에 있는 경우
            StartCoroutine(WaitChase());
        }
        else
        {
            Debug.Log("Attack");
            state = PistrisState.Attack;
        }
    }

    IEnumerator WaitDash()
    {
        if(!isWaitDashDelay)
        {
            isWaitDashDelay = true;

            yield return new WaitForSeconds(1.0f);

            state = PistrisState.Dash;

            isWaitDashDelay = false;

            StopCoroutine("WaitDash");
        }
    }

    IEnumerator WaitChase()
    {
        if(!isChaseDelay)
        {
            isChaseDelay = true;

            float time = Random.Range(0, 3.0f);

            yield return new WaitForSeconds(time);

            Debug.Log("추적까지 대기한 시간: " + time);

            pistrisAnimation.ChangeRunAnimation();
            state = PistrisState.Chase;
            isChaseDelay = false;

            StopCoroutine("WaitChase");
        }
    }

    // 추적 상태 업데이트
    void UpdateChase()
    {
        if (CalcPlayerDistance() >= dashCriterionDistance)
        {
            pistrisAi.StopMove();
            state = PistrisState.Dash;
        }
        else if (CalcPlayerDistance() <= attackCriterionDistance)
        {
            Debug.Log("Attack");
            pistrisAi.StopMove();
            state = PistrisState.Attack;
        }
        else
        {
            pistrisAi.MoveChaseDestination();

            if (pistrisAi.ArriveDestination())
            {
                pistrisAi.StopMove();
                pistrisAnimation.ChangeIdleAnimation();
                state = PistrisState.Idle;
            }
        }
    }

    // 돌진 상태 업데이트
    void UpdateDash()
    {
        if (!isDashDelay)
        {
            // 플레이어를 바라보고 있다가 일정 시간 뒤에 플레이어를 향해 돌진
            StartCoroutine(WaitDashDelay());
        }

        // 돌진 도착지에 도착한 경우
        if (pistrisAi.ArriveDestination())
        { 
            // 박스 콜라이더 Trigger 해제 후 기본 상태로 전이
            boxCollider.isTrigger = false;
            pistrisAi.StopMove();
            pistrisAnimation.ChangeIdleAnimation();
            state = PistrisState.Idle;

            isDashDelay = false;

            StopCoroutine("WaitDashDelay");
        }
    }

    // 돌진 상태 딜레이 동안 대기
    // 플레이어를 바라보고 있다가 일정 시간 뒤에 플레이어를 향해 돌진
    IEnumerator WaitDashDelay()
    {
        isDashDelay = true;

        pistris.LookAt(new Vector3(player.position.x, pistris.position.y, player.position.z));
        pistrisAnimation.ChangeIdleAnimation();

        yield return new WaitForSeconds(1.0f);

        boxCollider.isTrigger = true;
        pistrisAi.MoveDashDestination();
        pistrisAnimation.ChangeDashAnimation();
    }

    // 공격 상태 업데이트
    void UpdateAttack()
    {        
        pistris.LookAt(new Vector3(player.position.x, pistris.position.y, player.position.z));
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (skill_Bite.UseSkillPossible())
        {
            pistrisAnimation.ChangeBiteAnimation();
            skill_Bite.UseSkill();
        }
        else
        {
            pistrisAnimation.ChangeIdleAnimation();

            if (CalcPlayerDistance() > skill_Bite.GetMonsterSkillData().MaxRange)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                pistrisAnimation.ChangeIdleAnimation();
                state = PistrisState.Idle;
            }
        }
    }

    // 피격 상태 업데이트
    void UpdateHit()
    {
        StartCoroutine(WaitAnimation());
    }

    // 피격 애니메이션이 다 끝난 이후 동작 처리
    IEnumerator WaitAnimation()
    {
        if(!isWaitHitAnimation)
        {
            isWaitHitAnimation = true;

            yield return new WaitForSeconds(pistrisAnimation.GetCurrentAnimationLength());

            if(pistrisData.CurrentHp <= 0)
            {
                // 현재 체력이 0 이하일 경우 죽음 상태로 전환
                pistrisAnimation.ChangeDeathAnimation();
                state = PistrisState.Death;
            }
            else
            {
                // 아닌경우 피격 상태로 전환
                pistrisAnimation.ChangeHitAnimation();
                state = PistrisState.Idle;
            }

            pistrisAi.StopMove();

            isWaitHitAnimation = false;

            StopCoroutine("WaitAnimation");
        }
    }

    // 죽음 상태 업데이트
    void UpdateDeath()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitDeathAnimation());
    }

    // 죽음 애니매이션이 다 끝난 이후 죽음 처리
    IEnumerator WaitDeathAnimation()
    {
        if (!isWaitDeathAnimation)
        {
            isWaitDeathAnimation = true;

            yield return new WaitForSeconds(pistrisAnimation.GetCurrentAnimationLength());

            DropExpAndGold(); // 경험치 및 재화 드랍
            DropItems();      // 아이템 드랍

            // 진행 중인 퀘스트들 중 해당 몬스터가 조건인지 확인
            QuestManager.instance.CheckQuestMonster(this);

            if (ValleyForestDungeonMapManager.instance != null) ValleyForestDungeonMapManager.instance.ClearDungeon();

            // 5초 뒤 몬스터 제거
            Destroy(gameObject, 5.0f);

            StopCoroutine("WaitDeathAnimation");
        }
    }

    // 플레이어 스킬로 입은 피해 처리 (플레이어 스킬에 맞았을 때)
    public override void DamagedPlayerSkill(float skillDamageValue)
    {
        // 플레이어 스킬 데미지만큼 몬스터의 체력 깎기
        pistrisData.CurrentHp -= skillDamageValue;

        // 연달아 맞을 경우 한번만 저장되도록
        if (!isDamage) isDamage = true;

        // 기본 상태나 추적 상태일 때만 피격 애니매이션 실행
        if (state == PistrisState.Idle || state == PistrisState.Chase)
        {
            pistrisAi.StopMove();
            pistrisAnimation.ChangeHitAnimation();
        }

        state = PistrisState.Hit;
    }

    // 몬스터 데이터 반환
    public override MonsterData GetMonsterData()
    {
        return pistrisData;
    }

    // 경험치 및 재화 드랍
    protected override void DropExpAndGold()
    {
        PlayerManager.instance.GainExp(pistrisData.Exp);                  // 획득한 경험치 처리
        InventoryManager.instance.Gold.CurrentAmount += pistrisData.Gold; // 획득한 재화(골드) 처리
    }

    // 아이템 드랍
    protected override void DropItems()
    {
        for(int i = 0; i < pistrisData.DropItemPrefabs.Count; i++)
        {
            // 드랍 아이템이 존재하는 경우
            if (pistrisData.DropItemPrefabs[i] != null)
            {
                // 0부터 100까지 중 랜덤하게 뽑기
                float probability = Random.Range(0.0f, 100.0f);

                //Debug.Log("probability: " + probability);

                // 뽑은 숫자가 아이템 드랍 확률 이하인 경우 (아이템이 드랍되는 경우)
                if (probability <= pistrisData.ItemDropProbability[i])
                {
                    // 몬스터 위치에 드랍 아이템 생성
                    Instantiate(pistrisData.DropItemPrefabs[i], transform.position + new Vector3(0, 1.0f, 0), transform.rotation);
                }
            }
        }
    }

    // 해당 몬스터와 플레이어 사이의 거리 계산
    private float CalcPlayerDistance()
    {
        Vector3 pistrisPosition = pistris.position;
        Vector3 playerPosition = player.position;

        float distance = Vector3.Distance(pistrisPosition, playerPosition);

        return distance;
    }

    // 몬스터 몸에 닿았을 때의 데미지  
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(WaitBodyAttackDelay());
        }
    }

    // 몬스터와 닿고 있을 때 0.3초마다 데미지가 들어감
    IEnumerator WaitBodyAttackDelay()
    {
        if(!isBodyAttackDelay)
        {
            isBodyAttackDelay = true;

            //Debug.Log("돌진 데미지");

            // 플레이어 데미지 처리

            yield return new WaitForSeconds(0.3f);

            isBodyAttackDelay = false;
        }
    }
}
