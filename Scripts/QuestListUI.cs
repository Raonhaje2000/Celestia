using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestListUI : MonoBehaviour
{
    public static QuestListUI instance;

    [SerializeField] QuestData mainQuest;
    [SerializeField] List<QuestData> subQuests;

    [SerializeField] QuestDetailUI questDetailUI;

    // Quest List UI ����
    GameObject mainQuestSlot;
    GameObject subQuestSlot;

    GameObject subQuestSlotPosition;

    [SerializeField] List<QuestListUISlotEvent> subQuestSlots;

    Button giveUpButton;
    Button xButton;

    // ���� Ŭ���� ���� ����
    QuestData currentQuest;
    int currentSlotIndex;

    private void Awake()
    {
        if (instance == null) instance = this;

        // ���� ���ҽ� �ҷ�����
        LoadResources();

        //// ���� ���� ����Ʈ ��� �ҷ�����
        //LoadProgressQuests();
    }

    void Start()
    {
        // ���� ���� ����Ʈ ��� �ҷ�����
        LoadProgressQuests();

        // �ʱ�ȭ
        Initialize();
        SetQuestSlots();
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        questDetailUI = GetComponent<QuestDetailUI>();

        mainQuestSlot = GameObject.Find("MainQuestSlot");
        subQuestSlot = GameObject.Find("SubQuestSlot");

        subQuestSlotPosition = GameObject.Find("ProgressQuests");

        subQuestSlots.Add(subQuestSlot.GetComponent<QuestListUISlotEvent>());

        giveUpButton = GameObject.Find("QuestDetailUIButtonNo").GetComponent<Button>();
        giveUpButton.onClick.AddListener(ClickGiveUpButton);

        xButton = GameObject.Find("QuestListUIButtonX").GetComponent<Button>();
        xButton.onClick.AddListener(delegate { gameObject.SetActive(false); } );
    }

    // ���� ���� ����Ʈ ��� �ҷ�����
    void LoadProgressQuests()
    {
        mainQuest = QuestManager.instance.GetProgressMainQuest();
        subQuests = QuestManager.instance.GetProgressSubQuests();
    }

    // �ʱ�ȭ
    void Initialize()
    {
        currentQuest = null;
        currentSlotIndex = -1;

        questDetailUI.ActiveQuestDetail(false);
    }

    public void SetQuestSlots()
    {
        LoadProgressQuests();
        questDetailUI.ActiveQuestDetail(false);

        // ���� ����Ʈ ����
        if (mainQuest != null)
        {
            mainQuestSlot.GetComponent<QuestListUISlotEvent>().SetSlot(mainQuest, 0);
            mainQuestSlot.gameObject.SetActive(true);
        }
        else
        {
            mainQuestSlot.GetComponent<QuestListUISlotEvent>().SetSlot(null, -1);
            mainQuestSlot.gameObject.SetActive(false);
        }

        // ���� ����Ʈ ����
        for (int i = 0; i < subQuests.Count; i++)
        {
            // ���� ����Ʈ ������ ���ڶ� ��� ���� ����
            if (subQuestSlots.Count - 1 < i)
            {
                GameObject slot = Instantiate(subQuestSlot);
                slot.transform.SetParent(subQuestSlotPosition.transform, false);

                subQuestSlots.Add(slot.GetComponent<QuestListUISlotEvent>());
            }

            subQuestSlots[i].SetSlot(subQuests[i], i);
            subQuestSlots[i].gameObject.SetActive(true);
        }

        // ���� ���� ���� ����Ʈ ������ �����ִ� ��� ��Ȱ��ȭ
        for(int i = subQuests.Count; i < subQuestSlots.Count; i++)
        {
            subQuestSlots[i].SetSlot(null, -1);
            subQuestSlots[i].gameObject.SetActive(false);
        }
    }

    public void SetQuestDetailBySlot(QuestData quest, int index)
    {
        currentQuest = quest;
        currentSlotIndex = index;

        questDetailUI.ActiveQuestDetail(true);
        questDetailUI.SetQuestDetail(quest, false);
    }

    void ClickGiveUpButton()
    {
        if (currentSlotIndex != -1)
        {
            if (currentQuest.Type == QuestData.QuestType.main)
            {
                mainQuest.IsProgress = false;
                mainQuest.IsConditionComplete = false;

                if(currentQuest.Condition != QuestData.CompletionCondition.AreaArrival) mainQuest.CurrentCount = 0;
            }
            else
            {
                subQuests[currentSlotIndex].IsProgress = false;
                subQuests[currentSlotIndex].IsConditionComplete = false;

                if (currentQuest.Condition != QuestData.CompletionCondition.AreaArrival) subQuests[currentSlotIndex].CurrentCount = 0;
            }

            currentQuest = null;
            currentSlotIndex = -1;

            SetQuestSlots();
            questDetailUI.ActiveQuestDetail(false);

            Debug.Log("����Ʈ ����");
        }
    }
}
