using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerInteraction : MonoBehaviour
{
    GameObject interactionUICanvas;                    // ��ȣ�ۿ� ǥ�� UI ĵ����
    GameObject interactionUIObject;                    // ��ȣ�ۿ� ǥ�� UI ������Ʈ
    InteractionUI interactionUI;                       // ��ȣ�ۿ� ǥ�� UI ������Ʈ

    Sprite interactionNpc;                             // NPC ��ȣ�ۿ� ��������Ʈ
    string interactionNpcText;                         // NPC ��ȣ�ۿ� �ؽ�Ʈ

    Sprite interactionObject;                          // ������Ʈ ��ȣ�ۿ� ��������Ʈ
    string interactionObjectText;                      // ������Ʈ ��ȣ�ۿ� �ؽ�Ʈ

    Sprite interactionPortal;                          // ������Ʈ ��ȣ�ۿ� ��������Ʈ

    GameObject interactionTargetMark;                  // ��ȣ�ۿ� Ÿ�� ��ũ

    [Min(0)]
    [SerializeField] float interactionRange;           // ��ȣ�ۿ� ����

    LayerMask interactionMask;                         // ��ȣ�ۿ� ���̾� ����ũ

    GameObject targetObejct;                           // ��ȣ�ۿ��� Ÿ�� ������Ʈ

    [SerializeField] KeyCode interactionKey;           // ��ȣ�ۿ� Ű

    GameObject questDetailUICanvas;                    // NPC ����Ʈ UI ĵ����
    GameObject questDetailObject;                      // NPC ����Ʈ UI ������Ʈ
    QuestDetailUINpcInteraction qusetNpcUI;            // NPC ����Ʈ UI ������Ʈ

    GameObject systemUIObject;

    private void Awake()
    {
        LoadResources(); // ���� ������Ʈ �ҷ�����
    }

    void Start()
    {
        Initialize(); // �ʱ�ȭ
    }

    void Update()
    {
        // ��ȣ�ۿ��� Ÿ�� ������Ʈ ������Ʈ
        UpdateTargetObejct();

        if (targetObejct != null)
        {
            // Ÿ�� ������Ʈ�� �ִ� ���

            // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ ����
            //SetInteractionUIAndTargetMark();

            if (!GameManager.instance.IsPlayerInteractionWithNpc)
            {
                // Ÿ�� ������Ʈ�� ������ ���¿��� ��ȣ�ۿ� Ű�� �ش� ������Ʈ�� ���콺 Ŭ������ ���
                if (Input.GetKeyDown(interactionKey) || (Input.GetMouseButtonDown(0) && CheckMouseClickTargetObject()))                   
                {
                    // ��ȣ�ۿ� ���� ó��
                    GetInteraction();
                }
            }
        }
        else
        {
            // Ÿ�� ������Ʈ�� ���� ���
            // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ ��Ȱ��ȭ
            //SetInteractionUIAndTargetMarkActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.IsPlayerInteractionWithNpc)
        {
            // �÷��̾ NPC�� ��ȣ�ۿ��ϰ� ���� ���� ���

            if (targetObejct != null)
            {
                // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ ����
                SetInteractionUIAndTargetMark();
            }
            else
            {
                // Ÿ�� ������Ʈ�� ���� ���
                // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ ��Ȱ��ȭ
                SetInteractionUIAndTargetMarkActive(false);
            }
        }
        else
        {
            // �÷��̾ NPC�� ��ȣ�ۿ��ϰ� �ִ� ���
            // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ ��Ȱ��ȭ
            SetInteractionUIAndTargetMarkActive(false);
        }
    }

    // ���� ���ҽ� �ҷ�����
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

    // �ʱ�ȭ
    void Initialize()
    {
        interactionNpcText = "��ȭ�ϱ�";
        interactionObjectText = "�����ϱ�";

        interactionRange = 5.0f;

        interactionMask = LayerMask.GetMask(new string[] { "NPC", "InteractionObject", "Portal" });                                         

        targetObejct = null;

        interactionKey = KeyCode.G;

        SetInteractionUIAndTargetMarkActive(false);

        questDetailObject.SetActive(false);
        systemUIObject.SetActive(false);
    }

    // ��ȣ�ۿ� ���� ó��
    void GetInteraction()
    {
        // �÷��̾ �ش� ������Ʈ�� �ٶ󺸵��� ȸ��
        //transform.LookAt(new Vector3(targetObejct.transform.position.x, transform.position.y, targetObejct.transform.position.z));     

        // ���̾ ���� ��ȣ�ۿ� ���� ó��
        if (targetObejct.layer == LayerMask.NameToLayer("NPC"))
        {
            // Ÿ�� ������Ʈ�� ���̾ NPC�� ���
            // NPC���� ��ȣ�ۿ� ���� ó��
            targetObejct.GetComponent<Npc>().InteroperateWithPlayer();
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("InteractionObject"))
        {
            // Ÿ�� ������Ʈ�� ���̾ ��ȣ�ۿ� ������Ʈ�� ���
            // ������Ʈ���� ��ȣ�ۿ� ���� ó��
            targetObejct.GetComponent<InteractionObject>().InteroperateWithPlayer();
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("Portal"))
        {
            // Ÿ�� ������Ʈ�� ���̾ ��Ż�� ���
            // ��Ż���� ��ȣ�ۿ� ���� ó��
            targetObejct.GetComponent<Portal>().MoveDestination();
        }
    }

    // ��ȣ�ۿ��� Ÿ�� ������Ʈ ������Ʈ
    void UpdateTargetObejct()
    {
        targetObejct = GetRangeObjectNear();
    }

    // ��ȣ�ۿ� �����ȿ� �ִ� ������Ʈ�� ��ȯ
    List<Transform> GetRangeObjects()
    {
        List<Transform> rangeObjects = new List<Transform>(); // ���� ���� ��ȣ�ۿ� ������Ʈ ���

        // ���� �ȿ� ���� ������Ʈ �� ������ ��츸 �޾ƿ�
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactionMask);                      

        for(int i = 0; i < colliders.Length; i++)
        {
            // ������Ʈ�� �Ÿ� ���� (������Ʈ �߽��� ���� �ȿ� ������ ��)
            if (GetObjectDistance(colliders[i].transform) <= interactionRange)
                rangeObjects.Add(colliders[i].gameObject.transform);
        }

        return rangeObjects;
    }

    // ���� ����� ��ȣ�ۿ� Ÿ�� ������Ʈ ���ϱ�
    GameObject GetRangeObjectNear()
    {
        List<Transform> rangeObjects = GetRangeObjects();

        GameObject rangeObjectNear = null;

        // �ּڰ� ���ϱ� �˰���
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

    // ���콺�� Ŭ���� ������Ʈ�� ��ȣ�ۿ� Ÿ�� ������Ʈ���� Ȯ��
    bool CheckMouseClickTargetObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���콺�� Ŭ���� ������Ʈ�� ��ȣ�ۿ� �� �� �ִ� ������Ʈ�� ���
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionMask))                                                         
        {
            GameObject clickObject = hit.transform.gameObject;

            // Ÿ�� ������Ʈ�� ��ġ�ϴ��� Ȯ��
            if (clickObject == targetObejct) return true;
        }

        return false;
    }

    // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ ����
    void SetInteractionUIAndTargetMark()
    {
        // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ Ȱ��ȭ
        SetInteractionUIAndTargetMarkActive(true);

        SetTargetMark();    // Ÿ�� ��ũ ����
        SetInteractionUI(); // ��ȣ�ۿ� UI ����
    }

    // ��ȣ�ۿ� UI ����
    void SetInteractionUI()
    {
        // ���̾ ���� ��ȣ�ۿ� UI ����
        if (targetObejct.layer == LayerMask.NameToLayer("NPC"))
        {
            // Ÿ�� ������Ʈ�� ���̾ NPC�� ���
            // NPC�� ��ȣ�ۿ��� ���� ����
            interactionUI.SetInteractionIconUI(interactionNpc, interactionNpcText, interactionKey);
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("InteractionObject"))
        {
            // Ÿ�� ������Ʈ�� ���̾ ��ȣ�ۿ� ������Ʈ�� ���
            // ������Ʈ�� ��ȣ�ۿ��� ���� ����
            interactionUI.SetInteractionIconUI(interactionObject, interactionObjectText, interactionKey);
        }
        else if (targetObejct.layer == LayerMask.NameToLayer("Portal"))
        {
            // Ÿ�� ������Ʈ�� ���̾ ��Ż�� ���
            // ��Ż�� ��ȣ�ۿ��� ���� ����
            string temp = string.Format("\'{0}\'(��)�� �̵�", targetObejct.GetComponent<Portal>().GetDestination());
            interactionUI.SetInteractionIconUI(interactionPortal, temp, interactionKey);

            interactionTargetMark.SetActive(false); // Ÿ�� ��ũ ��Ȱ��ȭ
        }

        // ��ȣ�ۿ� UI�� �÷��̾� �Ӹ� ���� �̵�
        interactionUIObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2.0f, 0));
    }

    // Ÿ�� ��ũ ����
    void SetTargetMark()
    {
        // �ش� ������Ʈ�� �ʺ� (x, z�� �� �� ����) ���ϱ�
        BoxCollider boxCollider = targetObejct.gameObject.GetComponent<BoxCollider>();
        float obejctWidth = Mathf.Max(boxCollider.bounds.size.x, boxCollider.bounds.size.z);

        // Ÿ�� ������Ʈ�� �ڽ� ��ü�� ����
        interactionTargetMark.transform.parent = targetObejct.transform;

        // Ÿ�� ��ũ ��ġ�� Ÿ�� ������Ʈ �� ������ ����
        Vector3 markPosition = new Vector3(targetObejct.transform.position.x, 0.25f, targetObejct.transform.position.z);
        interactionTargetMark.transform.position = markPosition;

        // Ÿ�� ��ũ ũ�� ���� (�θ� ��ü�� ���� �þ ��ŭ ���������μ� ���� ����)
        Vector3 targetLocalScale = targetObejct.transform.localScale;
        interactionTargetMark.transform.localScale = new Vector3(obejctWidth / targetLocalScale.x, 1, obejctWidth / targetLocalScale.z);
    }

    // ��ȣ�ۿ� UI�� ��ȣ�ۿ� Ÿ�� ��ũ Ȱ��ȭ ����
    void SetInteractionUIAndTargetMarkActive(bool active)
    {
        interactionUIObject.SetActive(active);
        interactionTargetMark.SetActive(active);
    }

    // �÷��̾�� ������Ʈ ������ �Ÿ� ���
    private float GetObjectDistance(Transform objectTransform)
    {
        Vector3 player = transform.position;
        Vector3 obj = objectTransform.position;

        player.y = 0;
        obj.y = 0;

        return Vector3.Distance(player, obj);                                                                                           
    }

    // ��ȣ�ۿ� ���� ����� �׸���
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;

        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, interactionRange);
    }
}
