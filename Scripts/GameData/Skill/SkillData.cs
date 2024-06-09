using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public enum SkillUserType { Player = 0, Monster = 1 }

    [SerializeField] protected SkillUserType userType; // 사용자 (시전자)

    [SerializeField] int skillId;                      // 스킬 ID
    [SerializeField] string skillName;                 // 스킬명

    [SerializeField] float duration;                   // 지속 시간
    [SerializeField] float afterDelay;                 // 사용 후 딜레이 (스킬 사용 후 움직이지 못하는 시간)

    [SerializeField]
    float cooldownTime;                                // 재사용 대기시간 (쿨타임)
    bool isCooldown;                                   // 재사용 대기시간 (쿨타임) 플래그

    [SerializeField] float maxRange;                   // 최대 공격 범위 (최대 사정 거리)

    [SerializeField] GameObject skillEffectPrefab;     // 스킬 이펙트 프리팹
    //[SerializeField] GameObject attackEffectPrefab;    // 피격 이펙트 프리팹

    // 스킬 데미지의 경우 플레이어 스킬과 몬스터 스킬의 데미지 산정 방식(레벨에 따른 데미지)이 다르므로 각 자식 클래스에서 다룸

    public SkillUserType UserType
    {
        get { return userType; }
    }

    public int SkillId
    {
        get { return skillId; }
    }

    public string SkillName
    {
        get { return skillName; }
    }

    public float Duration
    { 
        get { return duration; }
    }

    public float AfterDelay
    {
        get { return afterDelay; }
    }

    public float CooldownTime
    {
        get { return cooldownTime; }
    }

    public bool IsCooldown
    { 
        get { return isCooldown; }
        set { isCooldown = value; }
    }

    public float MaxRange
    {
        get { return maxRange; }
    }

    public GameObject SkillEffectPrefab
    {
        get { return skillEffectPrefab; }
    }

    //public GameObject AttackEffectPrefab
    //{
    //    get { return attackEffectPrefab; }
    //}
}
