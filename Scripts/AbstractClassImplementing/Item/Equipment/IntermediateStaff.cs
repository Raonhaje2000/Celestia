using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateStaff : EquipmentItme
{
    EquipmentItmeData intermediateStaff; // �߱� ������ ������ ������

    private void Awake()
    {
        // �ش� �κ��丮 �������� ������ ����
        SetInventoryItemData();
    }

    // �ش� �κ��丮 �������� ������ ����
    public override void SetInventoryItemData()
    {
        intermediateStaff = ScriptableObject.Instantiate(Resources.Load<EquipmentItmeData>("GameData/Item/Equipment/IntermediateStaffData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return intermediateStaff;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        IntermediateStaff newBunndle = gameObject.AddComponent<IntermediateStaff>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ��� �������� ��� ���Կ� ����
    public override void EquipSlot()
    {
        // ����� �����Ǵ� ���� ���ݷ��� �ش� ��� �������� ���� ���ݷ����� ����
        PlayerManager.instance.PlayerStatus.WeaponAddMagicAttack = intermediateStaff.MagicAttack;
    }
}