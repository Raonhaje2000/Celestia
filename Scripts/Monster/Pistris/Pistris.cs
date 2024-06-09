using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �� ���� ���� - �ǽ�Ʈ����
public class Pistris : BossMonster
{
    public enum PistrisState
    {
        Idle = 0,     // �⺻
        Chase,        // ����
        Dash,         // ���� (�÷��̾ �ָ� ���������� ���)
        Attack,       // ����
        Hit,          // �ǰ�
        Death         // ����
    }

    [SerializeField] PistrisState state;

    [SerializeField] BossMonsterData pistrisData; // �ǽ�Ʈ���� ������ (ü�� ���� ������ ����)

    // ������Ʈ�� �߰��� Ŭ������
    PistrisAnimation pistrisAnimation;            // �ǽ�Ʈ���� �ִϸ��̼� ��� Ŭ����
    PistrisAi pistrisAi;                          // �ǽ�Ʈ���� AI ��� Ŭ����

    PistrisSkill_Bite skill_Bite;                 // �ǽ�Ʈ���� ��ų(������) ��� Ŭ����
    PistrisSkill_TailSwing skill_TailSwing;       // �ǽ�Ʈ���� ��ų(���� �ֵθ���) ��� Ŭ����
    PistrisSkill_JumpAttack skill_JumpAttack;     // �ǽ�Ʈ���� ��ų(�������) ��� Ŭ����

    Transform player;                             // �÷��̾� (Ÿ��)
    Transform pistris;                            // �ǽ�Ʈ���� (����)

    float dashCriterionDistance;                  // ���� ���� �Ÿ� (�ش� ���� �̸��� �Ϲ� ����)
    float attackCriterionDistance;                // ���� ���� �Ÿ� (��ų ������, ���� �ֵθ��� �� �ּ� �Ÿ�)

    bool isDeath;                                 // �׾��� �� �ѹ��� ó���ǵ��� �ϴ� �÷���
    bool isDamage;

    bool isBodyAttackDelay;
    bool isWaitHitAnimation;
    bool isWaitDeathAnimation;

    BoxCollider boxCollider;

    // �ڷ�ƾ ���� ����
    [SerializeField] bool isWaitDashDelay;
    [SerializeField] bool isChaseDelay;
    [SerializeField] bool isDashDelay;

    bool isWaitNextFrame;

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

    // ���� ���ҽ� �ҷ�����
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

    // �ʱ�ȭ
    void Initialize()
    {
        state = PistrisState.Idle;

        dashCriterionDistance = 50.0f;
        attackCriterionDistance = Mathf.Max(skill_Bite.GetMonsterSkillData().MaxRange, skill_TailSwing.GetMonsterSkillData().MaxRange);

        pistrisData.CurrentHp = pistrisData.Hp;

        isDeath = false;

        isChaseDelay = false;
        isDashDelay = false;

        // ���� AI �ӷ� ����
        pistrisAi.SetSpeeds(pistrisData.RunSpeed, pistrisData.RunSpeed * 4.0f);
    }

    // �⺻ ���� ������Ʈ
    void UpdateIdle()
    {
        if (CalcPlayerDistance() >= dashCriterionDistance)
        {
            // ���� ���� �Ÿ��� �ִ� ���
            StartCoroutine(WaitDash());
        }
        else if (CalcPlayerDistance() > attackCriterionDistance)
        {
            // ���� ���� �ۿ� �ִ� ���
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

            Debug.Log("�������� ����� �ð�: " + time);

            pistrisAnimation.ChangeRunAnimation();
            state = PistrisState.Chase;
            isChaseDelay = false;

            StopCoroutine("WaitChase");
        }
    }

    // ���� ���� ������Ʈ
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

    // ���� ���� ������Ʈ
    void UpdateDash()
    {
        if (!isDashDelay)
        {
            // �÷��̾ �ٶ󺸰� �ִٰ� ���� �ð� �ڿ� �÷��̾ ���� ����
            StartCoroutine(WaitDashDelay());
        }

        // ���� �������� ������ ���
        if (pistrisAi.ArriveDestination())
        { 
            // �ڽ� �ݶ��̴� Trigger ���� �� �⺻ ���·� ����
            boxCollider.isTrigger = false;
            pistrisAi.StopMove();
            pistrisAnimation.ChangeIdleAnimation();
            state = PistrisState.Idle;

            isDashDelay = false;

            StopCoroutine("WaitDashDelay");
        }
    }

    // ���� ���� ������ ���� ���
    // �÷��̾ �ٶ󺸰� �ִٰ� ���� �ð� �ڿ� �÷��̾ ���� ����
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

    // ���� ���� ������Ʈ
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

    // �ǰ� ���� ������Ʈ
    void UpdateHit()
    {
        StartCoroutine(WaitAnimation());
    }

    // �ǰ� �ִϸ��̼��� �� ���� ���� ���� ó��
    IEnumerator WaitAnimation()
    {
        if(!isWaitHitAnimation)
        {
            isWaitHitAnimation = true;

            yield return new WaitForSeconds(pistrisAnimation.GetCurrentAnimationLength());

            if(pistrisData.CurrentHp <= 0)
            {
                // ���� ü���� 0 ������ ��� ���� ���·� ��ȯ
                pistrisAnimation.ChangeDeathAnimation();
                state = PistrisState.Death;
            }
            else
            {
                // �ƴѰ�� �ǰ� ���·� ��ȯ
                pistrisAnimation.ChangeHitAnimation();
                state = PistrisState.Idle;
            }

            pistrisAi.StopMove();

            isWaitHitAnimation = false;

            StopCoroutine("WaitAnimation");
        }
    }

    // ���� ���� ������Ʈ
    void UpdateDeath()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitDeathAnimation());
    }

    // ���� �ִϸ��̼��� �� ���� ���� ���� ó��
    IEnumerator WaitDeathAnimation()
    {
        if (!isWaitDeathAnimation)
        {
            isWaitDeathAnimation = true;

            yield return new WaitForSeconds(pistrisAnimation.GetCurrentAnimationLength());

            DropExpAndGold(); // ����ġ �� ��ȭ ���
            DropItems();      // ������ ���

            // ���� ���� ����Ʈ�� �� �ش� ���Ͱ� �������� Ȯ��
            QuestManager.instance.CheckQuestMonster(this);

            if (ValleyForestDungeonMapManager.instance != null) ValleyForestDungeonMapManager.instance.ClearDungeon();

            // 5�� �� ���� ����
            Destroy(gameObject, 5.0f);

            StopCoroutine("WaitDeathAnimation");
        }
    }

    // �÷��̾� ��ų�� ���� ���� ó�� (�÷��̾� ��ų�� �¾��� ��)
    public override void DamagedPlayerSkill(float skillDamageValue)
    {
        // �÷��̾� ��ų ��������ŭ ������ ü�� ���
        pistrisData.CurrentHp -= skillDamageValue;

        // ���޾� ���� ��� �ѹ��� ����ǵ���
        if (!isDamage) isDamage = true;

        // �⺻ ���³� ���� ������ ���� �ǰ� �ִϸ��̼� ����
        if (state == PistrisState.Idle || state == PistrisState.Chase)
        {
            pistrisAi.StopMove();
            pistrisAnimation.ChangeHitAnimation();
        }

        state = PistrisState.Hit;
    }

    // ���� ������ ��ȯ
    public override MonsterData GetMonsterData()
    {
        return pistrisData;
    }

    // ����ġ �� ��ȭ ���
    protected override void DropExpAndGold()
    {
        PlayerManager.instance.GainExp(pistrisData.Exp);                  // ȹ���� ����ġ ó��
        InventoryManager.instance.Gold.CurrentAmount += pistrisData.Gold; // ȹ���� ��ȭ(���) ó��
    }

    // ������ ���
    protected override void DropItems()
    {
        for(int i = 0; i < pistrisData.DropItemPrefabs.Count; i++)
        {
            // ��� �������� �����ϴ� ���
            if (pistrisData.DropItemPrefabs[i] != null)
            {
                // 0���� 100���� �� �����ϰ� �̱�
                float probability = Random.Range(0.0f, 100.0f);

                //Debug.Log("probability: " + probability);

                // ���� ���ڰ� ������ ��� Ȯ�� ������ ��� (�������� ����Ǵ� ���)
                if (probability <= pistrisData.ItemDropProbability[i])
                {
                    // ���� ��ġ�� ��� ������ ����
                    Instantiate(pistrisData.DropItemPrefabs[i], transform.position + new Vector3(0, 1.0f, 0), transform.rotation);
                }
            }
        }
    }

    // �ش� ���Ϳ� �÷��̾� ������ �Ÿ� ���
    private float CalcPlayerDistance()
    {
        Vector3 pistrisPosition = pistris.position;
        Vector3 playerPosition = player.position;

        float distance = Vector3.Distance(pistrisPosition, playerPosition);

        return distance;
    }

    // ���� ���� ����� ���� ������  
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(WaitBodyAttackDelay());
        }
    }

    // ���Ϳ� ��� ���� �� 0.3�ʸ��� �������� ��
    IEnumerator WaitBodyAttackDelay()
    {
        if(!isBodyAttackDelay)
        {
            isBodyAttackDelay = true;

            //Debug.Log("���� ������");

            // �÷��̾� ������ ó��

            yield return new WaitForSeconds(0.3f);

            isBodyAttackDelay = false;
        }
    }
}
