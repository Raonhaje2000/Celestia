using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Npc : MonoBehaviour
{
    protected NpcData npcData;                // NPC의 데이터

    // NPC 이름 표시 관련
    protected GameObject npcNameTag;          // NPC 이름(호칭 + 이름) 표시 오브젝트

    TextMeshPro npcAppellationText;           // NPC 호칭 텍스트
    TextMeshPro npcNameText;                  // NPC 이름 텍스트

    // NPC 퀘스트 표시 관련
    protected GameObject npcQuestMark;        // NPC 머리위에 뜨는 퀘스트 표시

    Material questMark;                       // NPC 머리위에 뜨는 퀘스트 표시의 Material

    // NPC 미니맵 표시 관련
    protected GameObject npcQuestMinimapMark; // 미니맵에 뜨는 NPC 퀘스트 표시

    Material questMinimapMark;                // 미니맵에 뜨는 NPC 퀘스트 표시의 Material

    Texture questStartMark;                   // 퀘스트를 시작할 때(퀘스트 수락)의 Texture
    Texture questFinishMark;                  // 퀘스트를 종료할 때(퀘스트 완료)의 Texture

    Color mainQuestColor;                     // 메인 퀘스트일 때의 색
    Color subQuestColor;                      // 서브 퀘스트일 때의 색

    // 상호작용 시 뜨는 퀘스트 창
    [SerializeField] protected QuestDetailUINpcInteraction questDetailUINpcInteraction; // 상호작용 시 뜨는 퀘스트창 UI

    // NPC 퀘스트 관련
    [SerializeField] protected List<QuestData> npcMainQuests;                           // NPC의 메인 퀘스트 목록
    [SerializeField] protected List<QuestData> npcSubQuests;                            // NPC의 서브 퀘스트 목록

    [SerializeField] protected QuestData currentNpcQuest;                               // 현재 NPC의 퀘스트
    [SerializeField] protected bool isQuestCompletion;                                  // 퀘스트 완료인지 확인하는 플래그

    /// <summary>
    /// 관련 리소스 불러오기
    /// </summary>
    protected void LoadResources()
    {
        // NPC 이름 표시 관련
        npcNameTag = Instantiate(Resources.Load<GameObject>("Prefabs/NPC/NpcNameTag"));
        npcNameTag.transform.SetParent(this.transform, false);

        npcAppellationText = npcNameTag.transform.Find("NpcAppellationText").GetComponent<TextMeshPro>();
        npcNameText = npcNameTag.transform.Find("NpcNameText").GetComponent<TextMeshPro>();

        // NPC 퀘스트 표시 관련
        npcQuestMark = Instantiate(Resources.Load<GameObject>("Prefabs/NPC/NpcQuestMark"));
        npcQuestMark.transform.SetParent(this.transform, false);

        questMark = npcQuestMark.GetComponent<MeshRenderer>().material;

        // NPC 미니맵 표시 관련
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
    /// 관련 UI 불러오기
    /// </summary>
    protected void LoadUI()
    {
        // 상호작용 시 뜨는 퀘스트 창
        questDetailUINpcInteraction = GameObject.Find("Canvas_QuestDetailUI(Clone)").transform.Find("QuestDetailObject").GetComponent<QuestDetailUINpcInteraction>();
    }

    /// <summary>
    /// QuestManager에서 해당 NPC와 관련된 퀘스트 데이터 가져오기(불러오기)
    /// </summary>
    /// <param name="npcData">해당 NPC의 데이터</param>
    protected void GetNpcQuests(NpcData npcData)
    {
        npcMainQuests = QuestManager.instance.GetNpcMainQuests(npcData);
        npcSubQuests = QuestManager.instance.GetNpcSubQuests(npcData);                                                                                   
    }

    /// <summary>
    /// Npc의 네임 태그 (NPC 머리 위의 이름표) 세팅 처리
    /// </summary>
    /// <param name="npcData">해당 NPC의 데이터</param>
    protected void SetNpcNameTag(NpcData npcData)
    {
        npcAppellationText.text = npcData.NpcAppellation;
        npcNameText.text = npcData.NpcName;

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        float obejctHeight = boxCollider.bounds.size.y;

        //float nameTagDistance = obejctHeight * 1.1f;
        //float questMarkDistance = obejctHeight * 1.3f;

        // 타겟 마크 위치를 NPC 머리 위쪽으로 변경
        npcNameTag.transform.position = transform.position + new Vector3(0, obejctHeight + 0.1f, 0);
        npcQuestMark.transform.position = transform.position + new Vector3(0, obejctHeight + 0.5f, 0);                               
    }

    /// <summary>
    /// 메인 카메라에 맞춰 NPC 이름 표시 회전
    /// </summary>
    protected void RotateNameTag()
    {
        npcAppellationText.gameObject.transform.rotation = Camera.main.transform.rotation;
        npcNameText.gameObject.transform.rotation = Camera.main.transform.rotation;

        Vector3 rotationVector = new Vector3(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y + 180, Camera.main.transform.rotation.z);
        npcQuestMark.gameObject.transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// 미니맵 및 NPC 머리 위의 퀘스트 마크 활성화 여부 세팅
    /// </summary>
    /// <param name="active">활성화 여부</param>
    protected void ActiveQuestMinimapMark(bool active)
    {
        npcQuestMinimapMark.SetActive(active);
        npcQuestMark.SetActive(active);
    }

    /// <summary>
    /// 퀘스트 시작 시 미니맵 및 NPC 머리 위의 퀘스트 마크 세팅
    /// </summary>
    /// <param name="isMainQuest">메인 퀘스트 여부 (false인 경우 서브 퀘스트)</param>
    protected void SetQuestStartMinimapMark(bool isMainQuest)
    {
        // 미니맵 및 NPC 머리 위의 퀘스트 마크 활성화
        ActiveQuestMinimapMark(true);

        // 미니맵 퀘스트 마크 Texture 변경 (메인, 서브에 따라 색 변경 포함)
        questMinimapMark.SetTexture("_Texture", questStartMark);

        if (isMainQuest) questMinimapMark.SetColor("_Color", mainQuestColor);
        else questMinimapMark.SetColor("_Color", subQuestColor);

        // NPC 머리 위 퀘스트 마크 Texture 변경 (메인, 서브에 따라 색 변경 포함)                                                             
        questMark.SetTexture("_Texture", questStartMark);

        if (isMainQuest) questMark.SetColor("_Color", mainQuestColor);
        else questMark.SetColor("_Color", subQuestColor);
    }

    /// <summary>
    /// 퀘스트 종료 시 미니맵 및 NPC 머리 위의 퀘스트 마크 세팅
    /// </summary>
    /// <param name="isMainQuest">메인 퀘스트 여부 (false인 경우 서브 퀘스트)</param>
    protected void SetQuestFinishMinimapMark(bool isMainQuest)
    {
        // 미니맵 및 NPC 머리 위의 퀘스트 마크 활성화
        ActiveQuestMinimapMark(true);

        // 미니맵 퀘스트 마크 Texture 변경 (메인, 서브에 따라 색 변경 포함)
        questMinimapMark.SetTexture("_Texture", questFinishMark);

        if (isMainQuest) questMinimapMark.SetColor("_Color", mainQuestColor);
        else questMinimapMark.SetColor("_Color", subQuestColor);

        // NPC 머리 위 퀘스트 마크 Texture 변경 (메인, 서브에 따라 색 변경 포함)                                                             
        questMark.SetTexture("_Texture", questFinishMark);

        if (isMainQuest) questMark.SetColor("_Color", mainQuestColor);
        else questMark.SetColor("_Color", subQuestColor);
    }

    /// <summary>
    /// 해당 NPC의 현재 퀘스트 및 UI 업데이트
    /// </summary>
    protected void UpdateCurrentUiQuest()
    {
        if (currentNpcQuest == null) ActiveQuestMinimapMark(false);
        else ActiveQuestMinimapMark(true);

        // 메인과 서브 중 메인 퀘스트 우선, 퀘스트 시작과 퀘스트 완료 중 퀘스트 완료 우선

        // 해당 NPC의 메인 퀘스트가 있는 경우
        if (npcMainQuests.Count > 0)
        {
            if (npcMainQuests[0].CompletionNpc.NpcId == npcData.NpcId)
            {
                if (npcMainQuests[0].CompletionNpcPosition != null)
                {
                    transform.position = npcMainQuests[0].CompletionNpcPosition.position;
                    transform.rotation = npcMainQuests[0].CompletionNpcPosition.rotation;
                }

                // 메인 퀘스트의 완료 NPC일 때
                if (npcMainQuests[0].IsProgress && npcMainQuests[0].IsConditionComplete)
                {
                    // 현재 진행중인 퀘스트인데 조건 달성을 했을 경우
                    // 해당 NPC의 퀘스트를 메인 퀘스트 완료로 설정
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

                // 메인 퀘스트의 시작 NPC일 때
                if (npcMainQuests[0].QuestID == QuestManager.instance.GetNextProgressMainQuest().QuestID && !npcMainQuests[0].IsProgress)
                {
                    // 다음에 진행해야하는 퀘스트인 경우
                    // 해당 NPC의 퀘스트를 메인 퀘스트 시작으로 설정
                    currentNpcQuest = npcMainQuests[0];
                    isQuestCompletion = false;
                    SetQuestStartMinimapMark(true);
                    return;
                }
            }
        }

        // 반복문 돌리면서 현재 진행중인 퀘스트가 아니면 계속 띄움
        // 완료 우선

        for (int i = 0; i < npcSubQuests.Count; i++)
        {
            if (npcSubQuests[i].CompletionNpc.NpcId == npcData.NpcId)
            {
                // 서브 퀘스트의 완료 NPC일 때

                if (npcSubQuests[i].CompletionNpcPosition != null)
                {
                    transform.position = npcSubQuests[i].CompletionNpcPosition.position;
                    transform.rotation = npcSubQuests[i].CompletionNpcPosition.rotation;
                }

                if (npcSubQuests[i].IsProgress && npcSubQuests[0].IsConditionComplete)
                {
                    // 현재 진행중인 퀘스트인데 조건 달성을 했을 경우
                    // 해당 NPC의 퀘스트를 서브 퀘스트 완료로 설정
                    currentNpcQuest = npcSubQuests[i];
                    isQuestCompletion = true;
                    SetQuestFinishMinimapMark(false);
                    return;
                }
            }

            if (npcSubQuests[i].StartNpc.NpcId == npcData.NpcId)
            {
                // 서브 퀘스트의 시작 NPC일 때

                if (npcSubQuests[i].StartNpcPosition != null)
                {
                    transform.position = npcSubQuests[i].StartNpcPosition.position;
                    transform.rotation = npcSubQuests[i].StartNpcPosition.rotation;
                }

                if (!npcSubQuests[i].IsProgress)
                {
                    // 현재 진행 상태가 아닌 퀘스트인 경우 (받지 않은 퀘스트의 경우)
                    // 해당 NPC의 퀘스트를 서브 퀘스트 시작으로 설정
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
    /// 현재 NPC의 퀘스트를 목록에서 삭제
    /// </summary>
    public void RemoveCurrentNpcQuest()
    {
        if (currentNpcQuest.CompletionNpc.NpcId == npcData.NpcId && currentNpcQuest.IsConditionComplete && currentNpcQuest.IsFinish)
        {
            // 종료 퀘스트인 경우 조건 달성했는지 확인 후 해당 목록에서 제거
            if (currentNpcQuest.Type == QuestData.QuestType.main) npcMainQuests.RemoveAt(0);
            else npcSubQuests.RemoveAt(FindSubQuestIndex());
        }
        else if (currentNpcQuest.StartNpc.NpcId == npcData.NpcId && currentNpcQuest.CompletionNpc.NpcId != npcData.NpcId && currentNpcQuest.IsProgress)
        {
            // 시작 퀘스트인 경우 종료 퀘스트까지 담당하는지 확인 후 해당 목록에서 제거
            if (currentNpcQuest.Type == QuestData.QuestType.main) npcMainQuests.RemoveAt(0);
            else npcSubQuests.RemoveAt(FindSubQuestIndex());
        }

        currentNpcQuest = null;
    }

    /// <summary>
    /// 현재 진행 중인 서브 퀘스트의 목록에서의 인덱스를 찾아 반환
    /// </summary>
    /// <returns>현재 진행 중인 서브 퀘스트의 목록에서의 인덱스</returns>
    int FindSubQuestIndex()
    {
        for(int i = 0; i < npcSubQuests.Count; i++)
        {
            if(currentNpcQuest.QuestID == npcSubQuests[i].QuestID) return i;                                                                            
        }

        return -1;
    }

    /// <summary>
    /// NPC 데이터 세팅
    /// </summary>
    protected abstract void SetNpcData();

    /// <summary>
    /// 플레이어와의 상호작용 동작 처리<br/>
    /// 플레이어가 상호작용 키를 눌렀을 때 호출함
    /// </summary>
    public abstract void InteroperateWithPlayer();
}
