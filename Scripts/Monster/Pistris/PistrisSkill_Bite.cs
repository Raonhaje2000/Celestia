using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 피스트리스 스킬 1 - 물어뜯기
public class PistrisSkill_Bite : MonsterSkill
{
    MonsterSkillData skillData; // 해당 스킬의 데이터

    BoxCollider boxCollider;

    Vector3 boxColliderPosition;
    Vector3 boxColliderSize;

    [SerializeField] LayerMask playerMask;

    [SerializeField] bool isDamageDelay; //  플레이어 레이어 마스크
    [SerializeField] bool isAfterDelay;
    [SerializeField] bool isCoolDown;

    private void Awake()
    {
        skillData = ScriptableObject.Instantiate(Resources.Load<MonsterSkillData>("GameData/Skill/Monster/Field03_ValleyForest/PistrisSkill_BiteData"));

        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Start()
    {
        boxColliderPosition = boxCollider.center + new Vector3(0, 0, skillData.MaxRange / 2);
        boxColliderSize = new Vector3(boxCollider.size.x, boxCollider.size.y, skillData.MaxRange - boxCollider.size.z * 0.25f); //# 0.25f는 보정값

        playerMask = LayerMask.GetMask("Player");

        isAfterDelay = false;
    }

    void Update()
    {
        isCoolDown = skillData.IsCooldown;
    }

    // 해당 공격 범위 내에서 플레이어와 충돌했는지 확인
    bool IsHitPlayer()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.TransformPoint(boxColliderPosition), boxColliderSize / 2, transform.rotation, playerMask);

        if (hitColliders.Length != 0) return true;
        else return false;
    }

    // 몬스터의 스킬 사용
    public override void UseSkill()
    {
        if (UseSkillPossible())
        {
            StartCoroutine(WaitUsingSkill()); // 스킬 사용 시간동안 대기
            StartCoroutine(WaitCoolTime());   // 스킬 사용과 동시에 재사용 대기 시간 시작
        }
    }


    // 스킬 사용 시간동안 대기
    IEnumerator WaitUsingSkill()
    {
        if (!isDamageDelay)
        {
            isDamageDelay = true;

            yield return new WaitForSeconds(skillData.DamageTime);

            isDamageDelay = false;

            if (IsHitPlayer())
            {
                float decreasePercentage = PlayerManager.instance.PlayerStatus.Defense / 100.0f; // 방어구로 인한 데미지 감소율 %
                float damage = (1.0f - decreasePercentage / 100.0f) * skillData.Damage;          // 플레이어가 실제로 받는 데미지

                if (!PlayerController.instance.anime.GetBool("Die")) PlayerController.instance.Damage();

                PlayerManager.instance.CurrentHp -= damage;

                Debug.Log("Player Damage: " + damage);
            }

            StartCoroutine(WaitAfterDelay());

            StopCoroutine("UsingSkill");
        }
    }

    // 스킬 쿨타임 시간동안 대기
    IEnumerator WaitCoolTime()
    {
        if (!skillData.IsCooldown)
        {
            skillData.IsCooldown = true;

            yield return new WaitForSeconds(skillData.CooldownTime);

            skillData.IsCooldown = false;
            StopCoroutine("WaitCoolTime");
        }
    }

    // 스킬 사용 후 딜레이 시간 동안 대기
    IEnumerator WaitAfterDelay()
    {
        if (!isAfterDelay)
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
