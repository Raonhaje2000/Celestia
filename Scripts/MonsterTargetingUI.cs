using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterTargetingUI : MonoBehaviour
{
    Slider monsterHpBar;              // 몬스터 HP 바

    TextMeshProUGUI monsterHpText;    // 몬스터 HP 텍스트
    TextMeshProUGUI monsterTypeText;  // 몬스터 타입 텍스트
    TextMeshProUGUI monsterLevelText; // 몬스터 레벨 텍스트
    TextMeshProUGUI monsterNameText;  // 몬스터 이름 텍스트

    private void Awake()
    {
        // 관련 리소스 불러오기
        LoadResources();
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        monsterHpBar = transform.Find("MonsterTargetingBackground/MonsterHpBar").GetComponent<Slider>();

        monsterHpText = transform.Find("MonsterTargetingBackground/MonsterHpBar/MonsterHpText").GetComponent<TextMeshProUGUI>();
        monsterTypeText = transform.Find("MonsterTargetingBackground/MonsterType/MonsterTypeText").GetComponent<TextMeshProUGUI>();
        monsterLevelText = transform.Find("MonsterTargetingBackground/MonsterLevel/MonsterLevelText").GetComponent<TextMeshProUGUI>();
        monsterNameText = transform.Find("MonsterTargetingBackground/MonsterNameText").GetComponent<TextMeshProUGUI>();
    }

    // 몬스터 정보를 받아와 몬스터 타겟팅 UI 세팅
    public void SetMonsterTargetingUI(GameObject monster)
    {
        // 타겟팅 된 몬스터의 데이터
        MonsterData monsterData = monster.GetComponent<Monster>().GetMonsterData();

        if (monsterData != null)
        {
            monsterHpBar.maxValue = monsterData.Hp;
            monsterHpBar.minValue = 0.0f;
            monsterHpBar.value = monsterData.CurrentHp;

            monsterHpText.text = string.Format("{0} / {1}", Mathf.FloorToInt(monsterData.CurrentHp), Mathf.CeilToInt(monsterData.Hp));
            monsterTypeText.text = (monsterData.Type == MonsterData.MonsterType.Normal) ? "일반" : "보스";
            monsterLevelText.text = monsterData.Level.ToString();
            monsterNameText.text = monsterData.MonsterName;
        }
    }
}
