using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerStaff : EquipmentItme
{
    [SerializeField] EquipmentItmeData beginnerStaff; // �ʱ� ������ ������ ������                                                            

    private void Awake()
    {
        // �ش� �κ��丮 �������� ������ ����
        SetInventoryItemData();
    }

    // �ش� �κ��丮 �������� ������ ����
    public override void SetInventoryItemData()
    {
        beginnerStaff = ScriptableObject.Instantiate(Resources.Load<EquipmentItmeData>("GameData/Item/Equipment/BeginnerStaffData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return beginnerStaff;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        BeginnerStaff newBunndle = gameObject.AddComponent<BeginnerStaff>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ��� �������� ��� ���Կ� ����
    public override void EquipSlot()
    {
        // ����� �����Ǵ� ���� ���ݷ��� �ش� ��� �������� ���� ���ݷ����� ����
        PlayerManager.instance.PlayerStatus.WeaponAddMagicAttack = beginnerStaff.MagicAttack;                                      
    }
}
