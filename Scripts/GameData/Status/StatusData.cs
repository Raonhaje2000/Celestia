using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "GameData/Status/Status")]
public class StatusData : ScriptableObject
{
    public const int STAT_MAX_POINT = 20;        // �ɷ�ġ�� �ִ� ���� ����Ʈ

    [SerializeField]
    float initHp;                                // �ʱ� HP         (���� ����Ʈ 0�� ��)
    float hp;                                    // HP

    [SerializeField]
    float initMp;                                // �ʱ� MP         (���� ����Ʈ 0�� ��)
    float mp;                                    // MP

    [SerializeField]
    float initMagicAttack;                       // �ʱ� ���� ���ݷ� (���� ����Ʈ 0�� ��)
    float magicAttack;                           // ���� ���ݷ�

    [SerializeField]
    float initDefense;                           // �ʱ� ����      (���� ����Ʈ 0�� ��)
    float defense;                               // ����

    [SerializeField]
    float moveSpeed;                             // �̵� �ӵ�

    [Min(0)] [SerializeField]
    float natureRecoveryMpPerSec;                // 1�ʴ� �ڿ� ȸ���Ǵ� MP

    [SerializeField] int hpPoint;                // HP ���� ����Ʈ
    [SerializeField] int mpPoint;                // MP ���� ����Ʈ
    [SerializeField] int magicAttackPoint;       // ���� ���ݷ� ���� ����Ʈ
    [SerializeField] int defensePoint;           // ���� ���� ����Ʈ

    [SerializeField] float pointAddHp;           // ���� ����Ʈ �� �����Ǵ� HP
    [SerializeField] float pointAddMp;           // ���� ����Ʈ �� �����Ǵ� MP
    [SerializeField] float pointAddMagicAttack;  // ���� ����Ʈ �� �����Ǵ� ���� ���ݷ�
    [SerializeField] float pointAddDefense;      // ���� ����Ʈ �� �����Ǵ� ����

    float weaponAddMagicAttack;                  // ����� �����Ǵ� ���� ���ݷ�

    public float Hp
    {
        get
        {
            // ���� HP (�ִ�) = �ʱ� HP + HP ���� ����Ʈ * ����Ʈ �� �����Ǵ� HP
            hp = initHp + hpPoint * pointAddHp;
            return hp;
        }
    }

    public float Mp
    {
        get
        {
            // ���� MP (�ִ�) = �ʱ� MP + MP ���� ����Ʈ * ����Ʈ �� �����Ǵ� MP
            mp = initMp + mpPoint * pointAddMp;
            return mp;
        }
    }

    public float MagicAttack
    { 
        get
        {
            // ���� ���� ���ݷ� = �ʱ� ���� ���ݷ� + ���� ���ݷ� ���� ����Ʈ * ����Ʈ �� �����Ǵ� ���� ���ݷ� + ���� �������� ������ ���� ���ݷ�
            magicAttack = initMagicAttack + magicAttackPoint * pointAddMagicAttack + weaponAddMagicAttack;
            return magicAttack;
        }
    }

    public float Defense
    {
        get
        {
            // ���� ���� = �ʱ� ���� + ���� ���� ����Ʈ * ����Ʈ �� �����Ǵ� ����
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

            // 0�� �ִ� ����Ʈ ���̰��� �ǵ��� ����
            hpPoint = Mathf.Clamp(hpPoint, 0, STAT_MAX_POINT);
        }
    }

    public int MpPoint
    {
        get { return mpPoint; }
        set
        {
            mpPoint = value;

            // 0�� �ִ� ����Ʈ ���̰��� �ǵ��� ����
            mpPoint = Mathf.Clamp(mpPoint, 0, STAT_MAX_POINT);
        }
    }

    public int MagicAttackPoint
    {
        get { return magicAttackPoint; }
        set
        {
            magicAttackPoint = value;

            // 0�� �ִ� ����Ʈ ���̰��� �ǵ��� ����
            magicAttackPoint = Mathf.Clamp(magicAttackPoint, 0, STAT_MAX_POINT);
        }
    }

    public int DefensePoint
    {
        get { return defensePoint; }
        set
        {
            defensePoint = value;

            // 0�� �ִ� ����Ʈ ���̰��� �ǵ��� ����
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

            // 0���� �۾����� �ʵ��� ����
            if (weaponAddMagicAttack < 0) weaponAddMagicAttack = 0;
        }
    }

    public float NatureRecoveryMpPerSec
    {
        get { return natureRecoveryMpPerSec; }
    }
}
