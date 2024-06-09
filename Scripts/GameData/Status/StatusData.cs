using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "GameData/Status/Status")]
public class StatusData : ScriptableObject
{
    public const int STAT_MAX_POINT = 20;        // 능력치당 최대 스탯 포인트

    [SerializeField]
    float initHp;                                // 초기 HP         (스탯 포인트 0일 때)
    float hp;                                    // HP

    [SerializeField]
    float initMp;                                // 초기 MP         (스탯 포인트 0일 때)
    float mp;                                    // MP

    [SerializeField]
    float initMagicAttack;                       // 초기 마법 공격력 (스탯 포인트 0일 때)
    float magicAttack;                           // 마법 공격력

    [SerializeField]
    float initDefense;                           // 초기 방어력      (스탯 포인트 0일 때)
    float defense;                               // 방어력

    [SerializeField]
    float moveSpeed;                             // 이동 속도

    [Min(0)] [SerializeField]
    float natureRecoveryMpPerSec;                // 1초당 자연 회복되는 MP

    [SerializeField] int hpPoint;                // HP 스탯 포인트
    [SerializeField] int mpPoint;                // MP 스탯 포인트
    [SerializeField] int magicAttackPoint;       // 마법 공격력 스탯 포인트
    [SerializeField] int defensePoint;           // 방어력 스탯 포인트

    [SerializeField] float pointAddHp;           // 스탯 포인트 당 증가되는 HP
    [SerializeField] float pointAddMp;           // 스탯 포인트 당 증가되는 MP
    [SerializeField] float pointAddMagicAttack;  // 스탯 포인트 당 증가되는 마법 공격력
    [SerializeField] float pointAddDefense;      // 스탯 포인트 당 증가되는 방어력

    float weaponAddMagicAttack;                  // 무기로 증가되는 마법 공격력

    public float Hp
    {
        get
        {
            // 현재 HP (최댓값) = 초기 HP + HP 스탯 포인트 * 포인트 당 증가되는 HP
            hp = initHp + hpPoint * pointAddHp;
            return hp;
        }
    }

    public float Mp
    {
        get
        {
            // 현재 MP (최댓값) = 초기 MP + MP 스탯 포인트 * 포인트 당 증가되는 MP
            mp = initMp + mpPoint * pointAddMp;
            return mp;
        }
    }

    public float MagicAttack
    { 
        get
        {
            // 현재 마법 공격력 = 초기 마법 공격력 + 마법 공격력 스탯 포인트 * 포인트 당 증가되는 마법 공격력 + 무기 장착으로 증가된 마법 공격렬
            magicAttack = initMagicAttack + magicAttackPoint * pointAddMagicAttack + weaponAddMagicAttack;
            return magicAttack;
        }
    }

    public float Defense
    {
        get
        {
            // 현재 방어력 = 초기 방어력 + 방어력 스탯 포인트 * 포인트 당 증가되는 방어력
            defense = initDefense + defensePoint + pointAddDefense;
            return defense;
        }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float RunSpeed
    {
        get { return moveSpeed * 2.0f; }
    }

    public int HpPoint
    {
        get { return hpPoint; }
        set
        {
            hpPoint = value;

            // 0과 최대 포인트 사이값이 되도록 변경
            hpPoint = Mathf.Clamp(hpPoint, 0, STAT_MAX_POINT);
        }
    }

    public int MpPoint
    {
        get { return mpPoint; }
        set
        {
            mpPoint = value;

            // 0과 최대 포인트 사이값이 되도록 변경
            mpPoint = Mathf.Clamp(mpPoint, 0, STAT_MAX_POINT);
        }
    }

    public int MagicAttackPoint
    {
        get { return magicAttackPoint; }
        set
        {
            magicAttackPoint = value;

            // 0과 최대 포인트 사이값이 되도록 변경
            magicAttackPoint = Mathf.Clamp(magicAttackPoint, 0, STAT_MAX_POINT);
        }
    }

    public int DefensePoint
    {
        get { return defensePoint; }
        set
        {
            defensePoint = value;

            // 0과 최대 포인트 사이값이 되도록 변경
            defensePoint = Mathf.Clamp(defensePoint, 0, STAT_MAX_POINT);
        }
    }

    public float PointAddHp
    {
        get { return pointAddHp; }
    }

    public float PointAddMp
    {
        get { return pointAddMp; }
    }

    public float PointAddMagicAttack
    {
        get { return pointAddMagicAttack; }
    }

    public float PointAddDefense
    {
        get { return pointAddDefense; }
    }

    public float WeaponAddMagicAttack
    {
        get { return weaponAddMagicAttack; }
        set
        { 
            weaponAddMagicAttack = value;

            // 0보다 작아지지 않도록 변경
            if (weaponAddMagicAttack < 0) weaponAddMagicAttack = 0;
        }
    }

    public float NatureRecoveryMpPerSec
    {
        get { return natureRecoveryMpPerSec; }
    }
}
