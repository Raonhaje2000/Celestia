using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterTargetingUI : MonoBehaviour
{
    Slider monsterHpBar;              // ���� HP ��

    TextMeshProUGUI monsterHpText;    // ���� HP �ؽ�Ʈ
    TextMeshProUGUI monsterTypeText;  // ���� Ÿ�� �ؽ�Ʈ
    TextMeshProUGUI monsterLevelText; // ���� ���� �ؽ�Ʈ
    TextMeshProUGUI monsterNameText;  // ���� �̸� �ؽ�Ʈ

    private void Awake()
    {
        // ���� ���ҽ� �ҷ�����
        LoadResources();
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        monsterHpBar = transform.Find("MonsterTargetingBackground/MonsterHpBar").GetComponent<Slider>();

        monsterHpText = transform.Find("MonsterTargetingBackground/MonsterHpBar/MonsterHpText").GetComponent<TextMeshProUGUI>();
        monsterTypeText = transform.Find("MonsterTargetingBackground/MonsterType/MonsterTypeText").GetComponent<TextMeshProUGUI>();
        monsterLevelText = transform.Find("MonsterTargetingBackground/MonsterLevel/MonsterLevelText").GetComponent<TextMeshProUGUI>();
        monsterNameText = transform.Find("MonsterTargetingBackground/MonsterNameText").GetComponent<TextMeshProUGUI>();
    }

    // ���� ������ �޾ƿ� ���� Ÿ���� UI ����
    public void SetMonsterTargetingUI(GameObject monster)
    {
        // Ÿ���� �� ������ ������
        MonsterData monsterData = monster.GetComponent<Monster>().GetMonsterData();

        if (monsterData != null)
        {
            monsterHpBar.maxValue = monsterData.Hp;
            monsterHpBar.minValue = 0.0f;
            monsterHpBar.value = monsterData.CurrentHp;

            monsterHpText.text = string.Format("{0} / {1}", Mathf.FloorToInt(monsterData.CurrentHp), Mathf.CeilToInt(monsterData.Hp));
            monsterTypeText.text = (monsterData.Type == MonsterData.MonsterType.Normal) ? "�Ϲ�" : "����";
            monsterLevelText.text = monsterData.Level.ToString();
            monsterNameText.text = monsterData.MonsterName;
        }
    }
}
