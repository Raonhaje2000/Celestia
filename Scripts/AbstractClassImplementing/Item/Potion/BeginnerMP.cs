using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerMP : PotionItem
{
    PotionItemData beginnerMP;      // �ʱ� ���� ȸ�� ���� ������ ������

    static float cooldownTimeMax;   // ���� ���ð� �ִ밪 (������ ��� ������ �ð�)
    static float cooldownTime;      // ���� ���ð�
    static bool isCooldownTime;     // ���� ���ð����� Ȯ���ϴ� �÷���

    private void Awake()
    {
        // �ش� �κ��丮 �������� ������ ����
        SetInventoryItemData();
    }

    // �ش� �κ��丮 �������� ������ ����
    public override void SetInventoryItemData()
    {
        beginnerMP = ScriptableObject.Instantiate(Resources.Load<PotionItemData>("GameData/Item/Potion/BeginnerMPData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return beginnerMP;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        BeginnerMP newBunndle = gameObject.AddComponent<BeginnerMP>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ���� ������ ���
    public override void UseProtionItem()
    {
        if (!UsePossible()) throw new System.Exception(beginnerMP.ItemName + " ��� �Ұ�");

        Debug.Log(beginnerMP.ItemName + " ������ ���");

        // ���� ���ð� �ʱ�ȭ �� ����
        cooldownTimeMax = beginnerMP.CooldownTime;
        cooldownTime = cooldownTimeMax;
        isCooldownTime = true;

        // ������ ȿ�� ó��
        // �������� ȸ�� �ۼ�Ʈ ��ġ��ŭ �÷��̾��� ���� ���� ȸ�� (ȸ�� ��ġ�� �Ҽ����� ����ó��)
        PlayerManager.instance.CurrentMp += Mathf.CeilToInt(PlayerManager.instance.MaxMp * beginnerMP.RecoveryPercentage / 100.0f);

        // ������ ���� ����
        beginnerMP.CurrentCount--;

        // �ش� ������ �������� ������ 0�� �Ǿ��� ���, �κ��丮 ������ ����
        if (beginnerMP.CurrentCount == 0) InventoryManager.instance.FindAndDeleteSlotItem();
    }

    // ���� �������� ��� �������� Ȯ��
    public override bool UsePossible()
    {
        // ���� ������ 1�� �̻��̰�, ���� ���ð��� �ƴϸ�, ������ ������� �÷��̾ ȸ�� �� �� �ִ� ��ġ�� �������� �� (�ִ� MP�� �ƴ� ��)
        if (beginnerMP.CurrentCount >= 1 && !isCooldownTime && PlayerManager.instance.CurrentMp < PlayerManager.instance.MaxMp) return true;
        else return false;
    }

    // ���� ���ð� ������Ʈ
    public override void UpdatePotionCoolDownTime(float deltaTime)
    {
        cooldownTime -= deltaTime;

        if (cooldownTime < 0) isCooldownTime = false;
    }

    // �ش� ���� ������ ������ ���� ���ð� ��ȯ
    public override float GetPotionCoolDownTime()
    {
        return cooldownTime;
    }

    public override bool IsPotionCoolDownTime()
    {
        return isCooldownTime;
    }
}