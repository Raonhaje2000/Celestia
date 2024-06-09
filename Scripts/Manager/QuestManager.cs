using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 퀘스트 관련 처리
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] List<QuestData> mainQuests; // 메인 퀘스트 목록
    [SerializeField] List<QuestData> subQuests;  // 서브 퀘스트 목록                                                           

    int completeMainIndex;                       // 완료한 메인 퀘스트 번호

    [SerializeField] SkillUiManager skillUiManager;

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
    }

    void Start()
    {
        // 초기화
        Initialize(); // 나중에 AddTest 지우고 풀기
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        QuestData[] questDataArray = Resources.LoadAll<QuestData>("GameData/Quest");

        // 퀘스트 타입에 따라 각 리스트에 추가
        for (int i = 0; i < questDataArray.Length; i++)
        {
            if (questDataArray[i].Type == QuestData.QuestType.main) mainQuests.Add(ScriptableObject.Instantiate(questDataArray[i]));
            else if (questDataArray[i].Type == QuestData.QuestType.sub) subQuests.Add(ScriptableObject.Instantiate(questDataArray[i]));
        }
    }

    // 초기화
    void Initialize()
    {
        //completeMainIndex = -1;
        completeMainIndex = -1;
    }

    // 다음에 진행 해야하는 메인 퀘스트 반환
    public QuestData GetNextProgressMainQuest()
    {
        if (completeMainIndex + 1 < mainQuests.Count) return mainQuests[completeMainIndex + 1];
        else return null;
    }

    // 진행 중인 메인 퀘스트 반환
    public QuestData GetProgressMainQuest()
    {
        // 완료한 메인 퀘스트의 다음 순서가 현재 진행 중인 경우에만 메인 퀘스트 반환
        QuestData progressMainQuest = null;

        if (completeMainIndex + 1 < mainQuests.Count && mainQuests[completeMainIndex + 1].IsProgress)
            progressMainQuest = mainQuests[completeMainIndex + 1];

        //if(progressMainQuest != null) Debug.Log(progressMainQuest.QuestTitle);

        return progressMainQuest;
    }

    // 진행 중인 서브 퀘스트들 반환
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

    // 해당 NPC와 관련된 메인 퀘스트 찾아서 반환
    public List<QuestData> GetNpcMainQuests(NpcData npcData)
    {
        List<QuestData> npcQuests = new List<QuestData>();

        // 해당 NPC와 관련된 메인 퀘스트 찾기
        for (int i = 0; i < mainQuests.Count; i++)
        {
            if (mainQuests[i].StartNpc != null && mainQuests[i].CompletionNpc != null && !mainQuests[i].IsFinish)
            {
                if (mainQuests[i].StartNpc.NpcId == npcData.NpcId || mainQuests[i].CompletionNpc.NpcId == npcData.NpcId)
                {
                    // 해당 NPC가 메인 퀘스트의 시작 NPC 또는 종료 NPC 일 때 목록에 추가
                    npcQuests.Add(mainQuests[i]);
                }
            }
        }

        return npcQuests;
    }

    // 해당 NPC와 관련된 서브 퀘스트 찾아서 반환
    public List<QuestData> GetNpcSubQuests(NpcData npcData)
    {
        List<QuestData> npcQuests = new List<QuestData>();
      
        for (int i = 0; i < subQuests.Count; i++)
        {
            if (subQuests[i].StartNpc != null && subQuests[i].CompletionNpc != null && !subQuests[i].IsFinish)
            {
                if (subQuests[i].StartNpc.NpcId == npcData.NpcId || subQuests[i].CompletionNpc.NpcId == npcData.NpcId)
                {
                    // 해당 NPC가 서브 퀘스트의 시작 NPC 또는 종료 NPC 일 때 목록에 추가
                    npcQuests.Add(subQuests[i]);
                }
            }
        }

        return npcQuests;
    }

    // 해당 아이템이 퀘스트 완료 조건 아이템인지 확인
    public void CheckQuestItem(InventoryItem item, int count)
    {
        // 진행중인 퀘스트 목록들을 가져옴
        QuestData progressMainQuest = GetProgressMainQuest();
        List<QuestData> progressSubQuests = GetProgressSubQuests();

        // 메인 퀘스트에서 완료 조건이 아이템 획득일 때만 확인
        if (progressMainQuest != null && progressMainQuest.Condition == QuestData.CompletionCondition.ObtainingItems)
        {
            if (((InventoryItemData)progressMainQuest.QuestObject).ItemId == (item.GetInventoryItemData().ItemId))
            {
                progressMainQuest.CurrentCount += count;
            }
        }

        // 서브 퀘스트에서 완료 조건이 아이템 획득일 때만 확인
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

    // 해당 몬스터가 퀘스트 완료 조건 몬스터인지 확인
    public void CheckQuestMonster(Monster monster)
    {
        // 진행중인 퀘스트 목록들을 가져옴
        QuestData progressMainQuest = GetProgressMainQuest();
        List<QuestData> progressSubQuests = GetProgressSubQuests();

        // 메인 퀘스트에서 완료 조건이 아이템 획득일 때만 확인
        if (progressMainQuest != null && progressMainQuest.Condition == QuestData.CompletionCondition.KillMonsters)
        {
            if (((MonsterData)progressMainQuest.QuestObject).MonsterId == (monster.GetMonsterData().MonsterId))
            {
                progressMainQuest.CurrentCount++;
            }
        }

        // 서브 퀘스트에서 완료 조건이 아이템 획득일 때만 확인
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

    // 퀘스트 보상 획득
    void ReceiveRewards(QuestData quest)
    {
        Debug.Log("보상 획득");

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
