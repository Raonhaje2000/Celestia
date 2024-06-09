using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicalRock : InteractionObject
{
    [SerializeField]
    MagicalRockData data;          // ������ ������                                                                                     
    
    GameObject diamond;            // ������ �߾��� ���� (�������� Ȱ�� �����϶��� ����)

    SkinnedMeshRenderer _renderer; // �ش� �������� ������

    Material active;               // Ȱ��ȭ ������ ���� ������ ����
    Material inactive;             // ��Ȱ��ȭ ������ ���� ������ ����

    GameObject systemUIObject;     // �ý��� UI ������Ʈ (��ȣ�ۿ� �� �ߴ� �˸� �޼��� â)
    TextMeshProUGUI systemText;    // �ý��� �ؽ�Ʈ (��ȣ�ۿ� �� �ߴ� �˸� �޼��� ����)

    bool isWaitSystemDelay;


    private void Awake()
    {
        diamond = transform.Find("Diamond").gameObject;

        _renderer = transform.Find("MRock_LOD0").gameObject.GetComponent<SkinnedMeshRenderer>();                                   

        active = Resources.Load<Material>("Materials/MagicalRock/MagicalRock_Active");
        inactive = Resources.Load<Material>("Materials/MagicalRock/MagicalRock_Inactive");
    }

    void Start()
    {
        systemUIObject = GameObject.Find("Canvas_InteractionUI(Clone)").transform.Find("SystemUIObject").gameObject;
        systemText = systemUIObject.transform.Find("SystemBackground/SystemText").gameObject.GetComponent<TextMeshProUGUI>();      

        // �������� ��ȣ�ۿ� ���� ���ο� ���� ���� ���� (�ٸ� ������ �Ѿ�͵� �����Ǳ� ����)                                       
        _renderer.material = (data.InteractionPossible) ? active : inactive;
        diamond.SetActive(data.InteractionPossible);
    }

    // �÷��̾���� ��ȣ�ۿ� �� ���� ó��
    public override void InteroperateWithPlayer()
    {
        // �ý��� UI ������Ʈ Ȱ��ȭ
        systemUIObject.SetActive(true);

        if (data.InteractionPossible)
        {
            // �������� ��ȣ�ۿ� ������ �� (Ȱ��ȭ ������ ��)
            data.InteractionPossible = false;

            diamond.SetActive(false);
            _renderer.material = inactive;

            // ��ų ����Ʈ ȹ�� ����
            //Debug.Log(data.ObjectName + "���� ��ų����Ʈ ȹ��");

            systemText.text = string.Format("[System] \'{0}\'���� ��ų ����Ʈ {1} ȹ��", data.ObjectName, data.SkillPoint);        

            PlayerManager.instance.CurrentSkillPoint++;
        }
        else
        {
            // �������� ��ȣ�ۿ� �Ұ��� �� �� (��Ȱ��ȭ ������ ��)

            // �̹� ����� ��� ����
            //Debug.Log("�̹� ����� ������");
            systemText.text = "[System] �̹� ������ �������Դϴ�.";
        }

        // �ý��� ������ ����� ������ ������
        StartCoroutine(WaitSystemDelay());
    }

    // �ý��� ������ ����� ������ ������
    IEnumerator WaitSystemDelay()
    {
        if(!isWaitSystemDelay)
        {
            isWaitSystemDelay = true;

            yield return new WaitForSeconds(2.0f);

            systemUIObject.SetActive(false);

            isWaitSystemDelay = false;

            StopCoroutine("WaitDelay");
        }
    }
}
