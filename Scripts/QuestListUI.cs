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

    // Quest List UI 관련
    GameObject mainQuestSlot;
    GameObject subQuestSlot;

    GameObject subQuestSlotPosition;

    [SerializeField] List<QuestListUISlotEvent> subQuestSlots;

    Button giveUpButton;
    Button xButton;

    // 현재 클릭된 슬롯 정보
    QuestData currentQuest;
    int currentSlotIndex;

    private void Awake()
    {
        if (instance == null) instance = this;

        // 관련 리소스 불러오기
        LoadResources();

        //// 진행 중인 퀘스트 목록 불러오기
        //LoadProgressQuests();
    }

    void Start()
    {
        // 진행 중인 퀘스트 목록 불러오기
        LoadProgressQuests();

        // 초기화
        Initialize();
        SetQuestSlots();
    }

    // 관련 리소스 불러오기
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

    // 진행 중인 퀘스트 목록 불러오기
    void LoadProgressQuests()
    {
        mainQuest = QuestManager.instance.GetProgressMainQuest();
        subQuests = QuestManager.instance.GetProgressSubQuests();
    }

    // 초기화
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

        // 메인 퀘스트 세팅
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

        // 서브 퀘스트 세팅
        for (int i = 0; i < subQuests.Count; i++)
        {
            // 서브 퀘스트 슬롯이 모자랄 경우 슬롯 생성
            if (subQuestSlots.Count - 1 < i)
            {
                GameObject slot = Instantiate(subQuestSlot);
                slot.transform.SetParent(subQuestSlotPosition.transform, false);

                subQuestSlots.Add(slot.GetComponent<QuestListUISlotEvent>());
            }

            subQuestSlots[i].SetSlot(subQuests[i], i);
            subQuestSlots[i].gameObject.SetActive(true);
        }

        // 진행 중인 서브 퀘스트 슬롯이 남아있는 경우 비활성화
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

            Debug.Log("퀘스트 포기");
        }
    }
}
