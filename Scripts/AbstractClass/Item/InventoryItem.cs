using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : Item
{
    /// <summary>
    /// 해당 인벤토리 아이템의 데이터 세팅<br/>
    /// 새로운 묶음을 만든 뒤 아이템 데이터를 세팅할 때 호출</br>
    /// ScriptableObject.Instantiate(해당 클래스의 아이템 데이터 변수명) 꼴로 사용                                          
    /// </summary>
    public abstract void SetInventoryItemData();

    /// <summary>
    /// 해당 인벤토리 아이템의 데이터 반환<br/>
    /// 인벤토리 아이템 데이터가 필요할 때 호출
    /// </summary>
    /// <returns> 인벤토리 아이템 데이터 반환 </returns>
    public abstract InventoryItemData GetInventoryItemData();

    /// <summary>
    /// 해당 인벤토리 아이템의 새로운 묶음(인스턴스) 생성 후 반환<br/>
    /// 인벤토리에서 새로운 묶음을 만들 때 호출
    /// </summary>
    /// <returns> 새로 만든 클래스 반환 (자료형은 해당 아이템 클래스) </returns>
    public abstract InventoryItem CreateNewBunddle();
}
