using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItme : InventoryItem
{
    /// <summary>
    /// 장비 아이템을 장비 슬롯에 장착했을 때 플레이어의 능력치 변화 처리<br/>                                               
    /// 장비 아이템을 장비 슬롯에 장착할 때 호출함
    /// </summary>
    public abstract void EquipSlot();
}
