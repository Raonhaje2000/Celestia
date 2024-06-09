using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicalRock : InteractionObject
{
    [SerializeField]
    MagicalRockData data;          // 마법석 데이터                                                                                     
    
    GameObject diamond;            // 마법석 중앙의 보석 (마법석이 활성 상태일때만 존재)

    SkinnedMeshRenderer _renderer; // 해당 마법석의 랜더러

    Material active;               // 활성화 상태일 때의 마법석 재질
    Material inactive;             // 비활성화 상태일 때의 마법석 재질

    GameObject systemUIObject;     // 시스템 UI 오브젝트 (상호작용 시 뜨는 알림 메세지 창)
    TextMeshProUGUI systemText;    // 시스템 텍스트 (상호작용 시 뜨는 알림 메세지 내용)

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

        // 마법석의 상호작용 가능 여부에 따라 상태 변경 (다른 씬에서 넘어와도 유지되기 위함)                                       
        _renderer.material = (data.InteractionPossible) ? active : inactive;
        diamond.SetActive(data.InteractionPossible);
    }

    // 플레이어와의 상호작용 시 동작 처리
    public override void InteroperateWithPlayer()
    {
        // 시스템 UI 오브젝트 활성화
        systemUIObject.SetActive(true);

        if (data.InteractionPossible)
        {
            // 마법석이 상호작용 가능할 때 (활성화 상태일 때)
            data.InteractionPossible = false;

            diamond.SetActive(false);
            _renderer.material = inactive;

            // 스킬 포인트 획득 문구
            //Debug.Log(data.ObjectName + "에서 스킬포인트 획득");

            systemText.text = string.Format("[System] \'{0}\'에서 스킬 포인트 {1} 획득", data.ObjectName, data.SkillPoint);        

            PlayerManager.instance.CurrentSkillPoint++;
        }
        else
        {
            // 마법석이 상호작용 불가능 할 때 (비활성화 상태일 때)

            // 이미 조사된 대상 문구
            //Debug.Log("이미 조사된 마법석");
            systemText.text = "[System] 이미 조사한 마법석입니다.";
        }

        // 시스템 문구가 사라질 때까지 딜레이
        StartCoroutine(WaitSystemDelay());
    }

    // 시스템 문구가 사라질 때까지 딜레이
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
