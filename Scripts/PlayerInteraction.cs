using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerInteraction : MonoBehaviour
{
    GameObject interactionUICanvas;                    // 상호작용 표시 UI 캔버스
    GameObject interactionUIObject;                    // 상호작용 표시 UI 오브젝트
    InteractionUI interactionUI;                       // 상호작용 표시 UI 컴포넌트

    Sprite interactionNpc;                             // NPC 상호작용 스프라이트
    string interactionNpcText;                         // NPC 상호작용 텍스트

    Sprite interactionObject;                          // 오브젝트 상호작용 스프라이트
    string interactionObjectText;                      // 오브젝트 상호작용 텍스트

    Sprite interactionPortal;                          // 오브젝트 상호작용 스프라이트

    GameObject interactionTargetMark;                  // 상호작용 타겟 마크

    [Min(0)]
    [SerializeField] float interactionRange;           // 상호작용 범위

    LayerMask interactionMask;                         // 상호작용 레이어 마스크

    GameObject targetObejct;                           // 상호작용할 타겟 오브젝트

    [SerializeField] KeyCode interactionKey;           // 상호작용 키

    GameObject questDetailUICanvas;                    // NPC 퀘스트 UI 캔버스
    GameObject questDetailObject;                      // NPC 퀘스트 UI 오브젝트
    QuestDetailUINpcInteraction qusetNpcUI;            // NPC 퀘스트 UI 컴포넌트

    GameObject systemUIObject;

    private void Awake()
    {
        LoadResources(); // 관련 오브젝트 불러오기
    }

    void Start()
    {
        Initialize(); // 초기화
    }

    void Update()
    {
        // 상호작용할 타겟 오브젝트 업데이트
        UpdateTargetObejct();

        if (targetObejct != null)
        {
            // 타겟 오브젝트가 있는 경우

            // 상호작용 UI와 상호작용 타겟 마크 띄우기
            //SetInteractionUIAndTargetMark();

            if (!GameManager.instance.IsPlayerInteractionWithNpc)
            {
                // 타겟 오브젝트가 지정된 상태에서 상호작용 키나 해당 오브젝트를 마우스 클릭했을 경우
                if (Input.GetKeyDown(interactionKey) || (Input.GetMouseButtonDown(0) && CheckMouseClickTargetObject()))                   
                {
                    // 상호작용 동작 처리
                    GetInteraction();
                }
            }
        }
        else
        {
            // 타겟 오브젝트가 없는 경우
            // 상호작용 UI와 상호작용 타겟 마크 비활성화
            //SetInteractionUIAndTargetMarkActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.IsPlayerInteractionWithNpc)
        {
            // 플레이어가 NPC와 상호작용하고 있지 않은 경우

            if (targetObejct != null)
            {
                // 상호작용 UI와 상호작용 타겟 마크 띄우기
                SetInteractionUIAndTargetMark();
            }
            else
            {
                // 타겟 오브젝트가 없는 경우
                // 상호작용 UI와 상호작용 타겟 마크 비활성화
                SetInteractionUIAndTargetMarkActive(false);
            }
        }
        else
        {
            // 플레이어가 NPC와 상호작용하고 있는 경우
            // 상호작용 UI와 상호작용 타겟 마크 비활성화
            SetInteractionUIAndTargetMarkActive(false);
        }
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        interactionUICanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas_InteractionUI"));                               
        interactionUIObject = interactionUICanvas.transform.Find("InteractionUIObject").gameObject;
        interactionUI = interactionUIObject.GetComponent<InteractionUI>();

        interactionNpc = Resources.Load<Sprite>("Icons/Interaction/Npc");
        interactionObject = Resources.Load<Sprite>("Icons/Interaction/Object");
        interactionPortal = Resources.Load<Sprite>("Icons/Interaction/Portal");

        interactionTargetMark = Instantiate(Resources.Load<GameObject>("Prefabs/TargetMark/InteractionTargetMark"));

        questDetailUICanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas_QuestDetailUI"));
        questDetailObject = questDetailUICanvas.transform.Find("QuestDetailObject").gameObject;
        qusetNpcUI = questDetailObject.GetComponent<QuestDetailUINpcInteraction>();

        systemUIObject = interactionUICanvas.transform.Find("SystemUIObject").gameObject;
    }

    // 초기화
    void Initialize()
    {
        interactionNpcText = "대화하기";
        interactionObjectText = "조사하기";

        interactionRange = 5.0f;

        interactionMask = LayerMask.GetMask(new string[] { "NPC", "InteractionObject", "Portal" });                                         

        targetObejct = null;

        interactionKey = KeyCode.G;

        SetInteractionUIAndTargetMarkActive(false);

        questDetailObject.SetActive(false);
        systemUIObject.SetActive(false);
    }

    // 상호작용 동작 처리
    void GetInteraction()
    {
        // 플레이어가 해당 오브젝트를 바라보도록 회전
        //transform.LookAt(new Vector3(targetObejct.transform.position.x, transform.position.y, targetObejct.transform.position.z));     

        // 레이어에 따른 상호작용 동작 처리
        if (targetObejct.layer == LayerMask.NameToLayer("NPC"))
        {
            // 타겟 오브젝트의 레이어가 NPC인 경우
            // NPC와의 상호작용 동작 처리
            targetObejct.GetComponent<Npc>().InteroperateWithPlayer();
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("InteractionObject"))
        {
            // 타겟 오브젝트의 레이어가 상호작용 오브젝트인 경우
            // 오브젝트와의 상호작용 동작 처리
            targetObejct.GetComponent<InteractionObject>().InteroperateWithPlayer();
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("Portal"))
        {
            // 타겟 오브젝트의 레이어가 포탈인 경우
            // 포탈과의 상호작용 동작 처리
            targetObejct.GetComponent<Portal>().MoveDestination();
        }
    }

    // 상호작용할 타겟 오브젝트 업데이트
    void UpdateTargetObejct()
    {
        targetObejct = GetRangeObjectNear();
    }

    // 상호작용 범위안에 있는 오브젝트들 반환
    List<Transform> GetRangeObjects()
    {
        List<Transform> rangeObjects = new List<Transform>(); // 범위 안의 상호작용 오브젝트 목록

        // 범위 안에 들어온 오브젝트 중 상호작용 오브젝트인 경우만 받아옴
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactionMask);                      

        for(int i = 0; i < colliders.Length; i++)
        {
            // 오브젝트의 거리 보정 (오브젝트 중심이 범위 안에 들어왔을 때)
            if (GetObjectDistance(colliders[i].transform) <= interactionRange)
                rangeObjects.Add(colliders[i].gameObject.transform);
        }

        return rangeObjects;
    }

    // 가장 가까운 상호작용 타겟 오브젝트 구하기
    GameObject GetRangeObjectNear()
    {
        List<Transform> rangeObjects = GetRangeObjects();

        GameObject rangeObjectNear = null;

        // 최솟값 구하기 알고리즘
        if (rangeObjects.Count > 0)
        {
            int nearIndex = 0;

            for (int i = 0; i < rangeObjects.Count; i++)
            {
                if (GetObjectDistance(rangeObjects[nearIndex]) > GetObjectDistance(rangeObjects[i]))                                          
                {
                    nearIndex = i;
                }
            }

            rangeObjectNear = rangeObjects[nearIndex].gameObject;
        }

        return rangeObjectNear;
    }

    // 마우스로 클릭한 오브젝트가 상호작용 타겟 오브젝트인지 확인
    bool CheckMouseClickTargetObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 마우스로 클릭된 오브젝트가 상호작용 할 수 있는 오브젝트인 경우
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionMask))                                                         
        {
            GameObject clickObject = hit.transform.gameObject;

            // 타겟 오브젝트와 일치하는지 확인
            if (clickObject == targetObejct) return true;
        }

        return false;
    }

    // 상호작용 UI와 상호작용 타겟 마크 띄우기
    void SetInteractionUIAndTargetMark()
    {
        // 상호작용 UI와 상호작용 타겟 마크 활성화
        SetInteractionUIAndTargetMarkActive(true);

        SetTargetMark();    // 타겟 마크 세팅
        SetInteractionUI(); // 상호작용 UI 세팅
    }

    // 상호작용 UI 세팅
    void SetInteractionUI()
    {
        // 레이어에 따른 상호작용 UI 세팅
        if (targetObejct.layer == LayerMask.NameToLayer("NPC"))
        {
            // 타겟 오브젝트의 레이어가 NPC인 경우
            // NPC와 상호작용할 때로 세팅
            interactionUI.SetInteractionIconUI(interactionNpc, interactionNpcText, interactionKey);
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("InteractionObject"))
        {
            // 타겟 오브젝트의 레이어가 상호작용 오브젝트인 경우
            // 오브젝트와 상호작용할 때로 세팅
            interactionUI.SetInteractionIconUI(interactionObject, interactionObjectText, interactionKey);
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("Portal"))
        {
            // 타겟 오브젝트의 레이어가 포탈인 경우
            // 포탈과 상호작용할 때로 세팅
            string temp = string.Format("\'{0}\'(으)로 이동", targetObejct.GetComponent<Portal>().GetDestination());
            interactionUI.SetInteractionIconUI(interactionPortal, temp, interactionKey);

            interactionTargetMark.SetActive(false); // 타겟 마크 비활성화
        }

        // 상호작용 UI를 플레이어 머리 위로 이동
        interactionUIObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2.0f, 0));
    }

    // 타겟 마크 세팅
    void SetTargetMark()
    {
        // 해당 오브젝트의 너비 (x, z축 중 긴 길이) 구하기
        BoxCollider boxCollider = targetObejct.gameObject.GetComponent<BoxCollider>();
        float obejctWidth = Mathf.Max(boxCollider.bounds.size.x, boxCollider.bounds.size.z);

        // 타겟 오브젝트의 자식 객체로 설정
        interactionTargetMark.transform.parent = targetObejct.transform;

        // 타겟 마크 위치를 타겟 오브젝트 발 밑으로 변경
        Vector3 markPosition = new Vector3(targetObejct.transform.position.x, 0.25f, targetObejct.transform.position.z);
        interactionTargetMark.transform.position = markPosition;

        // 타겟 마크 크기 변경 (부모 객체로 인해 늘어난 만큼 나눠줌으로서 비율 맞춤)
        Vector3 targetLocalScale = targetObejct.transform.localScale;
        interactionTargetMark.transform.localScale = new Vector3(obejctWidth / targetLocalScale.x, 1, obejctWidth / targetLocalScale.z);
    }

    // 상호작용 UI와 상호작용 타겟 마크 활성화 세팅
    void SetInteractionUIAndTargetMarkActive(bool active)
    {
        interactionUIObject.SetActive(active);
        interactionTargetMark.SetActive(active);
    }

    // 플레이어와 오브젝트 사이의 거리 계산
    private float GetObjectDistance(Transform objectTransform)
    {
        Vector3 player = transform.position;
        Vector3 obj = objectTransform.position;

        player.y = 0;
        obj.y = 0;

        return Vector3.Distance(player, obj);                                                                                           
    }

    // 상호작용 범위 기즈모 그리기
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;

        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, interactionRange);
    }
}
