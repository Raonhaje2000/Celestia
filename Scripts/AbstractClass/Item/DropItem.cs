using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropItem : InventoryItem
{
    protected Rigidbody _rigidbody;

    float forceAmount;      // ���ϴ� ���� ��
    Vector3 forceDirection; // ���� ����

    /// <summary>
    /// ��� ������ �ʱ�ȭ
    /// </summary>
    protected void SetInit()
    {
        _rigidbody = GetComponent<Rigidbody>();

        forceDirection = Vector3.up;
        forceAmount = 50;

        Destroy(gameObject, 60); // ���� 60�� �� ����
    }

    /// <summary>
    /// ��� ������ Ƣ�������
    /// </summary>
    protected void BounceDropItem()
    {
        // ���� ��ġ�κ��� �����ϰ� ������ ��ġ�� ������ ����߸���
        float x = Random.Range(-0.5f, 0.5f);
        float z = Random.Range(-0.5f, 0.5f);

        forceDirection = Vector3.Normalize(new Vector3(x, 3.0f, z));

        // ���� ���� (��ü ������ ������ ����)
        _rigidbody.AddForce(forceDirection * forceAmount, ForceMode.Impulse);
    }

    /// <summary>
    /// ��� ������ ȹ��(�κ��丮 ����) ó���� ���� ��� ������ ������ ��ȯ<br/>
    /// �÷��̾ ȹ�� ������ �Ÿ� ������ ������ ȹ�� Ű�� ������ �� ȣ��
    /// </summary>
    /// <returns> ȹ���ϴ� ��� ������ </returns>
    public abstract DropItem ObtainDropItem();

    /// <summary>
    /// ��� �������� ȹ��(�κ��丮�� �߰�)�ϴ� ó��</br>
    /// OnCollisionEnter���� Ground�� �浹�ߴ��� Ȯ�� ��, Invoke���� ȣ���Ͽ� �� �� ������ �� �κ��丮�� ������</br>           
    /// InventoryManager.instance.AddItems() ȣ�� �� �� �� �� �ش� ������ ����
    /// </summary>
    public abstract void MoveItemToInventory();
}
