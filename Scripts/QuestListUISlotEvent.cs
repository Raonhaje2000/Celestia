using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestListUISlotEvent : MonoBehaviour
{
    QuestData questData;
    int slotIndex;

    Button button;
    TextMeshProUGUI titleText;

    private void Awake()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(delegate { QuestListUI.instance.SetQuestDetailBySlot(questData, slotIndex); }); // 버튼 이벤트 등록

        titleText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void SetSlot(QuestData quest, int index)
    {
        questData = quest;
        slotIndex = index;

        titleText.text = (slotIndex != -1) ? questData.QuestTitle : "";
    }

    public void SetSlot(int index)
    {
        slotIndex = index;

        titleText.text = "";
    }
}
