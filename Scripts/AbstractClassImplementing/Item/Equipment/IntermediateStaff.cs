using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateStaff : EquipmentItme
{
    EquipmentItmeData intermediateStaff; // 중급 스태프 아이템 데이터

    private void Awake()
    {
        // 해당 인벤토리 아이템의 데이터 세팅
        SetInventoryItemData();
    }

    // 해당 인벤토리 아이템의 데이터 세팅
    public override void SetInventoryItemData()
    {
        intermediateStaff = ScriptableObject.Instantiate(Resources.Load<EquipmentItmeData>("GameData/Item/Equipment/IntermediateStaffData"));
    }

    // 해당 인벤토리 아이템의 데이터 반환
    public override InventoryItemData GetInventoryItemData()
    {
        return intermediateStaff;
    }

    // 해당 인벤토리 아이템의 새로운 묶음(컴포넌트) 생성 후 반환
    public override InventoryItem CreateNewBunddle()
    {
        // 묶음 생성 후 아이템 데이터 세팅
        IntermediateStaff newBunndle = gameObject.AddComponent<IntermediateStaff>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // 장비 아이템을 장비 슬롯에 장착
    public override void EquipSlot()
    {
        // 무기로 증가되는 마법 공격력을 해당 장비 아이템의 마법 공격력으로 변경
        PlayerManager.instance.PlayerStatus.WeaponAddMagicAttack = intermediateStaff.MagicAttack;
    }
}