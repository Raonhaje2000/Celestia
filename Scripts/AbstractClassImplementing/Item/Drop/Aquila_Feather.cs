using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquila_Feather : DropItem
{
    DropItemData aquila_Feather; // �������� ���� ������ ������

    private void Awake()
    {
        SetInventoryItemData();
    }

    private void Start()
    {
        SetInit();        // ��� ������ �ʱ�ȭ
        BounceDropItem(); // ������ ���ÿ� ��� ������ Ƣ�������
    }

    // �ش� �κ��丮 �������� ������ ����
    public override void SetInventoryItemData()
    {
        aquila_Feather = ScriptableObject.Instantiate(Resources.Load<DropItemData>("GameData/Item/Drop/Field_03/Aquila_FeatherData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return aquila_Feather;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        Aquila_Feather newBunndle = gameObject.AddComponent<Aquila_Feather>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ȹ���� �ش� ��� ������ ��ȯ
    public override DropItem ObtainDropItem()
    {
        return this;
    }

    // �ش� ��� ������ �κ��丮�� �߰�
    public override void MoveItemToInventory()
    {
        // �ش� ��� ������ �κ��丮�� 1�� �߰�
        InventoryManager.instance.AddItems(this, 1);

        CancelInvoke("MoveItemToInventory");
        Destroy(this.gameObject, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ���� �� 1.5�� �� �ڵ����� �κ��丮�� �߰�
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log(aquila_Feather.ItemName + " ���� ����");
            Invoke("MoveItemToInventory", 1.5f);
        }
    }
}