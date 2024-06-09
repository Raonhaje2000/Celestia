using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterTargeting : MonoBehaviour
{
    GameObject monsterTargetingUICanvas;    // Ÿ���� ���� ǥ�� UI ĵ����
    GameObject monsterTargetingUIObject;    // Ÿ���� ���� ǥ�� UI ������Ʈ
    MonsterTargetingUI monsterTargetingUI;  // Ÿ���� ���� ǥ�� UI ������Ʈ

    GameObject targetMarkTopPrefab;         // Ÿ�� ��ũ(��) ������
    Transform targetMarkTop;                // Ÿ�� ��ũ(��)

    GameObject targetMarkBottomPrefab;      // Ÿ�� ��ũ(�Ʒ�) ������
    Transform targetMarkBottom;             // Ÿ�� ��ũ(�Ʒ�)

    [Min(0)]
    [SerializeField] float viewAreaRadius;  // �þ� ������

    [Range(0, 180)]
    [SerializeField] float viewAreaAngle;   // �þ� ����

    LayerMask monsterMask;                  // ���� ���̾� ����ũ

    GameObject targetMonster;               // Ÿ���� �� ����

    public GameObject TargetMonster
    {
        get { return targetMonster; }
    }

    private void Awake()
    {
        LoadObjects(); // ���� ������Ʈ �ҷ�����
    }

    void Start()
    {
        Initialize(); // �ʱ�ȭ
    }

    void Update()
    {
        UpdateTargetMonster(); // Ÿ���� �� ���� ������Ʈ
    }

    // ���� ������Ʈ �ҷ�����
    private void LoadObjects()
    {
        monsterTargetingUICanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Canvas_MonsterTargetingUI"));               
        monsterTargetingUIObject = monsterTargetingUICanvas.transform.Find("MonsterTargetingObject").gameObject;
        monsterTargetingUI = monsterTargetingUIObject.GetComponent<MonsterTargetingUI>();

        targetMarkTopPrefab = Resources.Load<GameObject>("Prefabs/TargetMark/MonsterTargetMarkTop");
        targetMarkBottomPrefab = Resources.Load<GameObject>("Prefabs/TargetMark/MonsterTargetMarkBottom");
    }

    // �ʱ�ȭ
    private void Initialize()
    {
        viewAreaRadius = 15.0f;
        viewAreaAngle = 120.0f;

        targetMarkTop = Instantiate(targetMarkTopPrefab).transform;
        targetMarkBottom = Instantiate(targetMarkBottomPrefab).transform;

        SetMonsterTargetingUIActive(false);
        SetTargetMarkTopActive(false);    // Ÿ�� ��ũ(��) ��Ȱ��ȭ
        SetTargetMarkBottomActive(false); // Ÿ�� ��ũ(�Ʒ�) ��Ȱ��ȭ

        monsterMask = LayerMask.GetMask("Monster");

        targetMonster = null;
    }

    // Ÿ���� �� ���� ������Ʈ
    private void UpdateTargetMonster()
    {
        if (targetMonster != null && targetMonster.activeSelf)
        {
            // Ÿ�� ���Ͱ� �����ϴ� ���

            SetMonsterTargetingUIActive(true);                       // ���� Ÿ���� UI Ȱ��ȭ
            monsterTargetingUI.SetMonsterTargetingUI(targetMonster); // Ÿ�� ���� �����ͷ� ���� Ÿ���� UI ����

            //SetTargetMarkTop(targetMonster.transform);             // Ÿ�� ���� ���� Ÿ�� ��ũ ����
            SetTargetMarkBottom(targetMonster.transform);            // Ÿ�� ���� �Ʒ��� Ÿ�� ��ũ ����
        }
        else
        {
            // Ÿ�� ���Ͱ� �������� �ʴ� ���

            SetMonsterTargetingUIActive(false); // ���� Ÿ���� UI Ȱ��ȭ
            SetTargetMarkBottomActive(false);   // Ÿ�� ��ũ(�Ǝm) ��Ȱ��ȭ
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab Ű�� ������ ���
            // �÷��̾�� ���� ����� ���͸� Ÿ������ ����
            targetMonster = GetTargetMonsterNear();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // ���콺�� Ŭ������ ���
            // Ŭ���� ���͸� Ÿ������ ����
            targetMonster = GetTargetMonsterClick();
        }
        else if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            // ` Ű�� ������ ���
            // Ÿ���� ����
            targetMonster = null;
        }
    }

    // �þ߰� �ȿ� ���� ���͵� ���ϱ�
    private List<Transform> GetFieldOfViewMonsters()
    {
        List<Transform> fieldOfViewMonsters = new List<Transform>(); // �þ߰� ���� ���� ���

        // ���� �ȿ� ���� ������Ʈ �� ������ ��츸 �޾ƿ�
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewAreaRadius, monsterMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Transform monster = colliders[i].transform;

            // ���� ���� ���� ���Ͱ� �þ� �ȿ� ���Դ��� Ȯ�� �� Ÿ�� ���� ��Ͽ� ����
            Vector3 direction = monster.position - transform.position;
            direction.y = 0;

            // ������ �Ÿ� ���� (���� �߽��� ���� �ȿ� ������ ��)
            if (GetMonsterDistance(monster.transform) <= viewAreaRadius && Vector3.Dot(direction.normalized, transform.forward) > CalcAngle(viewAreaAngle / 2).z)
            {
                if (monster != null) fieldOfViewMonsters.Add(monster);
            }
        }

        return fieldOfViewMonsters;
    }

    // ���� ����� Ÿ�� ���� ���ϱ�
    private GameObject GetTargetMonsterNear()
    {
        List<Transform> fieldOfViewMonsters = GetFieldOfViewMonsters();

        GameObject targetMonsterNear = null;

        // �ּڰ� ���ϱ� �˰���
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

    // ���콺 Ŭ���� Ÿ�� ���� ���ϱ�
    private GameObject GetTargetMonsterClick()
    {
        GameObject targetMonsterClick = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���콺�� Ŭ���� ������Ʈ�� ������ ���
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterMask))
        {
            GameObject clickMonster = hit.transform.gameObject;

            Vector3 direction = clickMonster.transform.position - transform.position;
            direction.y = 0;

            // ���Ͱ� �þ� ���� ���� ���� ���
            if (GetMonsterDistance(clickMonster.transform) <= viewAreaRadius && Vector3.Dot(direction.normalized, transform.forward) > CalcAngle(viewAreaAngle / 2).z)
            {
                targetMonsterClick = clickMonster;
            }
        }

        return targetMonsterClick;
    }

    // Ÿ�� ���� ���� Ÿ�� ��ũ ����
    private void SetTargetMarkTop(Transform monsterTransform)
    {
        SetTargetMarkTopActive(true);

        // �ش� ������ ���� (y�� ����) ���ϱ�
        BoxCollider boxCollider = monsterTransform.gameObject.GetComponent<BoxCollider>();
        float obejctHeight = boxCollider.bounds.size.y;

        // ���� �߽ɰ� Ÿ�� ��ũ ������ �Ÿ�
        float targetMarkDistance = obejctHeight / 2.0f + 1.0f;

        // Ÿ�� ��ũ ��ġ�� ���� �Ӹ� �������� ����
        Vector3 markPosition = new Vector3(monsterTransform.position.x, monsterTransform.position.y + targetMarkDistance, monsterTransform.position.z);
        targetMarkTop.transform.position = markPosition;
    }

    // Ÿ�� ���� �Ʒ��� Ÿ�� ��ũ ����
    private void SetTargetMarkBottom(Transform monsterTransform)
    {
        SetTargetMarkBottomActive(true);

        // �ش� ������ �ʺ� (x, z�� �� �� ����) ���ϱ�
        BoxCollider boxCollider = monsterTransform.gameObject.GetComponent<BoxCollider>();
        float obejctWidth = Mathf.Max(boxCollider.bounds.size.x, boxCollider.bounds.size.z);
        float yPosition = monsterTransform.position.y - boxCollider.bounds.size.y / 2.0f + 1.25f;

        // Ÿ�� ��ũ ��ġ�� ���� �� ������ ����
        Vector3 markPosition = new Vector3(monsterTransform.position.x, yPosition, monsterTransform.position.z);
        targetMarkBottom.transform.position = markPosition;

        // Ÿ�� ��ũ ũ�� ���� (�θ� ��ü�� ���� �þ ��ŭ ���������μ� ���� ����)
        targetMarkBottom.transform.localScale = new Vector3(obejctWidth / monsterTransform.localScale.x, 1, obejctWidth / monsterTransform.localScale.z);
    }

    public void SetMonsterTargetingUIActive(bool active)
    {
        monsterTargetingUIObject.SetActive(active);
    }

    // Ÿ�� ��ũ(��) Ȱ��ȭ ����
    public void SetTargetMarkTopActive(bool active)
    {
        targetMarkTop.gameObject.SetActive(active);
    }

    // Ÿ�� ��ũ(�Ʒ�) Ȱ��ȭ ����
    public void SetTargetMarkBottomActive(bool active)
    {
        targetMarkBottom.gameObject.SetActive(active);
    }

    // �þ� ����� �׸���
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;

        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, viewAreaRadius);

        Vector3 viewLeft = transform.position + CalcAngle(-viewAreaAngle / 2 + transform.rotation.eulerAngles.y) * viewAreaRadius;
        Vector3 viewRight = transform.position + CalcAngle(viewAreaAngle / 2 + transform.rotation.eulerAngles.y) * viewAreaRadius;

        Handles.DrawLine(transform.position, viewLeft);
        Handles.DrawLine(transform.position, viewRight);
    }

    // ���� ���
    private Vector3 CalcAngle(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;

        Vector3 angle = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));

        return angle;
    }

    // �÷��̾�� ���� ������ �Ÿ� ���
    private float GetMonsterDistance(Transform monsterTransform)
    {
        Vector3 player = transform.position;
        Vector3 monster = monsterTransform.position;

        player.y = 0;
        monster.y = 0;

        return Vector3.Distance(player, monster);
    }
}
