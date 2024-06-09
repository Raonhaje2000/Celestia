using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistris_Meat : DropItem
{
    DropItemData pistris_Meat; // 피스트리스의 고기 아이템 데이터

    private void Awake()
    {
        SetInventoryItemData();
    }

    private void Start()
    {
        SetInit();        // 드랍 아이템 초기화
        BounceDropItem(); // 생성과 동시에 드랍 아이템 튀어오르기
    }

    // 해당 인벤토리 아이템의 데이터 세팅
    public override void SetInventoryItemData()
    {
        pistris_Meat = ScriptableObject.Instantiate(Resources.Load<DropItemData>("GameData/Item/Drop/Field_03/Pistris_MeatData"));
    }

    // 해당 인벤토리 아이템의 데이터 반환
    public override InventoryItemData GetInventoryItemData()
    {
        return pistris_Meat;
    }

    // 해당 인벤토리 아이템의 새로운 묶음(컴포넌트) 생성 후 반환
    public override InventoryItem CreateNewBunddle()
    {
        // 묶음 생성 후 아이템 데이터 세팅
        Pistris_Meat newBunndle = gameObject.AddComponent<Pistris_Meat>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // 획득한 해당 드랍 아이템 반환
    public override DropItem ObtainDropItem()
    {
        return this;
    }

    // 해당 드랍 아이템 인벤토리에 추가
    public override void MoveItemToInventory()
    {
        // 해당 드랍 아이템 인벤토리에 1개 추가
        InventoryManager.instance.AddItems(this, 1);

        CancelInvoke("MoveItemToInventory");
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 땅에 닿은 후 1.5초 뒤 자동으로 인벤토리에 추가
        if (collision.gameObject.tag == "Ground")
        {
            //Debug.Log(pistris_Meat.ItemName + " 땅에 닿음");
            Invoke("MoveItemToInventory", 1.5f);
        }
    }
}