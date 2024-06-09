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

    [SerializeField] float damage;     // ��ų ������
    [SerializeField] float damageTime; // ��ų �������� ���� �ð�

    public float Damage
    {
        get { return damage; }
    }

    public float DamageTime
    {
        get { return damageTime; }
    }
}
