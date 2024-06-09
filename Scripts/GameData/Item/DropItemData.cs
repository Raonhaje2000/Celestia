using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem", menuName = "GameData/Item/DropItem")]                                             
public class DropItemData : InventoryItemData
{
    [SerializeField] float dropProbability = 100; // µå¶ø È®·ü

    public DropItemData()
    {
        base.inventoryType = InventoryItemType.Drop;
    }

    public float DropProbability
    {
        get { return dropProbability; }
    }
}
