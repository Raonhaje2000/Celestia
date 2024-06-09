using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerStaff : EquipmentItme
{
    [SerializeField] EquipmentItmeData beginnerStaff; // 초급 스태프 아이템 데이터                                                            

    private void Awake()
    {
        // 해당 인벤토리 아이템의 데이터 세팅
        SetInventoryItemData();
    }

    // 해당 인벤토리 아이템의 데이터 세팅
    public override void SetInventoryItemData()
    {
        beginnerStaff = ScriptableObject.Instantiate(Resources.Load<EquipmentItmeData>("GameData/Item/Equipment/BeginnerStaffData"));
    }

    // 해당 인벤토리 아이템의 데이터 반환
    public override InventoryItemData GetInventoryItemData()
    {
        return beginnerStaff;
    }

    // 해당 인벤토리 아이템의 새로운 묶음(컴포넌트) 생성 후 반환
    public override InventoryItem CreateNewBunddle()
    {
        // 묶음 생성 후 아이템 데이터 세팅
        BeginnerStaff newBunndle = gameObject.AddComponent<BeginnerStaff>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // 장비 아이템을 장비 슬롯에 장착
    public override void EquipSlot()
    {
        // 무기로 증가되는 마법 공격력을 해당 장비 아이템의 마법 공격력으로 변경
        PlayerManager.instance.PlayerStatus.WeaponAddMagicAttack = beginnerStaff.MagicAttack;                                      
    }
}
