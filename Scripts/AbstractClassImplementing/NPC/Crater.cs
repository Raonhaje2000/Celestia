using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crater : Npc
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
        npcData = ScriptableObject.Instantiate(Resources.Load<NpcData>("GameData/Npc/Main/CraterData"));                   
    }

    public override void InteroperateWithPlayer()
    {
        // ���� NPC�� ���õ� ����Ʈ�� ���� ���� ��ȣ�ۿ�
        if (currentNpcQuest != null)
        {
            GameManager.instance.IsPlayerInteractionWithNpc = true;

            // ����Ʈ ���� NPC �� ��
            // ���� ����Ʈ�� ���� ���� ���¿��� ������ �޼� ���� ��
            questDetailUINpcInteraction.SetQusetNpcUIActive(true);
            questDetailUINpcInteraction.SetQusetNpcUI(this, currentNpcQuest, isQuestCompletion);
        }
    }
}
