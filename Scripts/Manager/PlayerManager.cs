using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ����, �ɷ�ġ, ��ų ���� ó��
public class PlayerManager : MonoBehaviour
{
    public const int PLAYER_MAX_LEVEL = 20;          // �÷��̾� ���� �ִ�

    public static PlayerManager instance;

    [Header("[Player Level]")]
    [Min(1)]
    [SerializeField] int level = 1;                   // ����

    [SerializeField] float maxExp;                    // �ִ� ����ġ (���� ���� ����)
    [SerializeField] float currentExp;                // ����ġ

    [Header("[Player Status]")]
    [SerializeField] StatusData playerStatus;         // �÷��̾� �ɷ�ġ                                                     

    int maxStatPoint;                                 // ���� ������ ���� ����Ʈ �ִ밪
    [SerializeField] int currentStatPoint = 1;        // ���� ������ ���� ����Ʈ

    [SerializeField] float currentHp;                 // ���� HP 
    [SerializeField] float currentMp;                 // ���� MP

    bool natureRecoveryMpDelay;                       // Mp �ڿ� ȸ�� ���������� üũ�ϴ� �ڷ�ƾ �÷���                              

    [Header("[Player Skill]")]
    [SerializeField] PlayerSkillData[] playerSkills;  // �÷��̾� ��ų��

    int maxSkillPoint;                                // ���� ������ �÷��̾� ��ų ����Ʈ �ִ밪
    [SerializeField] int currentSkillPoint = 4;       // ���� ������ �÷��̾� ��ų ����Ʈ

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

            // ���� ������ �Ѿ�� ������Ʈ �ı����� �ʰ� ����
            // ���� ������ �������� ���̴� ������ ����
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // �̹� ���� ���� �ش� ������Ʈ�� �������� ��� ����
            Destroy(gameObject);
        }

        // ���� ���ҽ� �ҷ�����
        LoadResources();
    }

    void Start()
    {
        // �ʱ�ȭ
        Initialize();
    }

    void Update()
    {
        // ���� MP�� �ִ� MP���� ���� ��� MP �ڿ� ȸ��
        if (currentMp < MaxMp) StartCoroutine(NatureRecoverMp());
        else StopCoroutine("NatureRecoverMp");
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        playerStatus = ScriptableObject.Instantiate(Resources.Load<StatusData>("GameData/Status/PlayerStatus"));                                 

        PlayerSkillData[] skillDataArray = Resources.LoadAll<PlayerSkillData>("GameData/Skill/Player");

        playerSkills = InstantiateSortedSkillArrayById(skillDataArray); // ���ĵ� ��ų �迭 ���� (��������)
    }

    // �ʱ�ȭ
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

    // ���ĵ� ��ų �迭 ���� (ID ��������)
    PlayerSkillData[] InstantiateSortedSkillArrayById(PlayerSkillData[] skillDataArray)
    {
        PlayerSkillData[] sortedSkillArray = new PlayerSkillData[skillDataArray.Length];

        sortedSkillArray[0] = ScriptableObject.Instantiate(skillDataArray[0]);

        // ID�� �������� �������� ����
        for (int i = 1; i < skillDataArray.Length; i++)
        {
            sortedSkillArray[i] = ScriptableObject.Instantiate(skillDataArray[i]);

            for (int j = 0; j < i; j++)
            {
                // ���� ��ų ID���� ������ �ִ� ��ų ID�� ū ��� �ڸ� ��ü
                if (sortedSkillArray[i].SkillId < sortedSkillArray[j].SkillId) SwapSkillData(ref sortedSkillArray[i], ref sortedSkillArray[j]);
            }
        }

        return sortedSkillArray;
    }

    // ��ų �ڸ� ��ü (Swap)
    void SwapSkillData(ref PlayerSkillData a, ref PlayerSkillData b)                                                                            
    {
        PlayerSkillData temp = a;
        a = b;
        b = temp;
    }

    // �÷��̾��� �ִ� ����ġ ���
    float CalcMaxExp()
    {
        // �ִ� ����ġ ����: 100 + (����-1)^2 + 100 * (����-1)
        return 100.0f + Mathf.Pow((level - 1), 2) + 100.0f * (level - 1);
    }

    // ����ġ ȹ�� ó��
    public void GainExp(float expValue)
    {
        if(expValue >= 0)
        {
            // ����ġ�� ����� ��

            currentExp += expValue;

            // ����ġ�� �ִ� ����ġ�� �Ѿ��� ���
            // ������ ó��
            if (currentExp >= MaxExp) LevelUp();
        }
        else
        {
            // ����ġ�� �Ҿ��� ��

            currentExp -= expValue;

            // ����ġ�� 0 �̸��� �� ���
            // 0���� ���� (���� ����)
            if (currentExp < 0.0f) currentExp = 0.0f;
        }
    }

    // ������ ó��
    void LevelUp()
    {
        level++;

        // ���� ����ġ�� ���� ���� ����ġ�� ���� ��Ű��, ����� ������ ���� �ִ� ����ġ ������Ʈ
        currentExp -= maxExp;
        maxExp = CalcMaxExp();

        InGame_Manager.instance.LevelTextUpdate();
    }

    // Mp �ڿ� ȸ��
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

    // ��ų ID�� ��ų ������ ã�Ƽ� ��ȯ
    public PlayerSkillData FindSkillData(PlayerSkillData skillData)
    {
        for(int i = 0; i < playerSkills.Length; i++)
        {
            if (playerSkills[i].SkillId == skillData.SkillId) return playerSkills[i];                                        
        }

        return null;
    }
}
