using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    Image interactionIcon;              // 상호작용 아이콘
    
    TextMeshProUGUI interactionText;    // 상호작용 텍스트
    TextMeshProUGUI interactionKeyText; // 상호작용 키 텍스트

    private void Awake()
    {
        // 관련 리소스 불러오기
        LoadResources();
    }

    void Start()
    {
        // 초기화
        Initialize();
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<Image>();

        interactionText = GameObject.Find("InteractionText").GetComponent<TextMeshProUGUI>();
        interactionKeyText = GameObject.Find("InteractionKeyText").GetComponent<TextMeshProUGUI>();                           
    }

    // 초기화
    void Initialize()
    {
        interactionIcon.sprite = null;

        interactionText.text = "상호작용";
        interactionKeyText.text = "[]";
    }

    // 상호작용 UI 아이콘 세팅
    public void SetInteractionIconUI(Sprite icon, string text, KeyCode keyCode)
    {
        interactionIcon.sprite = icon;

        interactionText.text = text;
        interactionKeyText.text = string.Format("[{0}]", keyCode.ToString());                                                                              
    }
}
