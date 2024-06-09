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
