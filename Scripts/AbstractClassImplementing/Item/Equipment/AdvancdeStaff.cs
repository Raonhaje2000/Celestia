using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedStaff : EquipmentItme
{
    EquipmentItmeData advancedStaff; // ��� ������ ������ ������

    private void Awake()
    {
        // �ش� �κ��丮 �������� ������ ����
        SetInventoryItemData();
    }

    // �ش� �κ��丮 �������� ������ ����
    public override void SetInventoryItemData()
    {
        advancedStaff = ScriptableObject.Instantiate(Resources.Load<EquipmentItmeData>("GameData/Item/Equipment/AdvancedStaffData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return advancedStaff;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        AdvancedStaff newBunndle = gameObject.AddComponent<AdvancedStaff>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ��� �������� ��� ���Կ� ����
    public override void EquipSlot()
    {
        // ����� �����Ǵ� ���� ���ݷ��� �ش� ��� �������� ���� ���ݷ����� ����
        PlayerManager.instance.PlayerStatus.WeaponAddMagicAttack = advancedStaff.MagicAttack;
    }
}