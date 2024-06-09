using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Procyon : Npc
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
    }

    void Update()
    {
        RotateNameTag();
        UpdateCurrentUiQuest();
    }

    protected override void SetNpcData()
    {
        npcData = ScriptableObject.Instantiate(Resources.Load<NpcData>("GameData/Npc/Main/ProcyonData"));                  
    }

    public override void InteroperateWithPlayer()
    {
        // 현재 NPC와 관련된 퀘스트가 있을 때만 상호작용
        if (currentNpcQuest != null)
        {
            GameManager.instance.IsPlayerInteractionWithNpc = true;

            // 퀘스트 창 UI 띄우기
            questDetailUINpcInteraction.SetQusetNpcUIActive(true);
            questDetailUINpcInteraction.SetQusetNpcUI(this, currentNpcQuest, isQuestCompletion);                            
        }
    }
}
