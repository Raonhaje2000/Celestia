using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ��� �� �븻 ���� 01 - ��Ǫ��
public class Lupus : NormalMonster
{
    public enum LupusState
    {
        Idle = 0,     // �⺻
        Walk,         // �ȱ�
        Chase,        // ����
        ChaseFailure, // ���� ����
        Attack,       // ����
        Damage,       // �ǰ�
        Die           // ����                                                   
    }

    [SerializeField] LupusState state;            // ��Ǫ�� ����
    [SerializeField] LupusState prevDamageState;  // �ǰ� �ޱ� ���� ����

    [SerializeField] NormalMonsterData lupusData; // ��Ǫ�� ������ (ü�� ���� ������ ����)

    // ������Ʈ�� �߰��� Ŭ������
    LupusAnimation lupusAnimation;            // ��Ǫ�� �ִϸ��̼� ��� Ŭ����
    LupusAi lupusAi;                          // ��Ǫ�� AI ��� Ŭ����

    LupusSkill_Ripping skill_Ripping;         // ��Ǫ�� ��ų(���Ⱕ�� ����) ��� Ŭ����

    Transform player;                         // �÷��̾� (Ÿ��)
    Transform lupus;                          // ��Ǫ�� (����)

    [SerializeField] bool isPreemptiveAttack; // ���� ����

    [SerializeField]
    Vector3 walkDestination;                  // �ȱ� ������ ���� ������

    float walkDistanceMax;                    // �ȱ�� �̵��ϴ� �ִ� �Ÿ�

    [SerializeField]
    bool isWalkHalt;                          // �ȱⰡ �ߴܵ� (�ȱ� ���� ���� ���·� ���̵�)
    [SerializeField]
    bool isChaseHalt;

    [SerializeField]
    float currentHp;

    bool isWaitWalkRandomPosition;
    bool isWaitMove;
    bool isWateDamage;
    bool isWaitDieAnimation;

    bool isDie;   // �׾��� �� �ѹ��� ó���ǵ��� �ϴ� �÷���
    bool isDamage;

    private void Awake()
    {
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
        isPreemptiveAttack = (PlayerManager.instance.Level <= lupusData.PreemptiveAttackMaxLevel) ? true : false;
        currentHp = lupusData.CurrentHp;

        switch (state)
        {
            case LupusState.Idle:
                {
                    UpdateIdle();           // �⺻ ���� ������Ʈ
                    break;
                }
            case LupusState.Walk:
                {
                    UpdateWalk();           // �ȱ� ���� ������Ʈ
                    break;
                }
            case LupusState.Chase:
                {
                    UpdateChase();          // ���� ���� ������Ʈ
                    break;
                }
            case LupusState.ChaseFailure:
                {
                    UpdateChaseFailure();   // ���� ���� ���� ������Ʈ
                    break;
                }
            case LupusState.Attack:
                {
                    UpdateAttack();         // ���� ���� ������Ʈ
                    break;
                }
            case LupusState.Damage:
                {
                    UpdateDamage();         // �ǰ� ���� ������Ʈ
                    break;
                }
            case LupusState.Die:
                {
                    UpdateDie();            // ���� ���� ������Ʈ
                    break;
                }
        }
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        lupusData = ScriptableObject.Instantiate(Resources.Load<NormalMonsterData>("GameData/Monster/Field03_ValleyForest/LupusData"));

        lupusAnimation = GetComponent<LupusAnimation>();
        lupusAi = GetComponent<LupusAi>();
        skill_Ripping = GetComponent<LupusSkill_Ripping>();

        player = GameObject.FindWithTag("Player").transform;
        lupus = this.transform;
    }

    // �ʱ�ȭ
    void Initialize()
    {
        state = LupusState.Idle;

        isPreemptiveAttack = true; // �⺻������ ����

        walkDestination = lupus.position;

        walkDistanceMax = 5.0f;

        isWalkHalt = false;

        lupusData.CurrentHp = lupusData.Hp; // ������ ���� ü���� ���� ü�� �ִ밪���� �ʱ�ȭ

        // ���� AI �ӷ� ����
        lupusAi.SetSpeeds(lupusData.MoveSpeed, lupusData.RunSpeed, lupusData.ReturnSpeed);
    }

    // �⺻ ���� ������Ʈ
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
                    // �������� ���
                    // �Ǵ� �İ����ε� �ǰ� ���� ü���� ���� ���
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

    // ������ ��ġ�� �ֺ� ��ȸ�ϱ�
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

    // �ȱ� ���� ������Ʈ
    void UpdateWalk()
    {
        if (CalcPlayerDistance() <= lupusData.SensingMaxRange)
        {
            if (isPreemptiveAttack || (!isPreemptiveAttack && lupusData.CurrentHp < lupusData.Hp))
            {
                // �������� ���
                // �Ǵ� �İ����ε� �ǰ� ���� ü���� ���� ���
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

    // �̵��ϴ� ���� ���߰� ��� ��� �� ���� ��ġ�� �ǵ��ư���
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

    // ���� ���� ������Ʈ
    void UpdateChase()
    {
        lupusAi.MoveChaseDestination(player.position);

        // �÷��̾ ���� ���� ������ ��� ���
        if (CalcPlayerDistance() > lupusData.SensingMaxRange)
        {
            // ���� ���� ��ġ�� �ǵ��� ���� ���� ���� ���·� ����
            lupusAi.MoveBeforeChasePosition();
            lupusAnimation.ChangeRunAnimation();
            state = LupusState.ChaseFailure;
            isChaseHalt = true;
        }

        // �÷��̾ ��ų ���� ���� �ִ� ���
        if (CalcPlayerDistance() <= skill_Ripping.GetMonsterSkillData().MaxRange)
        {
            // ���� ���·� ����
            lupusAi.StopMove();
            state = LupusState.Attack;
        }
    }

    // ���� ���� ���� ������Ʈ
    void UpdateChaseFailure()
    {
        lupusAi.MoveBeforeChasePosition();

        // ���� �ڸ��� �̵� ������ �׻� Ǯ��
        lupusData.CurrentHp = lupusData.Hp;

        if(lupusAi.ArriveBeforeChasePosition())
        {
            lupusAnimation.ChangeIdleAnimation();
            isChaseHalt = false;
            state = LupusState.Idle;
        }
    }

    // ���� ���� ������Ʈ
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
                // ���� �Ÿ����� �־����� ���

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                lupusAi.MoveChaseDestination(player.position);
                lupusAnimation.ChangeRunAnimation();
                state = LupusState.Chase;
            }
        }
    }

    // �ǰ� ���� ������Ʈ
    void UpdateDamage()
    {
        StartCoroutine(WaitDamage());
    }

    // �ǰ� �ִϸ��̼��� �� ���� ���� ���� ó��
    IEnumerator WaitDamage()
    {
        if (!isWateDamage)
        {
            isWateDamage = true;
            
            yield return new WaitForSeconds(lupusAnimation.GetCurrentAnimationLength());

            if (lupusData.CurrentHp <= 0)
            {
                // ���� ü���� 0 ������ ��� ���� ���·� ��ȯ
                lupusAi.StopMove();
                lupusAnimation.ChangeDieAnimation();
                state = LupusState.Die;
            }
            else
            {
                // �ƴѰ�� �ǰ� ���·� ��ȯ
                isDamage = false;
                state = prevDamageState;
            }

            isWateDamage = false;

            StopCoroutine("Wait");
        }
    }

    // ���� ���� ������Ʈ
    void UpdateDie()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitDieAnimation());
    }

    // ���� �ִϸ��̼��� �� ���� ���� ���� ó��
    IEnumerator WaitDieAnimation()
    {
        if (!isWaitDieAnimation)
        {
            isWaitDieAnimation = true;

            yield return new WaitForSeconds(lupusAnimation.GetCurrentAnimationLength());

            DropExpAndGold(); // ����ġ �� ��ȭ ���
            DropItems();      // ������ ���

            // ���� ���� ����Ʈ�� �� �ش� ���Ͱ� �������� Ȯ��
            QuestManager.instance.CheckQuestMonster(this);

            if (ValleyForestMapManager.instance != null) ValleyForestMapManager.instance.LupusCurrentGenCount--;

            // 5�� �� ���� ����
            Destroy(gameObject, 5.0f);

            StopCoroutine("WaitAni");
        }
    }

    // ���� ���� ����� ���� ������       
    private void OnCollisionEnter(Collision collision)
    {
  
    }

    // �÷��̾� ��ų�� ���� ���� ó�� (�÷��̾� ��ų�� �¾��� ��)
    public override void DamagedPlayerSkill(float skillDamageValue)
    {
        if (state != LupusState.ChaseFailure)
        {
            // �÷��̾� ��ų ��������ŭ ������ ü�� ���
            lupusData.CurrentHp -= skillDamageValue;

            // ���޾� ���� ��� �ѹ��� ����ǵ���
            if(!isDamage)
            {
                prevDamageState = state; // ���� ���� ���� (�ǰ� �� ���� ���·� ���ư�)                                      
                isDamage = true;
            }

            // �ǰ� ���·� ����
            lupusAnimation.ChangeDamageAnimation();
            
            state = LupusState.Damage;
        }
    }

    // ����ġ �� ��ȭ ���
    protected override void DropExpAndGold()
    {
        PlayerManager.instance.GainExp(lupusData.Exp);                  // ȹ���� ����ġ ó��
        InventoryManager.instance.Gold.CurrentAmount += lupusData.Gold; // ȹ���� ��ȭ(���) ó��
    }

    // ������ ���
    protected override void DropItems()
    {
        for (int i = 0; i < lupusData.DropItemPrefabs.Count; i++)
        {
            // ��� �������� �����ϴ� ���
            if(lupusData.DropItemPrefabs[i] != null)
            {
                // 0���� 100���� �� �����ϰ� �̱�
                float probability = Random.Range(0.0f, 100.0f);

                //Debug.Log("probability: " + probability);

                // ���� ���ڰ� ������ ��� Ȯ�� ������ ��� (�������� ����Ǵ� ���)
                if (probability <= lupusData.ItemDropProbability[i])
                {
                    // ���� ��ġ�� ��� ������ ����
                    Instantiate(lupusData.DropItemPrefabs[i], transform.position + new Vector3(0, 1.0f, 0), transform.rotation);
                }
            }
        }
    }

    // �ش� ���Ϳ� �÷��̾� ������ �Ÿ� ���
    private float CalcPlayerDistance()
    {
        Vector3 lupusPosition = lupus.position;
        Vector3 playerPosition = player.position;

        float distance = Vector3.Distance(lupusPosition, playerPosition);

        return distance;
    }

    // ���� ������ ��ȯ
    public override MonsterData GetMonsterData()
    {
        return lupusData;
    }
}
