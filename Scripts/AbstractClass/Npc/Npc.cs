using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Npc : MonoBehaviour
{
    protected NpcData npcData;                // NPC�� ������

    // NPC �̸� ǥ�� ����
    protected GameObject npcNameTag;          // NPC �̸�(ȣĪ + �̸�) ǥ�� ������Ʈ

    TextMeshPro npcAppellationText;           // NPC ȣĪ �ؽ�Ʈ
    TextMeshPro npcNameText;                  // NPC �̸� �ؽ�Ʈ

    // NPC ����Ʈ ǥ�� ����
    protected GameObject npcQuestMark;        // NPC �Ӹ����� �ߴ� ����Ʈ ǥ��

    Material questMark;                       // NPC �Ӹ����� �ߴ� ����Ʈ ǥ���� Material

    // NPC �̴ϸ� ǥ�� ����
    protected GameObject npcQuestMinimapMark; // �̴ϸʿ� �ߴ� NPC ����Ʈ ǥ��

    Material questMinimapMark;                // �̴ϸʿ� �ߴ� NPC ����Ʈ ǥ���� Material

    Texture questStartMark;                   // ����Ʈ�� ������ ��(����Ʈ ����)�� Texture
    Texture questFinishMark;                  // ����Ʈ�� ������ ��(����Ʈ �Ϸ�)�� Texture

    Color mainQuestColor;                     // ���� ����Ʈ�� ���� ��
    Color subQuestColor;                      // ���� ����Ʈ�� ���� ��

    // ��ȣ�ۿ� �� �ߴ� ����Ʈ â
    [SerializeField] protected QuestDetailUINpcInteraction questDetailUINpcInteraction; // ��ȣ�ۿ� �� �ߴ� ����Ʈâ UI

    // NPC ����Ʈ ����
    [SerializeField] protected List<QuestData> npcMainQuests;                           // NPC�� ���� ����Ʈ ���
    [SerializeField] protected List<QuestData> npcSubQuests;                            // NPC�� ���� ����Ʈ ���

    [SerializeField] protected QuestData currentNpcQuest;                               // ���� NPC�� ����Ʈ
    [SerializeField] protected bool isQuestCompletion;                                  // ����Ʈ �Ϸ����� Ȯ���ϴ� �÷���

    /// <summary>
    /// ���� ���ҽ� �ҷ�����
    /// </summary>
    protected void LoadResources()
    {
        // NPC �̸� ǥ�� ����
        npcNameTag = Instantiate(Resources.Load<GameObject>("Prefabs/NPC/NpcNameTag"));
        npcNameTag.transform.SetParent(this.transform, false);

        npcAppellationText = npcNameTag.transform.Find("NpcAppellationText").GetComponent<TextMeshPro>();
        npcNameText = npcNameTag.transform.Find("NpcNameText").GetComponent<TextMeshPro>();

        // NPC ����Ʈ ǥ�� ����
        npcQuestMark = Instantiate(Resources.Load<GameObject>("Prefabs/NPC/NpcQuestMark"));
        npcQuestMark.transform.SetParent(this.transform, false);

        questMark = npcQuestMark.GetComponent<MeshRenderer>().material;

        // NPC �̴ϸ� ǥ�� ����
        npcQuestMinimapMark = Instantiate(Resources.Load<GameObject>("Prefabs/NPC/NpcQuestMinimapMark"));
        npcQuestMinimapMark.transform.SetParent(this.transform, false);
        npcQuestMinimapMark.transform.rotation = Quaternion.Euler(Vector3.zero);

        questMinimapMark = npcQuestMinimapMark.GetComponent<MeshRenderer>().material;

        questStartMark = Resources.Load<Texture>("Images/ExclamationMark");
        questFinishMark = Resources.Load<Texture>("Images/QuestionMark");

        mainQuestColor = new Color(255 / 255.0f, 140 / 255.0f, 0 / 255.0f, 255 / 255.0f);
        subQuestColor = new Color(0 / 255.0f, 140 / 255.0f, 255 / 255.0f, 255 / 255.0f);
    }

    /// <summary>
    /// ���� UI �ҷ�����
    /// </summary>
    protected void LoadUI()
    {
        // ��ȣ�ۿ� �� �ߴ� ����Ʈ â
        questDetailUINpcInteraction = GameObject.Find("Canvas_QuestDetailUI(Clone)").transform.Find("QuestDetailObject").GetComponent<QuestDetailUINpcInteraction>();
    }

    /// <summary>
    /// QuestManager���� �ش� NPC�� ���õ� ����Ʈ ������ ��������(�ҷ�����)
    /// </summary>
    /// <param name="npcData">�ش� NPC�� ������</param>
    protected void GetNpcQuests(NpcData npcData)
    {
        npcMainQuests = QuestManager.instance.GetNpcMainQuests(npcData);
        npcSubQuests = QuestManager.instance.GetNpcSubQuests(npcData);                                                                                   
    }

    /// <summary>
    /// Npc�� ���� �±� (NPC �Ӹ� ���� �̸�ǥ) ���� ó��
    /// </summary>
    /// <param name="npcData">�ش� NPC�� ������</param>
    protected void SetNpcNameTag(NpcData npcData)
    {
        npcAppellationText.text = npcData.NpcAppellation;
        npcNameText.text = npcData.NpcName;

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        float obejctHeight = boxCollider.bounds.size.y;

        //float nameTagDistance = obejctHeight * 1.1f;
        //float questMarkDistance = obejctHeight * 1.3f;

        // Ÿ�� ��ũ ��ġ�� NPC �Ӹ� �������� ����
        npcNameTag.transform.position = transform.position + new Vector3(0, obejctHeight + 0.1f, 0);
        npcQuestMark.transform.position = transform.position + new Vector3(0, obejctHeight + 0.5f, 0);                               
    }

    /// <summary>
    /// ���� ī�޶� ���� NPC �̸� ǥ�� ȸ��
    /// </summary>
    protected void RotateNameTag()
    {
        npcAppellationText.gameObject.transform.rotation = Camera.main.transform.rotation;
        npcNameText.gameObject.transform.rotation = Camera.main.transform.rotation;

        Vector3 rotationVector = new Vector3(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y + 180, Camera.main.transform.rotation.z);
        npcQuestMark.gameObject.transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// �̴ϸ� �� NPC �Ӹ� ���� ����Ʈ ��ũ Ȱ��ȭ ���� ����
    /// </summary>
    /// <param name="active">Ȱ��ȭ ����</param>
    protected void ActiveQuestMinimapMark(bool active)
    {
        npcQuestMinimapMark.SetActive(active);
        npcQuestMark.SetActive(active);
    }

    /// <summary>
    /// ����Ʈ ���� �� �̴ϸ� �� NPC �Ӹ� ���� ����Ʈ ��ũ ����
    /// </summary>
    /// <param name="isMainQuest">���� ����Ʈ ���� (false�� ��� ���� ����Ʈ)</param>
    protected void SetQuestStartMinimapMark(bool isMainQuest)
    {
        // �̴ϸ� �� NPC �Ӹ� ���� ����Ʈ ��ũ Ȱ��ȭ
        ActiveQuestMinimapMark(true);

        // �̴ϸ� ����Ʈ ��ũ Texture ���� (����, ���꿡 ���� �� ���� ����)
        questMinimapMark.SetTexture("_Texture", questStartMark);

        if (isMainQuest) questMinimapMark.SetColor("_Color", mainQuestColor);
        else questMinimapMark.SetColor("_Color", subQuestColor);

        // NPC �Ӹ� �� ����Ʈ ��ũ Texture ���� (����, ���꿡 ���� �� ���� ����)                                                             
        questMark.SetTexture("_Texture", questStartMark);

        if (isMainQuest) questMark.SetColor("_Color", mainQuestColor);
        else questMark.SetColor("_Color", subQuestColor);
    }

    /// <summary>
    /// ����Ʈ ���� �� �̴ϸ� �� NPC �Ӹ� ���� ����Ʈ ��ũ ����
    /// </summary>
    /// <param name="isMainQuest">���� ����Ʈ ���� (false�� ��� ���� ����Ʈ)</param>
    protected void SetQuestFinishMinimapMark(bool isMainQuest)
    {
        // �̴ϸ� �� NPC �Ӹ� ���� ����Ʈ ��ũ Ȱ��ȭ
        ActiveQuestMinimapMark(true);

        // �̴ϸ� ����Ʈ ��ũ Texture ���� (����, ���꿡 ���� �� ���� ����)
        questMinimapMark.SetTexture("_Texture", questFinishMark);

        if (isMainQuest) questMinimapMark.SetColor("_Color", mainQuestColor);
        else questMinimapMark.SetColor("_Color", subQuestColor);

        // NPC �Ӹ� �� ����Ʈ ��ũ Texture ���� (����, ���꿡 ���� �� ���� ����)                                                             
        questMark.SetTexture("_Texture", questFinishMark);

        if (isMainQuest) questMark.SetColor("_Color", mainQuestColor);
        else questMark.SetColor("_Color", subQuestColor);
    }

    /// <summary>
    /// �ش� NPC�� ���� ����Ʈ �� UI ������Ʈ
    /// </summary>
    protected void UpdateCurrentUiQuest()
    {
        if (currentNpcQuest == null) ActiveQuestMinimapMark(false);
        else ActiveQuestMinimapMark(true);

        // ���ΰ� ���� �� ���� ����Ʈ �켱, ����Ʈ ���۰� ����Ʈ �Ϸ� �� ����Ʈ �Ϸ� �켱

        // �ش� NPC�� ���� ����Ʈ�� �ִ� ���
        if (npcMainQuests.Count > 0)
        {
            if (npcMainQuests[0].CompletionNpc.NpcId == npcData.NpcId)
            {
                if (npcMainQuests[0].CompletionNpcPosition != null)
                {
                    transform.position = npcMainQuests[0].CompletionNpcPosition.position;
                    transform.rotation = npcMainQuests[0].CompletionNpcPosition.rotation;
                }

                // ���� ����Ʈ�� �Ϸ� NPC�� ��
                if (npcMainQuests[0].IsProgress && npcMainQuests[0].IsConditionComplete)
                {
                    // ���� �������� ����Ʈ�ε� ���� �޼��� ���� ���
                    // �ش� NPC�� ����Ʈ�� ���� ����Ʈ �Ϸ�� ����
                    currentNpcQuest = npcMainQuests[0];
                    isQuestCompletion = true;
                    SetQuestFinishMinimapMark(true);
                    return;
                }
            }

            if (npcMainQuests[0].StartNpc.NpcId == npcData.NpcId)
            {
                if (npcMainQuests[0].StartNpcPosition != null)
                {
                    transform.position = npcMainQuests[0].StartNpcPosition.position;
                    transform.rotation = npcMainQuests[0].StartNpcPosition.rotation;
                }

                // ���� ����Ʈ�� ���� NPC�� ��
                if (npcMainQuests[0].QuestID == QuestManager.instance.GetNextProgressMainQuest().QuestID && !npcMainQuests[0].IsProgress)
                {
                    // ������ �����ؾ��ϴ� ����Ʈ�� ���
                    // �ش� NPC�� ����Ʈ�� ���� ����Ʈ �������� ����
                    currentNpcQuest = npcMainQuests[0];
                    isQuestCompletion = false;
                    SetQuestStartMinimapMark(true);
                    return;
                }
            }
        }

        // �ݺ��� �����鼭 ���� �������� ����Ʈ�� �ƴϸ� ��� ���
        // �Ϸ� �켱

        for (int i = 0; i < npcSubQuests.Count; i++)
        {
            if (npcSubQuests[i].CompletionNpc.NpcId == npcData.NpcId)
            {
                // ���� ����Ʈ�� �Ϸ� NPC�� ��

                if (npcSubQuests[i].CompletionNpcPosition != null)
                {
                    transform.position = npcSubQuests[i].CompletionNpcPosition.position;
                    transform.rotation = npcSubQuests[i].CompletionNpcPosition.rotation;
                }

                if (npcSubQuests[i].IsProgress && npcSubQuests[0].IsConditionComplete)
                {
                    // ���� �������� ����Ʈ�ε� ���� �޼��� ���� ���
                    // �ش� NPC�� ����Ʈ�� ���� ����Ʈ �Ϸ�� ����
                    currentNpcQuest = npcSubQuests[i];
                    isQuestCompletion = true;
                    SetQuestFinishMinimapMark(false);
                    return;
                }
            }

            if (npcSubQuests[i].StartNpc.NpcId == npcData.NpcId)
            {
                // ���� ����Ʈ�� ���� NPC�� ��

                if (npcSubQuests[i].StartNpcPosition != null)
                {
                    transform.position = npcSubQuests[i].StartNpcPosition.position;
                    transform.rotation = npcSubQuests[i].StartNpcPosition.rotation;
                }

                if (!npcSubQuests[i].IsProgress)
                {
                    // ���� ���� ���°� �ƴ� ����Ʈ�� ��� (���� ���� ����Ʈ�� ���)
                    // �ش� NPC�� ����Ʈ�� ���� ����Ʈ �������� ����
                    currentNpcQuest = npcSubQuests[i];
                    isQuestCompletion = false;
                    SetQuestStartMinimapMark(false);
                    return;
                }
            }

            currentNpcQuest = null;
        }
    }

    /// <summary>
    /// ���� NPC�� ����Ʈ�� ��Ͽ��� ����
    /// </summary>
    public void RemoveCurrentNpcQuest()
    {
        if (currentNpcQuest.CompletionNpc.NpcId == npcData.NpcId && currentNpcQuest.IsConditionComplete && currentNpcQuest.IsFinish)
        {
            // ���� ����Ʈ�� ��� ���� �޼��ߴ��� Ȯ�� �� �ش� ��Ͽ��� ����
            if (currentNpcQuest.Type == QuestData.QuestType.main) npcMainQuests.RemoveAt(0);
            else npcSubQuests.RemoveAt(FindSubQuestIndex());
        }
        else if (currentNpcQuest.StartNpc.NpcId == npcData.NpcId && currentNpcQuest.CompletionNpc.NpcId != npcData.NpcId && currentNpcQuest.IsProgress)
        {
            // ���� ����Ʈ�� ��� ���� ����Ʈ���� ����ϴ��� Ȯ�� �� �ش� ��Ͽ��� ����
            if (currentNpcQuest.Type == QuestData.QuestType.main) npcMainQuests.RemoveAt(0);
            else npcSubQuests.RemoveAt(FindSubQuestIndex());
        }

        currentNpcQuest = null;
    }

    /// <summary>
    /// ���� ���� ���� ���� ����Ʈ�� ��Ͽ����� �ε����� ã�� ��ȯ
    /// </summary>
    /// <returns>���� ���� ���� ���� ����Ʈ�� ��Ͽ����� �ε���</returns>
    int FindSubQuestIndex()
    {
        for(int i = 0; i < npcSubQuests.Count; i++)
        {
            if(currentNpcQuest.QuestID == npcSubQuests[i].QuestID) return i;                                                                            
        }

        return -1;
    }

    /// <summary>
    /// NPC ������ ����
    /// </summary>
    protected abstract void SetNpcData();

    /// <summary>
    /// �÷��̾���� ��ȣ�ۿ� ���� ó��<br/>
    /// �÷��̾ ��ȣ�ۿ� Ű�� ������ �� ȣ����
    /// </summary>
    public abstract void InteroperateWithPlayer();
}
