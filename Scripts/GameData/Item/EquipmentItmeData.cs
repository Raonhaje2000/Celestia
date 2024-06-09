using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItme", menuName = "GameData/Item/EquipmentItme")]                            
public class EquipmentItmeData : InventoryItemData
{
    [SerializeField] float magicAttack; // ���� ���ݷ�

    public EquipmentItmeData()
    {
        base.inventoryType = InventoryItemType.Equipment;
    }

    public float MagicAttack
    {
        get { return magicAttack; }
    }
}
