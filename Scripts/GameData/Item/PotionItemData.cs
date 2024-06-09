using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionItem", menuName = "GameData/Item/PotionItem")]                                          
public class PotionItemData : InventoryItemData
{
    public enum RecoveryItemType { Hp = 0, Mp = 1 }

    [SerializeField] RecoveryItemType recoveryType;   // 회복 타입

    [SerializeField] float recoveryPercentage;        // 회복 수치 (%)

    [SerializeField] float cooldownTime;              // 재사용 대기시간 (쿨타임)

    public PotionItemData()
    {
        base.inventoryType = InventoryItemType.Consumable;
    }

    public RecoveryItemType RecoveryType
    { 
        get { return recoveryType; }
    }

    public float RecoveryPercentage
    {
        get { return recoveryPercentage; }
    }

    public float CooldownTime
    {
        get { return cooldownTime; }
    }
}
