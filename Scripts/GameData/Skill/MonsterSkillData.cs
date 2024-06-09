using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSkill", menuName = "GameData/Skill/MonsterSkill")]                                  
public class MonsterSkillData : SkillData
{
    public MonsterSkillData()
    {
        base.userType = SkillUserType.Monster;
    }

    [SerializeField] float damage;     // 스킬 데미지
    [SerializeField] float damageTime; // 스킬 데미지가 들어가는 시간

    public float Damage
    {
        get { return damage; }
    }

    public float DamageTime
    {
        get { return damageTime; }
    }
}
