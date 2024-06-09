using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSkill : Skill
{
    /// <summary>
    /// �ش� ���� ��ų ������ ��ȯ<br/>
    /// ���� ��ų �����Ͱ� �ʿ��� �� ȣ��
    /// </summary>
    /// <returns>�ش� ���� ��ų ������ ��ȯ</returns>
    public abstract MonsterSkillData GetMonsterSkillData();                                                             

    /// <summary>
    /// ���Ͱ� �ش� ��ų�� ����� �� �ִ���<br/>
    /// ���Ͱ� ��ų�� ����ϱ� �� ��ų ��� ���� ���� �Ǵ��� �� ȣ��
    /// </summary>
    /// <returns>�ش� ��ų ��� ���� ����</returns>
    public abstract bool UseSkillPossible();
}
