using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ������ ��ų 1 - ���Ⱕ�� ����
public class LupusSkill_Ripping : MonsterSkill
{
    MonsterSkillData skillData; // �ش� ��ų�� ������

    BoxCollider boxCollider;

    Vector3 boxColliderPosition;
    Vector3 boxColliderSize;

    [SerializeField] LayerMask playerMask; // �÷��̾� ���̾� ����ũ

    [SerializeField] bool isDamageDelay;
    [SerializeField] bool isAfterDelay;
    [SerializeField] bool isCoolDown;

    private void Awake()
    {
        skillData = ScriptableObject.Instantiate(Resources.Load<MonsterSkillData>("GameData/Skill/Monster/Field03_ValleyForest/LupusSkill_RippingData"));

        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Start()
    {
        boxColliderPosition = boxCollider.center + new Vector3(0, 0, skillData.MaxRange / 2);
        boxColliderSize = new Vector3(boxCollider.size.x, boxCollider.size.y, skillData.MaxRange - boxCollider.size.z * 0.25f); //# 0.25f�� ������

        playerMask = LayerMask.GetMask("Player");

        isAfterDelay = false;
    }

    private void Update()
    {
        isCoolDown = skillData.IsCooldown;
    }

    //�ش� ���� ���� ������ �÷��̾�� �浹�ߴ��� Ȯ��
    bool IsHitPlayer()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.TransformPoint(boxColliderPosition), boxColliderSize / 2, transform.rotation, playerMask);

        if (hitColliders.Length != 0) return true;
        else return false;
    }

    // ������ ��ų ���
    public override void UseSkill()
    {
        if(UseSkillPossible())
        {
            StartCoroutine(WaitCoolTime());   // ��ų ���� ���ÿ� ���� ��� �ð� ����            
            StartCoroutine(WaitUsingSkill()); // ��ų ��� �ð����� ���
        }
    }

    // ��ų ��� �ð����� ���
    IEnumerator WaitUsingSkill()
    {
        if(!isDamageDelay)
        {
            isDamageDelay = true;

            yield return new WaitForSeconds(skillData.DamageTime);

            isDamageDelay = false;

            if (IsHitPlayer())
            {
                float decreasePercentage = PlayerManager.instance.PlayerStatus.Defense / 100.0f; // ���� ���� ������ ������ %
                float damage = (1.0f - decreasePercentage / 100.0f) * skillData.Damage;          // �÷��̾ ������ �޴� ������

                if(!PlayerController.instance.anime.GetBool("Die")) PlayerController.instance.Damage();

                PlayerManager.instance.CurrentHp -= damage;

                Debug.Log("Player Damage: " + damage);
            }

            StartCoroutine(WaitAfterDelay());

            StopCoroutine("UsingSkill");
        }
    }

    // ��ų ��Ÿ�� �ð����� ���
    IEnumerator WaitCoolTime()
    {
        if(!skillData.IsCooldown)
        {
            skillData.IsCooldown = true;

            yield return new WaitForSeconds(skillData.CooldownTime);

            skillData.IsCooldown = false;
            StopCoroutine("WaitCoolTime");
        }
    }

    // ��ų ��� �� ������ �ð� ���� ���
    IEnumerator WaitAfterDelay()
    {
        if(!isAfterDelay)
        {
            isAfterDelay = true;

            yield return new WaitForSeconds(skillData.AfterDelay);

            isAfterDelay = false;
            StopCoroutine("WaitAfterDelay");
        }
    }

    public override MonsterSkillData GetMonsterSkillData()
    {
        return skillData;
    }

    public override bool UseSkillPossible()
    {
        return !skillData.IsCooldown && !isAfterDelay;
    }

    private void OnDrawGizmos()
    {
        if (skillData != null)
        {
            Matrix4x4 prevMatrix = Gizmos.matrix;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxColliderPosition, boxColliderSize);

            Gizmos.matrix = prevMatrix;
        }
    }
}
