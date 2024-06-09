using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saffiano : Npc
{
    private void Awake()
    {
        SetNpcData();
        LoadResources();
    }

    void Start()
    {
        LoadUI();
        GetNpcQuests(npcData);
        SetNpcNameTag(npcData);

        npcNameTag.transform.localScale = new Vector3(3, 3, 3);
        npcQuestMark.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        npcQuestMinimapMark.transform.localPosition += new Vector3(0, 5, 0);
        npcQuestMinimapMark.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    void Update()
    {
        RotateNameTag();
        UpdateCurrentUiQuest();
    }

    protected override void SetNpcData()
    {
        npcData = ScriptableObject.Instantiate(Resources.Load<NpcData>("GameData/Npc/Sub/Saffiano"));                      
    }

    public override void InteroperateWithPlayer()
    {
        // 현재 NPC와 관련된 퀘스트가 있을 때만 상호작용
        if (currentNpcQuest != null)
        {
            GameManager.instance.IsPlayerInteractionWithNpc = true;

            // 퀘스트 종료 NPC 일 때
            // 현재 퀘스트가 진행 중인 상태에서 조건을 달성 했을 때
            questDetailUINpcInteraction.SetQusetNpcUIActive(true);
            questDetailUINpcInteraction.SetQusetNpcUI(this, currentNpcQuest, isQuestCompletion);
        }
    }
}
