using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "GameData/Skill/PlayerSkill")]
public class PlayerSkillData : SkillData
{
    public const int SKILL_MAX_LEVEL = 5;            // ��ų �ִ� ����

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

    [SerializeField] Sprite icon;                    // ��ų ������

    [SerializeField] string tooltip;                 // ��ų ����

    [SerializeField] bool isAcquisition;             // ��ų ���� ����

    [SerializeField] int currentLevel = 1;           // ���� ��ų ����

    [SerializeField] float levelUpAddMpConsumption;  // ������ �� �����Ǵ� Mp �Ҹ�
    [SerializeField] float levelUpAddDamage;         // ������ �� �����Ǵ� ������

    [SerializeField]
    float initMpConsumption;                         // �ʱ� Mp �Ҹ� (��ų ���� 1�� ��)
    float mpConsumption;                             // Mp �Ҹ�

    [SerializeField]
    float initDamage;                                // �ʱ� ��ų ������ (��ų ���� 1�� ��)                                 
    float damage;                                    // ��ų ������

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

            // ������ 1�� �ִ� ���� ���̰��� �ǵ��� ����
            currentLevel = Mathf.Clamp(currentLevel, 1, SKILL_MAX_LEVEL);
        }
    }

    public float MpConsumption
    {
        get
        {
            // MP �Ҹ� = �ʱ� Mp �Ҹ� + (���� ���� - 1) * ������ �� �����Ǵ� Mp �Ҹ�
            mpConsumption = initMpConsumption + (currentLevel - 1) * levelUpAddMpConsumption;

            return mpConsumption;
        }
    }

    public float Damage
    {
        get
        {
            // ��ų ������ = �ʱ� ������ + (���� ���� - 1) * ������ �� �����Ǵ� ������
            damage = initDamage + (currentLevel - 1) * levelUpAddDamage;

            return damage;
        }
    }
}
