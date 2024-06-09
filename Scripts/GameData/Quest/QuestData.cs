using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "GameData/Quest/Quest")]
public class QuestData : ScriptableObject
{
    public const int REWARD_ITEM_MAX_COUNT = 2;

    public enum QuestType { main = 0, sub = 1 }

    public enum CompletionCondition
    { 
        AreaArrival = 0, // 지역 도착
        ObtainingItems,  // 아이템 획득
        KillMonsters     // 몬스터 처치
    }

    public enum QuestProgressArea
    {
        Village = 0,      // 마을
        SacredTreeForest, // 신수 숲
        TreesForest,      // 나무 숲
        ValleyForest,     // 계곡 숲
        CaveForest        // 동굴 숲
    }

    [SerializeField] QuestType type;                                // 퀘스트 타입
    [SerializeField] CompletionCondition condition;                 // 퀘스트 완료 조건

    [SerializeField] int questId;                                   // 퀘스트 ID
    [SerializeField] string questTitle;                             // 퀘스트 제목

    [Multiline(6)] [SerializeField] string startContent;            // 시작 내용
    [Multiline(6)] [SerializeField] string completionContent;       // 완료 내용

    [SerializeField] string summary;                                // 퀘스트 요약 (퀘스트 리스트에 간단히 뜰 것)                    

    [SerializeField] QuestProgressArea progressArea;                // 퀘스트 수행 지역
    string progressAreaString;                                      // 퀘스트 수행 지역(문자열)

    [SerializeField] ScriptableObject questObject;                  // 퀘스트 관련 오브젝트

    [SerializeField] int completionCount = -1;                      // 완료 아이템 수, 몬스터 처치 수
    int currentCount;                                               // 현재 아이템 수, 몬스터 처치 수

    [SerializeField] NpcData startNpc;                              // 시작 NPC ID
    [SerializeField] Transform startNpcPosition;
    [SerializeField] NpcData completionNpc;                         // 완료 NPC ID
    [SerializeField] Transform completionNpcPosition;

    bool isProgress;                                                // 진행 여부 (퀘스트를 진행 중인지)
    bool isConditionComplete;                                       // 조건 완료 여부 (조건을 달성 했는지)
    bool isFinish;                                                  // 완료 여부 (퀘스트를 완료 했는지)

    [Min(0)] [SerializeField] int rewardExp;                        // 보상 경험치
    [Min(0)] [SerializeField] int rewardGold;                       // 보상 골드
    [Min(0)] [SerializeField] int rewardFeather;                    // 보상 부활의 깃털

    [SerializeField] InventoryItem[] rewardItems = new InventoryItem[REWARD_ITEM_MAX_COUNT]; // 보상 아이템
    [SerializeField] int[] rewardItemsCount = new int[REWARD_ITEM_MAX_COUNT];                // 보상 아이템 개수

    [SerializeField] PlayerSkillData liftedSkill;                                            // 해금되는 스킬

    public QuestType Type
    {
        get { return type; }
    }

    public CompletionCondition Condition
    {
        get { return condition; }
    }

    public int QuestID
    {
        get { return questId; }
    }

    public string QuestTitle
    {
        get { return questTitle; }
    }

    public string StartContent
    {
        get { return startContent; }
    }

    public string CompletionContent
    {
        get { return completionContent; }
    }

    public string Summary
    {
        get { return summary; }
    }

    public QuestProgressArea ProgressArea
    {
        get { return progressArea; }
    }

    public string ProgressAreaString
    {
        get
        {
            if (progressAreaString != "") progressAreaString = ProgressAreaToString();

            return progressAreaString; 
        }
    }

    public ScriptableObject QuestObject
    {
        get
        {
            // 퀘스트 완료 조건이 지역 도착이 아닌 경우에만 퀘스트 관련 오브젝트 반환
            questObject = (condition != CompletionCondition.AreaArrival) ? questObject : null;

            return questObject;
        }
    }

    public int CompletionCount
    { 
        get
        {
            // 완료 조건이 지역 도착이 아닌 경우(아이템 획득 또는 몬스터 처치인 경우)에만 개수 반환
            completionCount = (condition != CompletionCondition.AreaArrival) ? completionCount : -1;
            return completionCount;
        }
    }

    public int CurrentCount
    {
        get { return currentCount; }
        set
        {
            currentCount = value;

            if (currentCount >= completionCount)
            {
                currentCount = completionCount;
                isConditionComplete = true;
            }
        }
    }

    public NpcData StartNpc
    {
        get { return startNpc; }
    }

    public Transform StartNpcPosition
    {
        get { return startNpcPosition; }
    }

    public NpcData CompletionNpc
    {
        get { return completionNpc; }
    }

    public Transform CompletionNpcPosition
    {
        get { return completionNpcPosition; }
    }

    public bool IsProgress
    {
        get { return isProgress; }
        set { isProgress = value; }
    }

    public bool IsConditionComplete
    {
        get { return isConditionComplete; }
        set { isConditionComplete = value; }
    }

    public bool IsFinish
    {
        get { return isFinish; }
        set { isFinish = value; }
    }

    public int RewardExp
    {
        get { return rewardExp; }
    }

    public int RewardGold
    {
        get { return rewardGold; }
    }

    public int RewardFeather
    {
        get { return rewardFeather; }
    }

    public InventoryItem[] RewardItems
    {
        get { return rewardItems; }
    }

    public int[] RewardItemsCount
    {
        get { return rewardItemsCount; }
    }

    public PlayerSkillData LiftedSkill
    { 
        get { return liftedSkill; }
    }

    // 퀘스트 수행 지역을 문자열로 반환
    string ProgressAreaToString()
    {
        switch (progressArea)
        {
            case QuestProgressArea.Village:          return "마을";
            case QuestProgressArea.SacredTreeForest: return "신수 숲";
            case QuestProgressArea.TreesForest:      return "나무 숲";
            case QuestProgressArea.ValleyForest:     return "계곡 숲";
            case QuestProgressArea.CaveForest:       return "동굴 숲";
            default:                                 return "";
        }
    }
}
