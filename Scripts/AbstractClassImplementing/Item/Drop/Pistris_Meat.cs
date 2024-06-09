using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistris_Meat : DropItem
{
    DropItemData pistris_Meat; // �ǽ�Ʈ������ ��� ������ ������

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
        pistris_Meat = ScriptableObject.Instantiate(Resources.Load<DropItemData>("GameData/Item/Drop/Field_03/Pistris_MeatData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return pistris_Meat;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        Pistris_Meat newBunndle = gameObject.AddComponent<Pistris_Meat>();

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
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ���� �� 1.5�� �� �ڵ����� �κ��丮�� �߰�
        if (collision.gameObject.tag == "Ground")
        {
            //Debug.Log(pistris_Meat.ItemName + " ���� ����");
            Invoke("MoveItemToInventory", 1.5f);
        }
    }
}