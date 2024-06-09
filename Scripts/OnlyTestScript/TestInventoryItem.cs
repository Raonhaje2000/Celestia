using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventoryItem : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] PlayerManager playerManager;

    [SerializeField] GameObject hpItem;
    [SerializeField] GameObject mpItem;

    [SerializeField] InventoryItem hp;
    [SerializeField] InventoryItem mp;

    private void Awake()
    {

    }

    void Start()
    {
        inventoryManager = InventoryManager.instance;
        playerManager = PlayerManager.instance;

        playerManager.CurrentHp = 0;

        //Debug.Log("플레이어의 현재 체력: " + playerManager.CurrentHp + " / 플레이어의 최대 체력: " + playerManager.MaxHp);

        TestItem();

        //Debug.Log("플레이어의 현재 체력: " + playerManager.CurrentHp + " / 플레이어의 최대 체력: " + playerManager.MaxHp);
    }

    private void Update()
    {
        //if (((PotionItem)inventoryManager.InventoryItems[0]).UsePossible())
        //{
        //    if (inventoryManager.InventoryItems.Count == 3)
        //    {
        //        UesItem(2);
        //    }
        //    else
        //    {
        //        UesItem(0);
        //    }
        //}
    }

    void TestItem()
    {
        hpItem = GameObject.Find("Item1");
        mpItem = GameObject.Find("Item2");

        hp = hpItem.GetComponent<InventoryItem>();
        mp = mpItem.GetComponent<InventoryItem>();

        //inventoryManager.AddItems(hpItem.GetComponent<InventoryItem>(), 100);
        //inventoryManager.AddItems(mpItem.GetComponent<InventoryItem>(), 30);
        //inventoryManager.AddItems(hpItem.GetComponent<InventoryItem>(), 100);

        //// Hp 99, Hp 99, Mp 30, HP 2

        //inventoryManager.DeleteItems(hpItem.GetComponent<InventoryItem>(), 200);

        //inventoryManager.AddItems(hpItem.GetComponent<InventoryItem>(), 2);

        InvokeRepeating("DelayAddItem", 0.1f, 1.0f);

        //inventoryManager.AddItems(hpItem.GetComponent<InventoryItem>(), 80);
        //inventoryManager.AddItems(mpItem.GetComponent<InventoryItem>(), mpItem.GetComponent<InventoryItem>().GetInventoryItemData().BundleMaxCount * 28);
        //inventoryManager.AddItems(hpItem.GetComponent<InventoryItem>(), 59 + 1);
        //inventoryManager.AddItems(mpItem.GetComponent<InventoryItem>(), 1);
        //inventoryManager.AddItems(hpItem.GetComponent<InventoryItem>(), 98);

        //Destroy(mpItem);

        Invoke("PrintInventoryItems", 10.0f);
        
        //PrintInventoryItems();

        //if (inventoryManager.InventoryItems[0] != null)
        //{
        //    Debug.Log("null 아님");
        //}

        //UesItem(0);

        //// 아이템 사용 예시
        //((PotionItem)inventoryItems[0]).UseProtionItem();
        //((PotionItem)inventoryItems[3]).UseProtionItem();
    }

    void DelayAddItem()
    {
        inventoryManager.AddItems(mp, 10);
        Debug.Log("아이템 추가");
    }

    void UesItem(int index)
    {
        ((PotionItem)inventoryManager.InventoryItems[index]).UseProtionItem();

        PrintInventoryItems();
    }

    void PrintInventoryItems()
    {
        string temp = "";

        for (int i = 0; i < inventoryManager.InventoryItems.Count; i++)
        {
            string itemName = inventoryManager.InventoryItems[i].GetInventoryItemData().ItemName;
            int count = inventoryManager.InventoryItems[i].GetInventoryItemData().CurrentCount;

            //temp += ("[" + i + "] " + itemName + " (" + count + ") / ");

            Debug.Log("[ " + i + " ] " + itemName + " (현재 슬롯 보유 수량: " + count + " )");
        }

        //Debug.Log(temp);
    }
}
