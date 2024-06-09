using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    /// <summary>
    /// ���Ͱ� �÷��̾� ��ų�� ���� ���� ó��<br/>
    /// ��ų���� �浹 ó���� �� �� ȣ����
    /// </summary>
    /// <param name="skillDamageValue"> ���Ͱ� ���� ��ų�� ������ </param>                                              
    public abstract void DamagedPlayerSkill(float skillDamageValue);

    /// <summary>
    /// ���Ͱ� �ִ� ����ġ �� ��ȭ ��� ó��<br/>
    /// ���Ͱ� �׾��� �� ȣ����
    /// </summary>
    protected abstract void DropExpAndGold();

    /// <summary>
    /// ������ ������ ���(��� ������ ������Ʈ ����) ó��<br/>
    /// ������ ��� Ȯ���� ���� ��� �������� ����<br/>
    /// ���Ͱ� �׾��� �� ȣ����
    /// </summary>
    protected abstract void DropItems();

    
    /// <summary>
    /// ���� ������ ��ȯ
    /// </summary>
    /// <returns>�ش� ������ ������</returns>
    public abstract MonsterData GetMonsterData();
}
