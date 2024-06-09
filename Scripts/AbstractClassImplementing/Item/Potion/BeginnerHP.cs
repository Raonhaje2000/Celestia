using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerHP : PotionItem
{
    PotionItemData beginnerHP;      // �ʱ� ü�� ȸ�� ���� ������ ������

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
        beginnerHP = ScriptableObject.Instantiate(Resources.Load<PotionItemData>("GameData/Item/Potion/BeginnerHPData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return beginnerHP;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        BeginnerHP newBunndle = gameObject.AddComponent<BeginnerHP>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ���� ������ ���
    public override void UseProtionItem()
    {
        if (!UsePossible()) throw new System.Exception(beginnerHP.ItemName + " ��� �Ұ�");

        Debug.Log(beginnerHP.ItemName + " ������ ���");

        // ���� ���ð� �ʱ�ȭ �� ����
        cooldownTimeMax = beginnerHP.CooldownTime;
        cooldownTime = cooldownTimeMax;
        isCooldownTime = true;

        // ������ ȿ�� ó��
        // �������� ȸ�� �ۼ�Ʈ ��ġ��ŭ �÷��̾��� ���� ü�� ȸ�� (ȸ�� ��ġ�� �Ҽ����� ����ó��)
        PlayerManager.instance.CurrentHp += Mathf.CeilToInt(PlayerManager.instance.MaxHp * beginnerHP.RecoveryPercentage / 100.0f);

        // ������ ���� ����
        beginnerHP.CurrentCount--;

        // �ش� ������ �������� ������ 0�� �Ǿ��� ���, �κ��丮 ������ ����
        if (beginnerHP.CurrentCount == 0) InventoryManager.instance.FindAndDeleteSlotItem();
    }

    // ���� �������� ��� �������� Ȯ��
    public override bool UsePossible()
    {
        // ���� ������ 1�� �̻��̰�, ���� ���ð��� �ƴϸ�, ������ ������� �÷��̾ ȸ�� �� �� �ִ� ��ġ�� �������� �� (�ִ� HP�� �ƴ� ��)
        if (beginnerHP.CurrentCount >= 1 && !isCooldownTime && PlayerManager.instance.CurrentHp < PlayerManager.instance.MaxHp) return true;
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