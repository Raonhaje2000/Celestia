using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public const int INVENTORY_SLOT_MAX_COUNT = 30;   // 인벤토리 아이템 슬롯 개수

    public static InventoryManager instance;

    List<InventoryItem> inventoryItems;               // 인벤토리 아이템

    GoodsItmeData gold;                               // 재화 - 골드
    GoodsItmeData feather;                            // 재화 - 부활의 깃털

    #region [Property]
    public List<InventoryItem> InventoryItems
    {
        get { return inventoryItems;}
    }

    public GoodsItmeData Feather
    {
        get { return feather; }
        set { feather = value; }
    }

    public GoodsItmeData Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 다음 씬으로 넘어가도 오브젝트 파괴되지 않고 유지
            // 게임 내에서 공통으로 쓰이는 데이터 유지
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // 이미 기존 씬에 해당 오브젝트가 남아있을 경우 제거
            Destroy(gameObject);
        }

        // 관련 리소스 불러오기
        LoadResources();

        // 초기화
        Initialize();
    }

    void Start()
    {
        // 초기화
        //Initialize();
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        gold = ScriptableObject.Instantiate(Resources.Load<GoodsItmeData>("GameData/Item/Goods/GoldData"));
        feather = ScriptableObject.Instantiate(Resources.Load<GoodsItmeData>("GameData/Item/Goods/FeatherData"));
    }

    // 초기화
    void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
    }

    // 아이템 획득 (인벤토리에 아이템 추가)
    public void AddItems(InventoryItem item, int count)
    {
        // 추가하려는 개수가 인벤토리에 추가하려는 개수보다 많은 경우 예외 발생 (공간이 없는데 더 추가하려는 것 방지)
        if (!HaveItemSlot(item, count)) throw new System.Exception("추가하려는 수가 인벤토리 내에 추가될 수 있는 개수보다 많음");
        if (count <= 0) throw new System.Exception("0개 이하는 추가 불가능");

        QuestManager.instance.CheckQuestItem(item, count);

        // 인벤토리에 보유한 동일 아이템의 인덱스를 가져옴
        List<int> indexList = FindItems(item);

        int maxCount = item.GetInventoryItemData().BundleMaxCount; // 해당 아이템의 묶음 최대 수량

        // 인벤토리에 동일 아이템을 보유하고 있는 경우
        if (indexList != null)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                int currentCount = inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount; // 해당 아이템의 해당 슬롯 보유 수량

                // 해당 슬롯의 아이템 개수가 묶음 수보다 적은 경우
                if (currentCount < maxCount)
                {
                    // 묶음 최대 수량까지 채움
                    int addPossibleCount = maxCount - currentCount;

                    if (count <= addPossibleCount)
                    {
                        // 채우려는 아이템 개수가 채울 수 있는 수량 이하인 경우
                        // 해당 개수만큼 아이템을 채우고 끝남
                        inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount += count;
                        return;
                    }
                    else
                    {
                        // 채우려는 아이템 개수가 채울 수 있는 수량보다 많은 경우
                        // 채울 수 있는 개수만큼 채우고 다음 슬롯으로 넘어감
                        inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount += addPossibleCount;
                        count -= addPossibleCount;
                    }
                }
            }
        }

        // 인벤토리에 동일 아이템을 보유하고 있지 않거나, 현재 존재하는 모든 슬롯에 채울 수 있는 만큼 다 넣고 남은 경우

        // 최대 보유 수량보다 추가하려는 수량이 더 많은 경우
        while (count > maxCount)
        {
            // 묶음 최대 수량 만큼 보유하고 있는 새로운 묶음 생성
            InventoryItem newBundleMax = item.CreateNewBunddle();

            newBundleMax.GetInventoryItemData().CurrentCount = newBundleMax.GetInventoryItemData().BundleMaxCount;

            inventoryItems.Add(newBundleMax);

            // 묶음 생성을 위해 새로 만든 컴포넌트 제거
            Destroy(newBundleMax.GetComponent<InventoryItem>());

            count -= maxCount;
        }

        // 남은 추가 수량 만큼 새로운 묶음 생성
        InventoryItem newBundle = item.CreateNewBunddle();
        newBundle.GetInventoryItemData().CurrentCount = count;

        inventoryItems.Add(newBundle);

        // 묶음 생성을 위해 새로 만든 컴포넌트 제거
        Destroy(newBundle.GetComponent<InventoryItem>());
    }

    // 아이템 삭제 (인벤토리에 아이템 제거)
    public void DeleteItems(InventoryItem item, int count)
    {
        // 제거하려는 개수가 현재 총 보유 개수보다 큰 경우 예외 발생 (인벤토리에 없는데 삭제하려는 오동작 방지)
        if (count > FindTotalItemCount(item)) throw new System.Exception("아이템이 존재하지 않거나 삭제하려는 수가 인벤토리 내 현재 보유 개수보다 많음");
        if (count <= 0) throw new System.Exception("0개 이하는 삭제 불가능");

        // 인벤토리에 보유한 동일 아이템의 인덱스를 가져옴
        List<int> indexList = FindItems(item);

        // 인벤토리 뒤에있는 아이템부터 제거
        for (int i = indexList.Count - 1; i >= 0; i--)
        {
            int currentCount = inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount; // 해당 아이템의 해당 슬롯 보유 수량

            if (count < currentCount)
            {
                // 제거하려는 개수가 현재 보유 개수보다 적은 경우
                // 해당 개수만큼 아이템을 제거하고 종료
                inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount -= count;
                return;
            }
            else if (count == currentCount)
            {
                // 제거하려는 개수가 현재 보유 개수와 같은 경우
                // 해당 슬롯에서 아이템 제거하고 종료
                inventoryItems.RemoveAt(indexList[i]);
                return;
            }
            else
            {
                // 제거하려는 개수가 현재 보유 개수보다 많은 경우
                // 제거할 수 있는 만큼 제거하고 해당 슬롯의 아이템을 제거한 뒤 다음 슬롯으로 넘어감
                count -= currentCount;
                inventoryItems.RemoveAt(indexList[i]);
            }
        }
    }

    // 인벤토리 내에서 아이템 찾기 (찾은 인덱스 List 반환)
    public List<int> FindItems(InventoryItem item)
    {
        List<int> itemIndexList = new List<int>(); // 찾은 아이템의 인덱스들

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // 인벤토리 내에서 같은 아이템을 찾은 경우 (아이템 ID가 동일 할 경우) 해당 인벤토리의 인덱스 저장
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId) itemIndexList.Add(i);
        }

        return itemIndexList;
    }

    // 인벤토리 내에서 아이템의 총 보유 개수 찾기
    public int FindTotalItemCount(InventoryItem item)
    {
        int total = 0;

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // 인벤토리 내에서 같은 아이템을 찾은 경우 (아이템 ID가 동일 할 경우) 기존 총합에 현재 개수 추가
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId)
                total += inventoryItems[i].GetInventoryItemData().CurrentCount;
        }

        return total;
    }

    // 인벤토리 내에서 여분의 슬롯이 있는지 확인 (슬롯 여부 확인)
    public bool HaveItemSlot(InventoryItem item, int addCount)
    {
        int maxBundleCount = item.GetInventoryItemData().BundleMaxCount; // 해당 아이템의 묶음 최대 개수
        int extraCount = 0;                                               // 인벤토리 내에서의 여분 개수 (더 들어갈 수 있는 개수)

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // 인벤토리 내에서 같은 아이템을 찾은 경우 (아이템 ID가 동일 할 경우) 기존 총합에 현재 개수 추가
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId)
            {
                extraCount += (maxBundleCount - inventoryItems[i].GetInventoryItemData().CurrentCount);

                // 여분 개수가 추가하려는 개수 이상인 경우
                if(extraCount >= addCount) return true;
            }
        }

        int itemNeedSlotCount = Mathf.CeilToInt((float)(addCount - extraCount) / (float)maxBundleCount); // 아이템을 추가하는데 필요한 여분 슬롯 개수
        int currentExtraSlotCount = INVENTORY_SLOT_MAX_COUNT - inventoryItems.Count;                     // 현재 남아있는 여분 슬롯 개수

        //Debug.Log("Need Slot: " + itemNeedSlotCount + " / Extra Slot: " + currentExtraSlotCount);

        // 남아있는 여분의 슬롯 개수가 아이템을 추가하는데 필요한 여분의 슬롯 개수 이상인 경우
        if (currentExtraSlotCount >= itemNeedSlotCount) return true;
        else return false;
    }


    // 인벤토리 내에서 아이템이 있는지 확인 (보유 여부 확인)
    public bool HaveItem(InventoryItem item)
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            // 하나라도 있는 경우
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId) return true;
        }

        // 인벤토리를 다 찾아봐도 아이템이 없는 경우
        return false;
    }

    // 인벤토리 내에서 현재 개수가 0이된 슬롯의 아이템을 찾은 후 인벤토리에서 제거
    public void FindAndDeleteSlotItem()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].GetInventoryItemData().CurrentCount == 0)
            {
                inventoryItems.RemoveAt(i);
                break;
            }
        }
    }
}