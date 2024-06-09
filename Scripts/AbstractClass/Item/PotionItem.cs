using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionItem : InventoryItem
{
    /// <summary>
    /// ���� �������� ������� ���� ȿ�� ó��<br/>
    /// ���� �������� Ŭ������ �� ȣ����
    /// </summary>
    public abstract void UseProtionItem();                                                              

    /// <summary>
    /// ���� �������� ��� �������� Ȯ��</br>
    /// ���� �������� Ŭ���� ����ϱ� ���� ȣ����</br>
    /// ���� ������ 1�� �̻��̰�, ���� ���ð��� �ƴϸ�,
    /// ������ ������� �÷��̾ ȸ�� �� �� �ִ� ��ġ�� �������� �� ��� ����                                               
    /// </summary>
    /// <returns>��� �������� Ȯ���ϴ� �÷���</returns>
    public abstract bool UsePossible();

    /// <summary>
    /// ���� ���ð� ������Ʈ ó��</br>
    /// ���� ���ð� ���� �������� ���� ���ð��� ������Ʈ �� �� ȣ����
    /// </summary>
    /// <param name="time">�帥 �ð�</param>
    /// <returns>����� ���� ���ð�</returns>
    public abstract void UpdatePotionCoolDownTime(float deltaTime);

    /// <summary>
    /// �ش� ���� ������ ������ ���� ���ð� ��ȯ ó��</br>
    /// �ش� ���� ������ ������ ���� ���ð��� Ȯ���� �� ȣ����</br>
    /// (Ư�� ���� ���� ���ð� �ؽ�Ʈ ǥ���� ��)
    /// </summary>
    /// <returns>���� ���ð�(�ʴ���)</returns>
    public abstract float GetPotionCoolDownTime();

    /// <summary>
    /// �ش� ���� ������ ������ ���� ���ð����� Ȯ��
    /// �ش� ���� ������ ������ �ִ� ���Ե��� Ȱ��ȭ ���θ� ������ �� ȣ����
    /// </summary>
    /// <returns>���� ���ð����� Ȯ���ϴ� �÷���</returns>
    public abstract bool IsPotionCoolDownTime();
}
