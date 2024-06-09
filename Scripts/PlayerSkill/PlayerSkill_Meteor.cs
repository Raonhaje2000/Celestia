using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스킬 4 - 메테오
public class PlayerSkill_Meteor : PlayerSkill
{
    PlayerManager playerManager;             // PlayerManager

    [SerializeField]
    PlayerSkillData skillDataOrigin;         // 스킬의 데이터 원본 (해당 데이터로 스킬의 현재 데이터를 찾아옴)                
    PlayerSkillData skillData;               // 스킬 데이터 (현재 스킬 레벨에 따른 데이터)

    [Min(10)]
    [SerializeField] float meteorFallHeight; // 운석이 떨어지는 높이
    float meteorFallAngle;                   // 운석이 떨어지는 각도

    Vector3 meteorFallStartPosition;         // 운석이 떨어지기 시작하는 위치

    [Min(1)]
    [SerializeField] int meteorMaxCount;     // 운석의 최대 개수
    [SerializeField] int meteorCurrentCount; // 운석의 현재 개수 (생성된 개수)

    float generateDelayTime;                 // 운석 생성 대기 시간
    bool isGenerateDelay;                    // 운석 생성 대기 중인지 확인하는 플래그

    private void Awake()
    {
        // 관련 리소스 불러오기
        //LoadResources();
    }

    void Start()
    {
        // 관련 리소스 불러오기
        LoadResources();

        // 초기화
        Initialize();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //    UseSkill();
        // }
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        playerManager = PlayerManager.instance;

        skillDataOrigin = ScriptableObject.Instantiate(Resources.Load<PlayerSkillData>("GameData/Skill/Player/MeteorData"));
        skillData = playerManager.FindSkillData(skillDataOrigin); // 스킬의 현재 데이터를 찾아옴
    }

    // 초기화
    void Initialize()
    {
        meteorFallHeight = 25.0f;
        meteorFallAngle = 30;

        meteorFallStartPosition = Vector3.zero;

        meteorMaxCount = 20;
        meteorCurrentCount = 0;

        generateDelayTime = 0.1f;
        isGenerateDelay = false;
    }

    // 운석 생성
    void GenerateMeteor()
    {
        GameObject meteor = Instantiate(skillData.SkillEffectPrefab, transform.position, Quaternion.identity);           

        // 스킬 레벨에 맞춰 데미지(스킬에 피격되었을 때 몬스터가 입는 데미지) 세팅
        // 스킬 최종 데미지 = 해당 스킬 데미지 + 플레이어의 마법 공격력 수치
        meteor.GetComponent<Meteor>().SetDamage(skillData.Damage + playerManager.PlayerStatus.MagicAttack);

        // 운석 위치를 랜덤 위치로 변경
        meteor.transform.position = CalcRandomPosition();
    }

    // 운석의 랜덤 위치 (운석이 떨어지는 시작 위치) 계산
    Vector3 CalcRandomPosition()
    {
        // tan(각도) = 높이 / 밑변임을 이용 (여기서 유니티 회전 각도에 의해 높이는 밑변이 됨)
        float xz = (meteorFallHeight + transform.position.y) * Mathf.Tan(meteorFallAngle * Mathf.Deg2Rad);

        meteorFallStartPosition = new Vector3(xz, meteorFallHeight, xz);

        // 스킬 범위 내에서 랜덤 위치에 생성
        float radius = skillData.MaxRange;

        float randomX = Random.Range(-radius, radius);
        float randomZ = Random.Range(-radius, radius);

        // 플레이어 위치 기준 운석이 랜덤하게 떨어지는 위치
        Vector3 position = transform.position + meteorFallStartPosition + new Vector3(randomX, 0, randomZ);                      

        return position;
    }

    // 운석의 최대 개수만큼 운석 생성 (생성 후 일정 시간 딜레이 후 다시 생성)
    IEnumerator GenerateMeteorsToMaxCount()
    {
        if (!isGenerateDelay)
        {
            isGenerateDelay = true;

            // 운석의 현재 생성 수가 최대 생성 수보다 작은 경우에만 운석 생성
            while (meteorCurrentCount < meteorMaxCount)
            {
                GenerateMeteor(); // 운석 생성
                meteorCurrentCount++;

                yield return new WaitForSeconds(generateDelayTime);                                                            
            }

            // 운셕의 현재 생성 수 초기화
            meteorCurrentCount = 0;

            isGenerateDelay = false;
        }
    }

    // 플레이어가 스킬을 사용했을 때의 처리
    public override void UseSkill()
    {
        // 스킬의 쿨타임이 아닌 경우
        if (!skillData.IsCooldown)
        {
            // MP 소모 후 스킬 시전
            playerManager.CurrentMp -= skillData.MpConsumption;                                                          
            StartCoroutine(GenerateMeteorsToMaxCount());
        }
    }
}
