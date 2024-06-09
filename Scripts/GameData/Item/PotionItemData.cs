using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionItem", menuName = "GameData/Item/PotionItem")]                                          
public class PotionItemData : InventoryItemData
{
    public enum RecoveryItemType { Hp = 0, Mp = 1 }

    [SerializeField] RecoveryItemType recoveryType;   // ȸ�� Ÿ��

    [SerializeField] float recoveryPercentage;        // ȸ�� ��ġ (%)

    [SerializeField] float cooldownTime;              // ���� ���ð� (��Ÿ��)

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
