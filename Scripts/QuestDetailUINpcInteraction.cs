using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDetailUINpcInteraction : MonoBehaviour
{
    QuestDetailUI questDetailUI;

    [SerializeField] TextMeshProUGUI questNpcUIHeaderText;

    [SerializeField] Button questDetailUIButtonYes;
    [SerializeField] TextMeshProUGUI questDetailUIButtonYesText;

    [SerializeField] Button questDetailUIButtonNo;
    [SerializeField] TextMeshProUGUI questDetailUIButtonNoText;

    QuestData npcQuestData;
    bool isCompletion;

    Npc currentNpc;

    private void Awake()
    {
        questDetailUI = GetComponent<QuestDetailUI>();

        Transform[] children = this.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "QuestNpcUIHeaderText") questNpcUIHeaderText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestDetailUIButtonYes") questDetailUIButtonYes = children[i].gameObject.GetComponent<Button>();
            if (children[i].name == "QuestDetailUIButtonNo") questDetailUIButtonNo = children[i].gameObject.GetComponent<Button>();
        }

        questDetailUIButtonYes.onClick.AddListener(ClickButtonYes);
        questDetailUIButtonYesText = questDetailUIButtonYes.transform.Find("QuestDetailUIButtonYesText").gameObject.GetComponent<TextMeshProUGUI>();

        questDetailUIButtonNo.onClick.AddListener(ClickButtonNo);
        questDetailUIButtonNoText = questDetailUIButtonNo.transform.Find("QuestDetailUIButtonNoText").gameObject.GetComponent<TextMeshProUGUI>();

    }

    public void SetQusetNpcUI(Npc npc, QuestData quest, bool isQuestCompletion)
    {
        currentNpc = npc;
        npcQuestData = quest;
        isCompletion = isQuestCompletion;

        questNpcUIHeaderText.text = (!isQuestCompletion) ? "퀘스트 수락" : "퀘스트 완료";

        questDetailUIButtonYesText.text = (!isQuestCompletion) ? "퀘스트 수락" : "퀘스트 완료";
        questDetailUIButtonNoText.text = (!isQuestCompletion) ? "퀘스트 거절" : "퀘스트 보류";

        questDetailUI.SetQuestDetail(quest, isQuestCompletion);
    }

    public void SetQusetNpcUIActive(bool active)
    {
        gameObject.SetActive(active);
    }

    void ClickButtonYes()
    {
        if (!isCompletion)
        {
            // 퀘스트 시작인 경우
            npcQuestData.IsProgress = true;
        }
        else
        {
            // 퀘스트 완료인 경우
            if(npcQuestData.Type == QuestData.QuestType.main) QuestManager.instance.FinishMainQuest(npcQuestData);
            else QuestManager.instance.FinishSubQuest(npcQuestData);
        }

        currentNpc.GetComponent<Npc>().RemoveCurrentNpcQuest();

        GameManager.instance.IsPlayerInteractionWithNpc = false;

        gameObject.SetActive(false);
    }

    void ClickButtonNo()
    {
        GameManager.instance.IsPlayerInteractionWithNpc = false;
        gameObject.SetActive(false);
    }
}
