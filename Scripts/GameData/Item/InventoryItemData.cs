using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemData : ItemData
{
    public enum InventoryItemType
    { 
        Equipment = 0,  // ���
        Consumable = 1, // �Ҹ� (����)
        Drop = 2        // ���
    }

    [SerializeField] protected InventoryItemType inventoryType;    // ����

    [SerializeField] int bundleMaxCount;                           // ���� �ִ� ���� (������ �Ѿ ��� ���ο� ������ ����)
    [SerializeField] int currentCount;                             // ���� ����      (�ش� ���������� ����)

    [SerializeField] bool purchasePossible;                        // ���� ���� ���� ����

    [SerializeField] int purchasePrice = -1;                       // ���� ����      (���� �� �κ��丮)
    [SerializeField] int sellingPrice;                             // �Ǹ� ����      (�κ��丮 �� ����)

    
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
            // �������� ���� �Ұ����� ��� �Ǹ� ������ -1
            purchasePrice = (!purchasePossible) ? -1 : purchasePrice;
            return purchasePrice;
        }
    }

    public int SellingPrice
    {
        get
        {
            // �������� ���� ������ �������� ���� ������ 60% (�Ҽ��� ����ó��)
            sellingPrice = (purchasePossible) ? Mathf.FloorToInt(purchasePrice * 0.6f) : sellingPrice;
            return sellingPrice;
        }
    }
}
