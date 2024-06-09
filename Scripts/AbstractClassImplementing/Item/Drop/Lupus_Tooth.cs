using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Lupus_Tooth : DropItem
{
    DropItemData lupus_Tooth; // 루푸스의 이빨 아이템 데이터
    [SerializeField]
    GameObject text;          // 아이템의 이름 텍스트

    private void Awake()
    {
        // 해당 인벤토리 아이템의 데이터 세팅
        SetInventoryItemData();
    }

    private void Start()
    {
        // 해당 아이템의 이름 텍스트를 불러온 뒤 비활성화
        text = transform.Find("ItemText").gameObject;
        text.SetActive(false);

        SetInit();        // 드랍 아이템 초기화
        BounceDropItem(); // 생성과 동시에 드랍 아이템 튀어오르기
    }

    // 해당 인벤토리 아이템의 데이터 세팅
    public override void SetInventoryItemData()
    {
        lupus_Tooth = ScriptableObject.Instantiate(Resources.Load<DropItemData>("GameData/Item/Drop/Field_03/Lupus_ToothData"));
    }

    // 해당 인벤토리 아이템의 데이터 반환
    public override InventoryItemData GetInventoryItemData()
    {
        return lupus_Tooth;
    }

    // 해당 인벤토리 아이템의 새로운 묶음(컴포넌트) 생성 후 반환
    public override InventoryItem CreateNewBunddle()
    {
        // 묶음 생성 후 아이템 데이터 세팅
        Lupus_Tooth newBunndle = gameObject.AddComponent<Lupus_Tooth>();

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
        InventoryManager.instance.AddItems(this, 1);

        CancelInvoke("MoveItemToInventory");
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 땅에 닿은 후 1.5초 뒤 자동으로 인벤토리에 추가
        if (collision.gameObject.tag == "Ground")
        {
            // 해당 아이템의 이름 텍스트를 활성화 및 해당 아이템의 이름으로 텍스트 변경
            text.SetActive(true);
            text.GetComponent<TextMeshPro>().text = lupus_Tooth.ItemName;

            // 메인 카메라에 맞춰 아이템의 이름 텍스트 회전
            transform.rotation = Camera.main.transform.rotation;

            //Debug.Log(lupus_Tooth.ItemName + " 땅에 닿음");
            
            Invoke("MoveItemToInventory", 1.5f);
        }
    }
}