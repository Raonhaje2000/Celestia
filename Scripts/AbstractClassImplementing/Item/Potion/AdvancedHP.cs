using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedHP : PotionItem
{
    PotionItemData advancedHP;      // ��� ü�� ȸ�� ���� ������ ������

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
        advancedHP = ScriptableObject.Instantiate(Resources.Load<PotionItemData>("GameData/Item/Potion/AdvancedHPData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return advancedHP;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        AdvancedHP newBunndle = gameObject.AddComponent<AdvancedHP>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ���� ������ ���
    public override void UseProtionItem()
    {
        if (!UsePossible()) throw new System.Exception(advancedHP.ItemName + " ��� �Ұ�");

        Debug.Log(advancedHP.ItemName + " ������ ���");

        // ���� ���ð� �ʱ�ȭ �� ����
        cooldownTimeMax = advancedHP.CooldownTime;
        cooldownTime = cooldownTimeMax;
        isCooldownTime = true;

        // ������ ȿ�� ó��
        // �������� ȸ�� �ۼ�Ʈ ��ġ��ŭ �÷��̾��� ���� ü�� ȸ�� (ȸ�� ��ġ�� �Ҽ����� ����ó��)
        PlayerManager.instance.CurrentHp += Mathf.CeilToInt(PlayerManager.instance.MaxHp * advancedHP.RecoveryPercentage / 100.0f);

        // ������ ���� ����
        advancedHP.CurrentCount--;

        // �ش� ������ �������� ������ 0�� �Ǿ��� ���, �κ��丮 ������ ����
        if (advancedHP.CurrentCount == 0) InventoryManager.instance.FindAndDeleteSlotItem();
    }

    // ���� �������� ��� �������� Ȯ��
    public override bool UsePossible()
    {
        // ���� ������ 1�� �̻��̰�, ���� ���ð��� �ƴϸ�, ������ ������� �÷��̾ ȸ�� �� �� �ִ� ��ġ�� �������� �� (�ִ� HP�� �ƴ� ��)
        if (advancedHP.CurrentCount >= 1 && !isCooldownTime && PlayerManager.instance.CurrentHp < PlayerManager.instance.MaxHp) return true;
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