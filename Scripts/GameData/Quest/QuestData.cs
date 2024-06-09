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
        AreaArrival = 0, // ���� ����
        ObtainingItems,  // ������ ȹ��
        KillMonsters     // ���� óġ
    }

    public enum QuestProgressArea
    {
        Village = 0,      // ����
        SacredTreeForest, // �ż� ��
        TreesForest,      // ���� ��
        ValleyForest,     // ��� ��
        CaveForest        // ���� ��
    }

    [SerializeField] QuestType type;                                // ����Ʈ Ÿ��
    [SerializeField] CompletionCondition condition;                 // ����Ʈ �Ϸ� ����

    [SerializeField] int questId;                                   // ����Ʈ ID
    [SerializeField] string questTitle;                             // ����Ʈ ����

    [Multiline(6)] [SerializeField] string startContent;            // ���� ����
    [Multiline(6)] [SerializeField] string completionContent;       // �Ϸ� ����

    [SerializeField] string summary;                                // ����Ʈ ��� (����Ʈ ����Ʈ�� ������ �� ��)                    

    [SerializeField] QuestProgressArea progressArea;                // ����Ʈ ���� ����
    string progressAreaString;                                      // ����Ʈ ���� ����(���ڿ�)

    [SerializeField] ScriptableObject questObject;                  // ����Ʈ ���� ������Ʈ

    [SerializeField] int completionCount = -1;                      // �Ϸ� ������ ��, ���� óġ ��
    int currentCount;                                               // ���� ������ ��, ���� óġ ��

    [SerializeField] NpcData startNpc;                              // ���� NPC ID
    [SerializeField] Transform startNpcPosition;
    [SerializeField] NpcData completionNpc;                         // �Ϸ� NPC ID
    [SerializeField] Transform completionNpcPosition;

    bool isProgress;                                                // ���� ���� (����Ʈ�� ���� ������)
    bool isConditionComplete;                                       // ���� �Ϸ� ���� (������ �޼� �ߴ���)
    bool isFinish;                                                  // �Ϸ� ���� (����Ʈ�� �Ϸ� �ߴ���)

    [Min(0)] [SerializeField] int rewardExp;                        // ���� ����ġ
    [Min(0)] [SerializeField] int rewardGold;                       // ���� ���
    [Min(0)] [SerializeField] int rewardFeather;                    // ���� ��Ȱ�� ����

    [SerializeField] InventoryItem[] rewardItems = new InventoryItem[REWARD_ITEM_MAX_COUNT]; // ���� ������
    [SerializeField] int[] rewardItemsCount = new int[REWARD_ITEM_MAX_COUNT];                // ���� ������ ����

    [SerializeField] PlayerSkillData liftedSkill;                                            // �رݵǴ� ��ų

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
            // ����Ʈ �Ϸ� ������ ���� ������ �ƴ� ��쿡�� ����Ʈ ���� ������Ʈ ��ȯ
            questObject = (condition != CompletionCondition.AreaArrival) ? questObject : null;

            return questObject;
        }
    }

    public int CompletionCount
    { 
        get
        {
            // �Ϸ� ������ ���� ������ �ƴ� ���(������ ȹ�� �Ǵ� ���� óġ�� ���)���� ���� ��ȯ
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

    // ����Ʈ ���� ������ ���ڿ��� ��ȯ
    string ProgressAreaToString()
    {
        switch (progressArea)
        {
            case QuestProgressArea.Village:          return "����";
            case QuestProgressArea.SacredTreeForest: return "�ż� ��";
            case QuestProgressArea.TreesForest:      return "���� ��";
            case QuestProgressArea.ValleyForest:     return "��� ��";
            case QuestProgressArea.CaveForest:       return "���� ��";
            default:                                 return "";
        }
    }
}
