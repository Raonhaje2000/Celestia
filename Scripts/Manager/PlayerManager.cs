using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 레벨, 능력치, 스킬 관련 처리
public class PlayerManager : MonoBehaviour
{
    public const int PLAYER_MAX_LEVEL = 20;          // 플레이어 레벨 최댓값

    public static PlayerManager instance;

    [Header("[Player Level]")]
    [Min(1)]
    [SerializeField] int level = 1;                   // 레벨

    [SerializeField] float maxExp;                    // 최대 경험치 (현재 레벨 기준)
    [SerializeField] float currentExp;                // 경험치

    [Header("[Player Status]")]
    [SerializeField] StatusData playerStatus;         // 플레이어 능력치                                                     

    int maxStatPoint;                                 // 보유 가능한 스탯 포인트 최대값
    [SerializeField] int currentStatPoint = 1;        // 현재 보유한 스탯 포인트

    [SerializeField] float currentHp;                 // 현재 HP 
    [SerializeField] float currentMp;                 // 현재 MP

    bool natureRecoveryMpDelay;                       // Mp 자연 회복 딜레이인지 체크하는 코루틴 플래그                              

    [Header("[Player Skill]")]
    [SerializeField] PlayerSkillData[] playerSkills;  // 플레이어 스킬들

    int maxSkillPoint;                                // 보유 가능한 플레이어 스킬 포인트 최대값
    [SerializeField] int currentSkillPoint = 4;       // 현재 보유한 플레이어 스킬 포인트

    #region [Property]

    public int Level
    {
        get { return level; }
    }

    public float MaxExp
    {
        get { return maxExp; }
    }

    public float CurrentExp
    {
        get { return currentExp; }
    }

    public StatusData PlayerStatus
    {
        get { return playerStatus; }
        set { playerStatus = value; }
    }

    public int MaxStatPoint
    {
        get { return maxStatPoint; }
    }

    public int CurrentStatPoint
    {
        get { return currentStatPoint; }
        set { currentStatPoint = value; }
    }

    public float MaxHp
    {
        get { return playerStatus.Hp; }
    }

    public float CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;

            if (currentHp > MaxHp) currentHp = MaxHp;
        }
    }

    public float MaxMp
    {
        get { return playerStatus.Mp; }
    }

    public float CurrentMp
    {
        get { return currentMp; }
        set
        {
            currentMp = value;

            if (currentMp > MaxMp) currentMp = MaxMp;
        }
    }

    public PlayerSkillData[] PlayerSkills
    {
        get { return playerSkills; }
    }

    public int MaxSkillPoint
    {
        get { return maxSkillPoint; }
    }

    public int CurrentSkillPoint
    {
        get { return currentSkillPoint; }
        set { currentSkillPoint = value; }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 다음 씬으로 넘어가도 오브젝트 파괴되지 않고 유지
            // 게임 내에서 공통으로 쓰이는 데이터 유지
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // 이미 기존 씬에 해당 오브젝트가 남아있을 경우 제거
            Destroy(gameObject);
        }

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
        // 현재 MP가 최대 MP보다 적은 경우 MP 자연 회복
        if (currentMp < MaxMp) StartCoroutine(NatureRecoverMp());
        else StopCoroutine("NatureRecoverMp");
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        playerStatus = ScriptableObject.Instantiate(Resources.Load<StatusData>("GameData/Status/PlayerStatus"));                                 

        PlayerSkillData[] skillDataArray = Resources.LoadAll<PlayerSkillData>("GameData/Skill/Player");

        playerSkills = InstantiateSortedSkillArrayById(skillDataArray); // 정렬된 스킬 배열 생성 (오름차순)
    }

    // 초기화
    void Initialize()
    {
        level = 1;

        maxExp = CalcMaxExp();
        currentExp = 0;

        maxStatPoint = PLAYER_MAX_LEVEL;
        currentStatPoint = level;

        currentHp = MaxHp;
        currentMp = MaxMp;

        natureRecoveryMpDelay = false;

        maxSkillPoint = playerSkills.Length * PlayerSkillData.SKILL_MAX_LEVEL;
        currentSkillPoint = playerSkills.Length;
    }

    // 정렬된 스킬 배열 생성 (ID 오름차순)
    PlayerSkillData[] InstantiateSortedSkillArrayById(PlayerSkillData[] skillDataArray)
    {
        PlayerSkillData[] sortedSkillArray = new PlayerSkillData[skillDataArray.Length];

        sortedSkillArray[0] = ScriptableObject.Instantiate(skillDataArray[0]);

        // ID를 기준으로 오름차순 정렬
        for (int i = 1; i < skillDataArray.Length; i++)
        {
            sortedSkillArray[i] = ScriptableObject.Instantiate(skillDataArray[i]);

            for (int j = 0; j < i; j++)
            {
                // 현재 스킬 ID보다 기존에 있던 스킬 ID가 큰 경우 자리 교체
                if (sortedSkillArray[i].SkillId < sortedSkillArray[j].SkillId) SwapSkillData(ref sortedSkillArray[i], ref sortedSkillArray[j]);
            }
        }

        return sortedSkillArray;
    }

    // 스킬 자리 교체 (Swap)
    void SwapSkillData(ref PlayerSkillData a, ref PlayerSkillData b)                                                                            
    {
        PlayerSkillData temp = a;
        a = b;
        b = temp;
    }

    // 플레이어의 최대 경험치 계산
    float CalcMaxExp()
    {
        // 최대 경험치 수식: 100 + (레벨-1)^2 + 100 * (레벨-1)
        return 100.0f + Mathf.Pow((level - 1), 2) + 100.0f * (level - 1);
    }

    // 경험치 획득 처리
    public void GainExp(float expValue)
    {
        if(expValue >= 0)
        {
            // 경험치를 얻었을 때

            currentExp += expValue;

            // 경험치가 최대 경험치를 넘었을 경우
            // 레벨업 처리
            if (currentExp >= MaxExp) LevelUp();
        }
        else
        {
            // 경험치를 잃었을 때

            currentExp -= expValue;

            // 경험치가 0 미만이 된 경우
            // 0으로 변경 (레벨 유지)
            if (currentExp < 0.0f) currentExp = 0.0f;
        }
    }

    // 레벨업 처리
    void LevelUp()
    {
        level++;

        // 남은 경험치를 다음 레벨 경험치로 이전 시키고, 변경된 레벨에 따라 최대 경험치 업데이트
        currentExp -= maxExp;
        maxExp = CalcMaxExp();

        InGame_Manager.instance.LevelTextUpdate();
    }

    // Mp 자연 회복
    IEnumerator NatureRecoverMp()
    {
        if (!natureRecoveryMpDelay)
        {
            natureRecoveryMpDelay = true;

            currentMp += playerStatus.NatureRecoveryMpPerSec;
            if (currentMp > MaxMp) currentMp = MaxMp;

            yield return new WaitForSecondsRealtime(1.0f);

            natureRecoveryMpDelay = false;
        }
    }

    // 스킬 ID로 스킬 데이터 찾아서 반환
    public PlayerSkillData FindSkillData(PlayerSkillData skillData)
    {
        for(int i = 0; i < playerSkills.Length; i++)
        {
            if (playerSkills[i].SkillId == skillData.SkillId) return playerSkills[i];                                        
        }

        return null;
    }
}
