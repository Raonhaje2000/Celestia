using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDropItem : MonoBehaviour
{
    InventoryManager inventoryManager;

    GameObject item1;
    GameObject item2;

    private void Awake()
    {
        inventoryManager = InventoryManager.instance;

        item1 = Resources.Load<GameObject>("Prefabs/DropItem/Field03_ValleyForest/DropItem_Lupus_Tooth");
        item2 = Resources.Load<GameObject>("Prefabs/DropItem/Field03_ValleyForest/DropItem_Pistris_Meat");
    }

    void Start()
    {
        Instantiate(item1, transform.position, transform.rotation);
        Instantiate(item1, transform.position, transform.rotation);
        Instantiate(item1, transform.position, transform.rotation);
        Instantiate(item2, transform.position, transform.rotation);

        Invoke("DebugInventory", 5.0f);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Instantiate(item1);
        //}
        //else if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Instantiate(item2);
        //}
    }

    void DebugInventory()
    {
        for (int i = 0; i < inventoryManager.InventoryItems.Count; i++)
        {
            string itemName = inventoryManager.InventoryItems[i].GetInventoryItemData().ItemName;
            int count = inventoryManager.InventoryItems[i].GetInventoryItemData().CurrentCount;

            Debug.Log("[ " + i + " ] " + itemName + " (현재 슬롯 보유 수량: " + count + " )");
        }
    }
}
