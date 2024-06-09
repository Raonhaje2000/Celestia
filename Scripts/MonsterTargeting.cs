using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterTargeting : MonoBehaviour
{
    GameObject monsterTargetingUICanvas;    // 타게팅 몬스터 표시 UI 캔버스
    GameObject monsterTargetingUIObject;    // 타게팅 몬스터 표시 UI 오브젝트
    MonsterTargetingUI monsterTargetingUI;  // 타게팅 몬스터 표시 UI 컴포넌트

    GameObject targetMarkTopPrefab;         // 타겟 마크(위) 프리팹
    Transform targetMarkTop;                // 타겟 마크(위)

    GameObject targetMarkBottomPrefab;      // 타겟 마크(아래) 프리팹
    Transform targetMarkBottom;             // 타겟 마크(아래)

    [Min(0)]
    [SerializeField] float viewAreaRadius;  // 시야 반지름

    [Range(0, 180)]
    [SerializeField] float viewAreaAngle;   // 시야 각도

    LayerMask monsterMask;                  // 몬스터 레이어 마스크

    GameObject targetMonster;               // 타겟팅 된 몬스터

    public GameObject TargetMonster
    {
        get { return targetMonster; }
    }

    private void Awake()
    {
        LoadObjects(); // 관련 오브젝트 불러오기
    }

    void Start()
    {
        Initialize(); // 초기화
    }

    void Update()
    {
        UpdateTargetMonster(); // 타겟팅 된 몬스터 업데이트
    }

    // 관련 오브젝트 불러오기
    private void LoadObjects()
    {
        monsterTargetingUICanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas_MonsterTargetingUI"));               
        monsterTargetingUIObject = monsterTargetingUICanvas.transform.Find("MonsterTargetingObject").gameObject;
        monsterTargetingUI = monsterTargetingUIObject.GetComponent<MonsterTargetingUI>();

        targetMarkTopPrefab = Resources.Load<GameObject>("Prefabs/TargetMark/MonsterTargetMarkTop");
        targetMarkBottomPrefab = Resources.Load<GameObject>("Prefabs/TargetMark/MonsterTargetMarkBottom");
    }

    // 초기화
    private void Initialize()
    {
        viewAreaRadius = 15.0f;
        viewAreaAngle = 120.0f;

        targetMarkTop = Instantiate(targetMarkTopPrefab).transform;
        targetMarkBottom = Instantiate(targetMarkBottomPrefab).transform;

        SetMonsterTargetingUIActive(false);
        SetTargetMarkTopActive(false);    // 타겟 마크(위) 비활성화
        SetTargetMarkBottomActive(false); // 타겟 마크(아래) 비활성화

        monsterMask = LayerMask.GetMask("Monster");

        targetMonster = null;
    }

    // 타겟팅 된 몬스터 업데이트
    private void UpdateTargetMonster()
    {
        if (targetMonster != null && targetMonster.activeSelf)
        {
            // 타겟 몬스터가 존재하는 경우

            SetMonsterTargetingUIActive(true);                       // 몬스터 타게팅 UI 활성화
            monsterTargetingUI.SetMonsterTargetingUI(targetMonster); // 타겟 몬스터 데이터로 몬스터 타게팅 UI 세팅

            //SetTargetMarkTop(targetMonster.transform);             // 타겟 몬스터 위에 타겟 마크 띄우기
            SetTargetMarkBottom(targetMonster.transform);            // 타겟 몬스터 아래에 타겟 마크 띄우기
        }
        else
        {
            // 타겟 몬스터가 존재하지 않는 경우

            SetMonsterTargetingUIActive(false); // 몬스터 타게팅 UI 활성화
            SetTargetMarkBottomActive(false);   // 타겟 마크(아럐) 비활성화
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab 키를 눌렀을 경우
            // 플레이어와 가장 가까운 몬스터를 타겟으로 지정
            targetMonster = GetTargetMonsterNear();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // 마우스를 클릭했을 경우
            // 클릭된 몬스터를 타겟으로 지정
            targetMonster = GetTargetMonsterClick();
        }
        else if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            // ` 키를 눌렀을 경우
            // 타게팅 해제
            targetMonster = null;
        }
    }

    // 시야각 안에 들어온 몬스터들 구하기
    private List<Transform> GetFieldOfViewMonsters()
    {
        List<Transform> fieldOfViewMonsters = new List<Transform>(); // 시야각 안의 몬스터 목록

        // 범위 안에 들어온 오브젝트 중 몬스터인 경우만 받아옴
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewAreaRadius, monsterMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Transform monster = colliders[i].transform;

            // 범위 내에 들어온 몬스터가 시야 안에 들어왔는지 확인 후 타겟 몬스터 목록에 넣음
            Vector3 direction = monster.position - transform.position;
            direction.y = 0;

            // 몬스터의 거리 보정 (몬스터 중심이 범위 안에 들어왔을 때)
            if (GetMonsterDistance(monster.transform) <= viewAreaRadius && Vector3.Dot(direction.normalized, transform.forward) > CalcAngle(viewAreaAngle / 2).z)
            {
                if (monster != null) fieldOfViewMonsters.Add(monster);
            }
        }

        return fieldOfViewMonsters;
    }

    // 가장 가까운 타겟 몬스터 구하기
    private GameObject GetTargetMonsterNear()
    {
        List<Transform> fieldOfViewMonsters = GetFieldOfViewMonsters();

        GameObject targetMonsterNear = null;

        // 최솟값 구하기 알고리즘
        if (fieldOfViewMonsters.Count > 0)
        {
            int nearIndex = 0;

            for(int i = 0; i < fieldOfViewMonsters.Count; i++)
            {
                if(GetMonsterDistance(fieldOfViewMonsters[nearIndex]) > GetMonsterDistance(fieldOfViewMonsters[i]))
                {
                    nearIndex = i;
                }
            }

            targetMonsterNear = fieldOfViewMonsters[nearIndex].gameObject;
        }

        return targetMonsterNear;
    }

    // 마우스 클릭된 타겟 몬스터 구하기
    private GameObject GetTargetMonsterClick()
    {
        GameObject targetMonsterClick = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 마우스로 클릭된 오브젝트가 몬스터인 경우
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterMask))
        {
            GameObject clickMonster = hit.transform.gameObject;

            Vector3 direction = clickMonster.transform.position - transform.position;
            direction.y = 0;

            // 몬스터가 시야 범위 내에 있을 경우
            if (GetMonsterDistance(clickMonster.transform) <= viewAreaRadius && Vector3.Dot(direction.normalized, transform.forward) > CalcAngle(viewAreaAngle / 2).z)
            {
                targetMonsterClick = clickMonster;
            }
        }

        return targetMonsterClick;
    }

    // 타겟 몬스터 위에 타겟 마크 띄우기
    private void SetTargetMarkTop(Transform monsterTransform)
    {
        SetTargetMarkTopActive(true);

        // 해당 몬스터의 높이 (y축 길이) 구하기
        BoxCollider boxCollider = monsterTransform.gameObject.GetComponent<BoxCollider>();
        float obejctHeight = boxCollider.bounds.size.y;

        // 몬스터 중심과 타겟 마크 사이의 거리
        float targetMarkDistance = obejctHeight / 2.0f + 1.0f;

        // 타겟 마크 위치를 몬스터 머리 위쪽으로 변경
        Vector3 markPosition = new Vector3(monsterTransform.position.x, monsterTransform.position.y + targetMarkDistance, monsterTransform.position.z);
        targetMarkTop.transform.position = markPosition;
    }

    // 타겟 몬스터 아래에 타겟 마크 띄우기
    private void SetTargetMarkBottom(Transform monsterTransform)
    {
        SetTargetMarkBottomActive(true);

        // 해당 몬스터의 너비 (x, z축 중 긴 길이) 구하기
        BoxCollider boxCollider = monsterTransform.gameObject.GetComponent<BoxCollider>();
        float obejctWidth = Mathf.Max(boxCollider.bounds.size.x, boxCollider.bounds.size.z);
        float yPosition = monsterTransform.position.y - boxCollider.bounds.size.y / 2.0f + 1.25f;

        // 타겟 마크 위치를 몬스터 발 밑으로 변경
        Vector3 markPosition = new Vector3(monsterTransform.position.x, yPosition, monsterTransform.position.z);
        targetMarkBottom.transform.position = markPosition;

        // 타겟 마크 크기 변경 (부모 객체로 인해 늘어난 만큼 나눠줌으로서 비율 맞춤)
        targetMarkBottom.transform.localScale = new Vector3(obejctWidth / monsterTransform.localScale.x, 1, obejctWidth / monsterTransform.localScale.z);
    }

    public void SetMonsterTargetingUIActive(bool active)
    {
        monsterTargetingUIObject.SetActive(active);
    }

    // 타겟 마크(위) 활성화 설정
    public void SetTargetMarkTopActive(bool active)
    {
        targetMarkTop.gameObject.SetActive(active);
    }

    // 타겟 마크(아래) 활성화 설정
    public void SetTargetMarkBottomActive(bool active)
    {
        targetMarkBottom.gameObject.SetActive(active);
    }

    // 시야 기즈모 그리기
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;

        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, viewAreaRadius);

        Vector3 viewLeft = transform.position + CalcAngle(-viewAreaAngle / 2 + transform.rotation.eulerAngles.y) * viewAreaRadius;
        Vector3 viewRight = transform.position + CalcAngle(viewAreaAngle / 2 + transform.rotation.eulerAngles.y) * viewAreaRadius;

        Handles.DrawLine(transform.position, viewLeft);
        Handles.DrawLine(transform.position, viewRight);
    }

    // 각도 계산
    private Vector3 CalcAngle(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;

        Vector3 angle = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));

        return angle;
    }

    // 플레이어와 몬스터 사이의 거리 계산
    private float GetMonsterDistance(Transform monsterTransform)
    {
        Vector3 player = transform.position;
        Vector3 monster = monsterTransform.position;

        player.y = 0;
        monster.y = 0;

        return Vector3.Distance(player, monster);
    }
}
