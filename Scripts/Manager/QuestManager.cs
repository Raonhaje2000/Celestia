using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ����Ʈ ���� ó��
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] List<QuestData> mainQuests; // ���� ����Ʈ ���
    [SerializeField] List<QuestData> subQuests;  // ���� ����Ʈ ���                                                           

    int completeMainIndex;                       // �Ϸ��� ���� ����Ʈ ��ȣ

    [SerializeField] SkillUiManager skillUiManager;

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
    }

    void Start()
    {
        // �ʱ�ȭ
        Initialize(); // ���߿� AddTest ����� Ǯ��
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        QuestData[] questDataArray = Resources.LoadAll<QuestData>("GameData/Quest");

        // ����Ʈ Ÿ�Կ� ���� �� ����Ʈ�� �߰�
        for (int i = 0; i < questDataArray.Length; i++)
        {
            if (questDataArray[i].Type == QuestData.QuestType.main) mainQuests.Add(ScriptableObject.Instantiate(questDataArray[i]));
            else if (questDataArray[i].Type == QuestData.QuestType.sub) subQuests.Add(ScriptableObject.Instantiate(questDataArray[i]));
        }
    }

    // �ʱ�ȭ
    void Initialize()
    {
        //completeMainIndex = -1;
        completeMainIndex = -1;
    }

    // ������ ���� �ؾ��ϴ� ���� ����Ʈ ��ȯ
    public QuestData GetNextProgressMainQuest()
    {
        if (completeMainIndex + 1 < mainQuests.Count) return mainQuests[completeMainIndex + 1];
        else return null;
    }

    // ���� ���� ���� ����Ʈ ��ȯ
    public QuestData GetProgressMainQuest()
    {
        // �Ϸ��� ���� ����Ʈ�� ���� ������ ���� ���� ���� ��쿡�� ���� ����Ʈ ��ȯ
        QuestData progressMainQuest = null;

        if (completeMainIndex + 1 < mainQuests.Count && mainQuests[completeMainIndex + 1].IsProgress)
            progressMainQuest = mainQuests[completeMainIndex + 1];

        //if(progressMainQuest != null) Debug.Log(progressMainQuest.QuestTitle);

        return progressMainQuest;
    }

    // ���� ���� ���� ����Ʈ�� ��ȯ
    public List<QuestData> GetProgressSubQuests()
    {
        List<QuestData> progresssubQuests = new List<QuestData>();

        for (int i = 0; i < subQuests.Count; i++)
        {
            if (subQuests[i].IsProgress)
            {
                progresssubQuests.Add(subQuests[i]);
            }
        }

        return progresssubQuests;
    }

    // �ش� NPC�� ���õ� ���� ����Ʈ ã�Ƽ� ��ȯ
    public List<QuestData> GetNpcMainQuests(NpcData npcData)
    {
        List<QuestData> npcQuests = new List<QuestData>();

        // �ش� NPC�� ���õ� ���� ����Ʈ ã��
        for (int i = 0; i < mainQuests.Count; i++)
        {
            if (mainQuests[i].StartNpc != null && mainQuests[i].CompletionNpc != null && !mainQuests[i].IsFinish)
            {
                if (mainQuests[i].StartNpc.NpcId == npcData.NpcId || mainQuests[i].CompletionNpc.NpcId == npcData.NpcId)
                {
                    // �ش� NPC�� ���� ����Ʈ�� ���� NPC �Ǵ� ���� NPC �� �� ��Ͽ� �߰�
                    npcQuests.Add(mainQuests[i]);
                }
            }
        }

        return npcQuests;
    }

    // �ش� NPC�� ���õ� ���� ����Ʈ ã�Ƽ� ��ȯ
    public List<QuestData> GetNpcSubQuests(NpcData npcData)
    {
        List<QuestData> npcQuests = new List<QuestData>();
      
        for (int i = 0; i < subQuests.Count; i++)
        {
            if (subQuests[i].StartNpc != null && subQuests[i].CompletionNpc != null && !subQuests[i].IsFinish)
            {
                if (subQuests[i].StartNpc.NpcId == npcData.NpcId || subQuests[i].CompletionNpc.NpcId == npcData.NpcId)
                {
                    // �ش� NPC�� ���� ����Ʈ�� ���� NPC �Ǵ� ���� NPC �� �� ��Ͽ� �߰�
                    npcQuests.Add(subQuests[i]);
                }
            }
        }

        return npcQuests;
    }

    // �ش� �������� ����Ʈ �Ϸ� ���� ���������� Ȯ��
    public void CheckQuestItem(InventoryItem item, int count)
    {
        // �������� ����Ʈ ��ϵ��� ������
        QuestData progressMainQuest = GetProgressMainQuest();
        List<QuestData> progressSubQuests = GetProgressSubQuests();

        // ���� ����Ʈ���� �Ϸ� ������ ������ ȹ���� ���� Ȯ��
        if (progressMainQuest != null && progressMainQuest.Condition == QuestData.CompletionCondition.ObtainingItems)
        {
            if (((InventoryItemData)progressMainQuest.QuestObject).ItemId == (item.GetInventoryItemData().ItemId))
            {
                progressMainQuest.CurrentCount += count;
            }
        }

        // ���� ����Ʈ���� �Ϸ� ������ ������ ȹ���� ���� Ȯ��
        for (int i = 0; i < progressSubQuests.Count; i++)
        {
            if (progressSubQuests[i].Condition == QuestData.CompletionCondition.ObtainingItems)
            {
                if (((InventoryItemData)progressSubQuests[i].QuestObject).ItemId == (item.GetInventoryItemData().ItemId))
                {
                    progressSubQuests[i].CurrentCount += count;
                }
            }
        }
    }

    // �ش� ���Ͱ� ����Ʈ �Ϸ� ���� �������� Ȯ��
    public void CheckQuestMonster(Monster monster)
    {
        // �������� ����Ʈ ��ϵ��� ������
        QuestData progressMainQuest = GetProgressMainQuest();
        List<QuestData> progressSubQuests = GetProgressSubQuests();

        // ���� ����Ʈ���� �Ϸ� ������ ������ ȹ���� ���� Ȯ��
        if (progressMainQuest != null && progressMainQuest.Condition == QuestData.CompletionCondition.KillMonsters)
        {
            if (((MonsterData)progressMainQuest.QuestObject).MonsterId == (monster.GetMonsterData().MonsterId))
            {
                progressMainQuest.CurrentCount++;
            }
        }

        // ���� ����Ʈ���� �Ϸ� ������ ������ ȹ���� ���� Ȯ��
        for (int i = 0; i < progressSubQuests.Count; i++)
        {
            if (progressSubQuests[i] != null && progressSubQuests[i].Condition == QuestData.CompletionCondition.KillMonsters)
            {
                if (((MonsterData)progressSubQuests[i].QuestObject).MonsterId == (monster.GetMonsterData().MonsterId))
                {
                    progressSubQuests[i].CurrentCount++;
                }
            }
        }
    }

    public void FinishMainQuest(QuestData quest)
    {
        quest.IsFinish = true;
        completeMainIndex++;

        ReceiveRewards(quest);
    }

    public void FinishSubQuest(QuestData quest)
    {
        quest.IsFinish = true;

        ReceiveRewards(quest);
    }

    // ����Ʈ ���� ȹ��
    void ReceiveRewards(QuestData quest)
    {
        Debug.Log("���� ȹ��");

        if (quest.RewardExp > 0) PlayerManager.instance.GainExp(quest.RewardExp);
        if (quest.RewardGold > 0) InventoryManager.instance.Gold.CurrentAmount += quest.RewardGold;
        if (quest.RewardFeather > 0) InventoryManager.instance.Feather.CurrentAmount += quest.RewardFeather;

        for (int i = 0; i < quest.RewardItems.Length; i++)
        {
            if (quest.RewardItems[i] != null)
            {
                //InventoryItem questItem = Instantiate(quest.RewardItems[i]);
                //InventoryManager.instance.AddItems(questItem, quest.RewardItemsCount[i]);

                // --------------------------------------------------------------------------

                InventoryItem questItem = Instantiate(quest.RewardItems[i]);
                questItem.SetInventoryItemData();
                questItem.transform.parent = InventoryManager.instance.transform;

                InventoryManager.instance.AddItems(questItem, quest.RewardItemsCount[i]);
                // --------------------------------------------------------------------------
            }

            if (quest.LiftedSkill != null)
            {
                PlayerManager.instance.FindSkillData(quest.LiftedSkill).IsAcquisition = true;
                skillUiManager.UpdateSkillList();
                InGame_Manager.instance.UpdateLockIcon();
            }
        }
    }
}
