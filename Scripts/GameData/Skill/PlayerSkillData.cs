using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "GameData/Skill/PlayerSkill")]
public class PlayerSkillData : SkillData
{
    public const int SKILL_MAX_LEVEL = 5;            // 스킬 최대 레벨

    public enum NameId
    {
        Fireball = 10101,
        Explosion = 10102,
        Blaze = 10103,
        Meteor = 10104
    }

    public PlayerSkillData()
    {
        base.userType = SkillUserType.Player;
    }

    [SerializeField] Sprite icon;                    // 스킬 아이콘

    [SerializeField] string tooltip;                 // 스킬 툴팁

    [SerializeField] bool isAcquisition;             // 스킬 습득 유무

    [SerializeField] int currentLevel = 1;           // 현재 스킬 레벨

    [SerializeField] float levelUpAddMpConsumption;  // 레벨업 시 증가되는 Mp 소모량
    [SerializeField] float levelUpAddDamage;         // 레벨업 시 증가되는 데미지

    [SerializeField]
    float initMpConsumption;                         // 초기 Mp 소모량 (스킬 레벨 1일 때)
    float mpConsumption;                             // Mp 소모량

    [SerializeField]
    float initDamage;                                // 초기 스킬 데미지 (스킬 레벨 1일 때)                                 
    float damage;                                    // 스킬 데미지

    public Sprite Icon
    {
        get { return icon; }
    }

    public string Tooltip
    {
        get { return tooltip; }
    }

    public bool IsAcquisition
    {
        get { return isAcquisition; }
        set { isAcquisition = value; }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set
        {
            currentLevel = value;

            // 레벨이 1과 최대 레벨 사이값이 되도록 변경
            currentLevel = Mathf.Clamp(currentLevel, 1, SKILL_MAX_LEVEL);
        }
    }

    public float MpConsumption
    {
        get
        {
            // MP 소모량 = 초기 Mp 소모량 + (현재 레벨 - 1) * 레벨업 시 증가되는 Mp 소모량
            mpConsumption = initMpConsumption + (currentLevel - 1) * levelUpAddMpConsumption;

            return mpConsumption;
        }
    }

    public float Damage
    {
        get
        {
            // 스킬 데미지 = 초기 데미지 + (현재 레벨 - 1) * 레벨업 시 증가되는 데미지
            damage = initDamage + (currentLevel - 1) * levelUpAddDamage;

            return damage;
        }
    }
}
