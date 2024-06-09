using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : Item
{
    /// <summary>
    /// �ش� �κ��丮 �������� ������ ����<br/>
    /// ���ο� ������ ���� �� ������ �����͸� ������ �� ȣ��</br>
    /// ScriptableObject.Instantiate(�ش� Ŭ������ ������ ������ ������) �÷� ���                                          
    /// </summary>
    public abstract void SetInventoryItemData();

    /// <summary>
    /// �ش� �κ��丮 �������� ������ ��ȯ<br/>
    /// �κ��丮 ������ �����Ͱ� �ʿ��� �� ȣ��
    /// </summary>
    /// <returns> �κ��丮 ������ ������ ��ȯ </returns>
    public abstract InventoryItemData GetInventoryItemData();

    /// <summary>
    /// �ش� �κ��丮 �������� ���ο� ����(�ν��Ͻ�) ���� �� ��ȯ<br/>
    /// �κ��丮���� ���ο� ������ ���� �� ȣ��
    /// </summary>
    /// <returns> ���� ���� Ŭ���� ��ȯ (�ڷ����� �ش� ������ Ŭ����) </returns>
    public abstract InventoryItem CreateNewBunddle();
}
