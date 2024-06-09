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
        // ���� NPC�� ���õ� ����Ʈ�� ���� ���� ��ȣ�ۿ�
        if (currentNpcQuest != null)
        {
            GameManager.instance.IsPlayerInteractionWithNpc = true;

            // ����Ʈ â UI ����
            questDetailUINpcInteraction.SetQusetNpcUIActive(true);
            questDetailUINpcInteraction.SetQusetNpcUI(this, currentNpcQuest, isQuestCompletion);                            
        }
    }
}