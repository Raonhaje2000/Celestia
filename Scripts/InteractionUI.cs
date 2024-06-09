using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    Image interactionIcon;              // ��ȣ�ۿ� ������
    
    TextMeshProUGUI interactionText;    // ��ȣ�ۿ� �ؽ�Ʈ
    TextMeshProUGUI interactionKeyText; // ��ȣ�ۿ� Ű �ؽ�Ʈ

    private void Awake()
    {
        // ���� ���ҽ� �ҷ�����
        LoadResources();
    }

    void Start()
    {
        // �ʱ�ȭ
        Initialize();
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<Image>();

        interactionText = GameObject.Find("InteractionText").GetComponent<TextMeshProUGUI>();
        interactionKeyText = GameObject.Find("InteractionKeyText").GetComponent<TextMeshProUGUI>();                           
    }

    // �ʱ�ȭ
    void Initialize()
    {
        interactionIcon.sprite = null;

        interactionText.text = "��ȣ�ۿ�";
        interactionKeyText.text = "[]";
    }

    // ��ȣ�ۿ� UI ������ ����
    public void SetInteractionIconUI(Sprite icon, string text, KeyCode keyCode)
    {
        interactionIcon.sprite = icon;

        interactionText.text = text;
        interactionKeyText.text = string.Format("[{0}]", keyCode.ToString());                                                                              
    }
}
