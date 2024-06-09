using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Lupus_Tooth : DropItem
{
    DropItemData lupus_Tooth; // ��Ǫ���� �̻� ������ ������
    [SerializeField]
    GameObject text;          // �������� �̸� �ؽ�Ʈ

    private void Awake()
    {
        // �ش� �κ��丮 �������� ������ ����
        SetInventoryItemData();
    }

    private void Start()
    {
        // �ش� �������� �̸� �ؽ�Ʈ�� �ҷ��� �� ��Ȱ��ȭ
        text = transform.Find("ItemText").gameObject;
        text.SetActive(false);

        SetInit();        // ��� ������ �ʱ�ȭ
        BounceDropItem(); // ������ ���ÿ� ��� ������ Ƣ�������
    }

    // �ش� �κ��丮 �������� ������ ����
    public override void SetInventoryItemData()
    {
        lupus_Tooth = ScriptableObject.Instantiate(Resources.Load<DropItemData>("GameData/Item/Drop/Field_03/Lupus_ToothData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return lupus_Tooth;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        Lupus_Tooth newBunndle = gameObject.AddComponent<Lupus_Tooth>();

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
        InventoryManager.instance.AddItems(this, 1);

        CancelInvoke("MoveItemToInventory");
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ���� �� 1.5�� �� �ڵ����� �κ��丮�� �߰�
        if (collision.gameObject.tag == "Ground")
        {
            // �ش� �������� �̸� �ؽ�Ʈ�� Ȱ��ȭ �� �ش� �������� �̸����� �ؽ�Ʈ ����
            text.SetActive(true);
            text.GetComponent<TextMeshPro>().text = lupus_Tooth.ItemName;

            // ���� ī�޶� ���� �������� �̸� �ؽ�Ʈ ȸ��
            transform.rotation = Camera.main.transform.rotation;

            //Debug.Log(lupus_Tooth.ItemName + " ���� ����");
            
            Invoke("MoveItemToInventory", 1.5f);
        }
    }
}