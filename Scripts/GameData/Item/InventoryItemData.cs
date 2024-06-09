using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemData : ItemData
{
    public enum InventoryItemType
    { 
        Equipment = 0,  // 장비
        Consumable = 1, // 소모 (포션)
        Drop = 2        // 드랍
    }

    [SerializeField] protected InventoryItemType inventoryType;    // 종류

    [SerializeField] int bundleMaxCount;                           // 묶음 최대 수량 (수량을 넘어선 경우 새로운 묶음을 만듦)
    [SerializeField] int currentCount;                             // 보유 수량      (해당 묶음에서의 수량)

    [SerializeField] bool purchasePossible;                        // 상점 구매 가능 여부

    [SerializeField] int purchasePrice = -1;                       // 구매 가격      (상점 → 인벤토리)
    [SerializeField] int sellingPrice;                             // 판매 가격      (인벤토리 → 상점)

    
    public InventoryItemType InventoryType
    {
        get { return inventoryType; }
    }

    public int BundleMaxCount
    {
        get { return bundleMaxCount; }
    }

    public int CurrentCount
    {
        get { return currentCount; }
        set { currentCount = value;}
    }

    public bool PurchasePossible
    {
        get { return purchasePossible; }
    }

    public int PurchasePrice
    {
        get
        {
            // 상점에서 구매 불가능한 경우 판매 가격은 -1
            purchasePrice = (!purchasePossible) ? -1 : purchasePrice;
            return purchasePrice;
        }
    }

    public int SellingPrice
    {
        get
        {
            // 상점에서 구매 가능한 아이템은 구매 가격의 60% (소수점 버림처리)
            sellingPrice = (purchasePossible) ? Mathf.FloorToInt(purchasePrice * 0.6f) : sellingPrice;
            return sellingPrice;
        }
    }
}
