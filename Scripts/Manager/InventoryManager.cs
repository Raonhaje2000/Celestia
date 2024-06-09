using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public const int INVENTORY_SLOT_MAX_COUNT = 30;   // �κ��丮 ������ ���� ����

    public static InventoryManager instance;

    List<InventoryItem> inventoryItems;               // �κ��丮 ������

    GoodsItmeData gold;                               // ��ȭ - ���
    GoodsItmeData feather;                            // ��ȭ - ��Ȱ�� ����

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

            // ���� ������ �Ѿ�� ������Ʈ �ı����� �ʰ� ����
            // ���� ������ �������� ���̴� ������ ����
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // �̹� ���� ���� �ش� ������Ʈ�� �������� ��� ����
            Destroy(gameObject);
        }

        // ���� ���ҽ� �ҷ�����
        LoadResources();

        // �ʱ�ȭ
        Initialize();
    }

    void Start()
    {
        // �ʱ�ȭ
        //Initialize();
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        gold = ScriptableObject.Instantiate(Resources.Load<GoodsItmeData>("GameData/Item/Goods/GoldData"));
        feather = ScriptableObject.Instantiate(Resources.Load<GoodsItmeData>("GameData/Item/Goods/FeatherData"));
    }

    // �ʱ�ȭ
    void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
    }

    // ������ ȹ�� (�κ��丮�� ������ �߰�)
    public void AddItems(InventoryItem item, int count)
    {
        // �߰��Ϸ��� ������ �κ��丮�� �߰��Ϸ��� �������� ���� ��� ���� �߻� (������ ���µ� �� �߰��Ϸ��� �� ����)
        if (!HaveItemSlot(item, count)) throw new System.Exception("�߰��Ϸ��� ���� �κ��丮 ���� �߰��� �� �ִ� �������� ����");
        if (count <= 0) throw new System.Exception("0�� ���ϴ� �߰� �Ұ���");

        QuestManager.instance.CheckQuestItem(item, count);

        // �κ��丮�� ������ ���� �������� �ε����� ������
        List<int> indexList = FindItems(item);

        int maxCount = item.GetInventoryItemData().BundleMaxCount; // �ش� �������� ���� �ִ� ����

        // �κ��丮�� ���� �������� �����ϰ� �ִ� ���
        if (indexList != null)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                int currentCount = inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount; // �ش� �������� �ش� ���� ���� ����

                // �ش� ������ ������ ������ ���� ������ ���� ���
                if (currentCount < maxCount)
                {
                    // ���� �ִ� �������� ä��
                    int addPossibleCount = maxCount - currentCount;

                    if (count <= addPossibleCount)
                    {
                        // ä����� ������ ������ ä�� �� �ִ� ���� ������ ���
                        // �ش� ������ŭ �������� ä��� ����
                        inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount += count;
                        return;
                    }
                    else
                    {
                        // ä����� ������ ������ ä�� �� �ִ� �������� ���� ���
                        // ä�� �� �ִ� ������ŭ ä��� ���� �������� �Ѿ
                        inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount += addPossibleCount;
                        count -= addPossibleCount;
                    }
                }
            }
        }

        // �κ��丮�� ���� �������� �����ϰ� ���� �ʰų�, ���� �����ϴ� ��� ���Կ� ä�� �� �ִ� ��ŭ �� �ְ� ���� ���

        // �ִ� ���� �������� �߰��Ϸ��� ������ �� ���� ���
        while (count > maxCount)
        {
            // ���� �ִ� ���� ��ŭ �����ϰ� �ִ� ���ο� ���� ����
            InventoryItem newBundleMax = item.CreateNewBunddle();

            newBundleMax.GetInventoryItemData().CurrentCount = newBundleMax.GetInventoryItemData().BundleMaxCount;

            inventoryItems.Add(newBundleMax);

            // ���� ������ ���� ���� ���� ������Ʈ ����
            Destroy(newBundleMax.GetComponent<InventoryItem>());

            count -= maxCount;
        }

        // ���� �߰� ���� ��ŭ ���ο� ���� ����
        InventoryItem newBundle = item.CreateNewBunddle();
        newBundle.GetInventoryItemData().CurrentCount = count;

        inventoryItems.Add(newBundle);

        // ���� ������ ���� ���� ���� ������Ʈ ����
        Destroy(newBundle.GetComponent<InventoryItem>());
    }

    // ������ ���� (�κ��丮�� ������ ����)
    public void DeleteItems(InventoryItem item, int count)
    {
        // �����Ϸ��� ������ ���� �� ���� �������� ū ��� ���� �߻� (�κ��丮�� ���µ� �����Ϸ��� ������ ����)
        if (count > FindTotalItemCount(item)) throw new System.Exception("�������� �������� �ʰų� �����Ϸ��� ���� �κ��丮 �� ���� ���� �������� ����");
        if (count <= 0) throw new System.Exception("0�� ���ϴ� ���� �Ұ���");

        // �κ��丮�� ������ ���� �������� �ε����� ������
        List<int> indexList = FindItems(item);

        // �κ��丮 �ڿ��ִ� �����ۺ��� ����
        for (int i = indexList.Count - 1; i >= 0; i--)
        {
            int currentCount = inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount; // �ش� �������� �ش� ���� ���� ����

            if (count < currentCount)
            {
                // �����Ϸ��� ������ ���� ���� �������� ���� ���
                // �ش� ������ŭ �������� �����ϰ� ����
                inventoryItems[indexList[i]].GetInventoryItemData().CurrentCount -= count;
                return;
            }
            else if (count == currentCount)
            {
                // �����Ϸ��� ������ ���� ���� ������ ���� ���
                // �ش� ���Կ��� ������ �����ϰ� ����
                inventoryItems.RemoveAt(indexList[i]);
                return;
            }
            else
            {
                // �����Ϸ��� ������ ���� ���� �������� ���� ���
                // ������ �� �ִ� ��ŭ �����ϰ� �ش� ������ �������� ������ �� ���� �������� �Ѿ
                count -= currentCount;
                inventoryItems.RemoveAt(indexList[i]);
            }
        }
    }

    // �κ��丮 ������ ������ ã�� (ã�� �ε��� List ��ȯ)
    public List<int> FindItems(InventoryItem item)
    {
        List<int> itemIndexList = new List<int>(); // ã�� �������� �ε�����

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // �κ��丮 ������ ���� �������� ã�� ��� (������ ID�� ���� �� ���) �ش� �κ��丮�� �ε��� ����
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId) itemIndexList.Add(i);
        }

        return itemIndexList;
    }

    // �κ��丮 ������ �������� �� ���� ���� ã��
    public int FindTotalItemCount(InventoryItem item)
    {
        int total = 0;

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // �κ��丮 ������ ���� �������� ã�� ��� (������ ID�� ���� �� ���) ���� ���տ� ���� ���� �߰�
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId)
                total += inventoryItems[i].GetInventoryItemData().CurrentCount;
        }

        return total;
    }

    // �κ��丮 ������ ������ ������ �ִ��� Ȯ�� (���� ���� Ȯ��)
    public bool HaveItemSlot(InventoryItem item, int addCount)
    {
        int maxBundleCount = item.GetInventoryItemData().BundleMaxCount; // �ش� �������� ���� �ִ� ����
        int extraCount = 0;                                               // �κ��丮 �������� ���� ���� (�� �� �� �ִ� ����)

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // �κ��丮 ������ ���� �������� ã�� ��� (������ ID�� ���� �� ���) ���� ���տ� ���� ���� �߰�
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId)
            {
                extraCount += (maxBundleCount - inventoryItems[i].GetInventoryItemData().CurrentCount);

                // ���� ������ �߰��Ϸ��� ���� �̻��� ���
                if(extraCount >= addCount) return true;
            }
        }

        int itemNeedSlotCount = Mathf.CeilToInt((float)(addCount - extraCount) / (float)maxBundleCount); // �������� �߰��ϴµ� �ʿ��� ���� ���� ����
        int currentExtraSlotCount = INVENTORY_SLOT_MAX_COUNT - inventoryItems.Count;                     // ���� �����ִ� ���� ���� ����

        //Debug.Log("Need Slot: " + itemNeedSlotCount + " / Extra Slot: " + currentExtraSlotCount);

        // �����ִ� ������ ���� ������ �������� �߰��ϴµ� �ʿ��� ������ ���� ���� �̻��� ���
        if (currentExtraSlotCount >= itemNeedSlotCount) return true;
        else return false;
    }


    // �κ��丮 ������ �������� �ִ��� Ȯ�� (���� ���� Ȯ��)
    public bool HaveItem(InventoryItem item)
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            // �ϳ��� �ִ� ���
            if (item.GetInventoryItemData().ItemId == inventoryItems[i].GetInventoryItemData().ItemId) return true;
        }

        // �κ��丮�� �� ã�ƺ��� �������� ���� ���
        return false;
    }

    // �κ��丮 ������ ���� ������ 0�̵� ������ �������� ã�� �� �κ��丮���� ����
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