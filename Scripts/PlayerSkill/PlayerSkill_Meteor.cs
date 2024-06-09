using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ��ų 4 - ���׿�
public class PlayerSkill_Meteor : PlayerSkill
{
    PlayerManager playerManager;             // PlayerManager

    [SerializeField]
    PlayerSkillData skillDataOrigin;         // ��ų�� ������ ���� (�ش� �����ͷ� ��ų�� ���� �����͸� ã�ƿ�)                
    PlayerSkillData skillData;               // ��ų ������ (���� ��ų ������ ���� ������)

    [Min(10)]
    [SerializeField] float meteorFallHeight; // ��� �������� ����
    float meteorFallAngle;                   // ��� �������� ����

    Vector3 meteorFallStartPosition;         // ��� �������� �����ϴ� ��ġ

    [Min(1)]
    [SerializeField] int meteorMaxCount;     // ��� �ִ� ����
    [SerializeField] int meteorCurrentCount; // ��� ���� ���� (������ ����)

    float generateDelayTime;                 // � ���� ��� �ð�
    bool isGenerateDelay;                    // � ���� ��� ������ Ȯ���ϴ� �÷���

    private void Awake()
    {
        // ���� ���ҽ� �ҷ�����
        //LoadResources();
    }

    void Start()
    {
        // ���� ���ҽ� �ҷ�����
        LoadResources();

        // �ʱ�ȭ
        Initialize();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //    UseSkill();
        // }
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        playerManager = PlayerManager.instance;

        skillDataOrigin = ScriptableObject.Instantiate(Resources.Load<PlayerSkillData>("GameData/Skill/Player/MeteorData"));
        skillData = playerManager.FindSkillData(skillDataOrigin); // ��ų�� ���� �����͸� ã�ƿ�
    }

    // �ʱ�ȭ
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

    // � ����
    void GenerateMeteor()
    {
        GameObject meteor = Instantiate(skillData.SkillEffectPrefab, transform.position, Quaternion.identity);           

        // ��ų ������ ���� ������(��ų�� �ǰݵǾ��� �� ���Ͱ� �Դ� ������) ����
        // ��ų ���� ������ = �ش� ��ų ������ + �÷��̾��� ���� ���ݷ� ��ġ
        meteor.GetComponent<Meteor>().SetDamage(skillData.Damage + playerManager.PlayerStatus.MagicAttack);

        // � ��ġ�� ���� ��ġ�� ����
        meteor.transform.position = CalcRandomPosition();
    }

    // ��� ���� ��ġ (��� �������� ���� ��ġ) ���
    Vector3 CalcRandomPosition()
    {
        // tan(����) = ���� / �غ����� �̿� (���⼭ ����Ƽ ȸ�� ������ ���� ���̴� �غ��� ��)
        float xz = (meteorFallHeight + transform.position.y) * Mathf.Tan(meteorFallAngle * Mathf.Deg2Rad);

        meteorFallStartPosition = new Vector3(xz, meteorFallHeight, xz);

        // ��ų ���� ������ ���� ��ġ�� ����
        float radius = skillData.MaxRange;

        float randomX = Random.Range(-radius, radius);
        float randomZ = Random.Range(-radius, radius);

        // �÷��̾� ��ġ ���� ��� �����ϰ� �������� ��ġ
        Vector3 position = transform.position + meteorFallStartPosition + new Vector3(randomX, 0, randomZ);                      

        return position;
    }

    // ��� �ִ� ������ŭ � ���� (���� �� ���� �ð� ������ �� �ٽ� ����)
    IEnumerator GenerateMeteorsToMaxCount()
    {
        if (!isGenerateDelay)
        {
            isGenerateDelay = true;

            // ��� ���� ���� ���� �ִ� ���� ������ ���� ��쿡�� � ����
            while (meteorCurrentCount < meteorMaxCount)
            {
                GenerateMeteor(); // � ����
                meteorCurrentCount++;

                yield return new WaitForSeconds(generateDelayTime);                                                            
            }

            // ����� ���� ���� �� �ʱ�ȭ
            meteorCurrentCount = 0;

            isGenerateDelay = false;
        }
    }

    // �÷��̾ ��ų�� ������� ���� ó��
    public override void UseSkill()
    {
        // ��ų�� ��Ÿ���� �ƴ� ���
        if (!skillData.IsCooldown)
        {
            // MP �Ҹ� �� ��ų ����
            playerManager.CurrentMp -= skillData.MpConsumption;                                                          
            StartCoroutine(GenerateMeteorsToMaxCount());
        }
    }
}
