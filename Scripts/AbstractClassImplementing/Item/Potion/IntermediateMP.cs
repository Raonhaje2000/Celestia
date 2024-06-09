using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateMP : PotionItem
{
    PotionItemData intermediateMP;  // �߱� ���� ȸ�� ���� ������ ������

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
        intermediateMP = ScriptableObject.Instantiate(Resources.Load<PotionItemData>("GameData/Item/Potion/IntermediateMPData"));
    }

    // �ش� �κ��丮 �������� ������ ��ȯ
    public override InventoryItemData GetInventoryItemData()
    {
        return intermediateMP;
    }

    // �ش� �κ��丮 �������� ���ο� ����(������Ʈ) ���� �� ��ȯ
    public override InventoryItem CreateNewBunddle()
    {
        // ���� ���� �� ������ ������ ����
        IntermediateMP newBunndle = gameObject.AddComponent<IntermediateMP>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // ���� ������ ���
    public override void UseProtionItem()
    {
        if (!UsePossible()) throw new System.Exception(intermediateMP.ItemName + " ��� �Ұ�");

        Debug.Log(intermediateMP.ItemName + " ������ ���");

        // ���� ���ð� �ʱ�ȭ �� ����
        cooldownTimeMax = intermediateMP.CooldownTime;
        cooldownTime = cooldownTimeMax;
        isCooldownTime = true;

        // ������ ȿ�� ó��
        // �������� ȸ�� �ۼ�Ʈ ��ġ��ŭ �÷��̾��� ���� ���� ȸ�� (ȸ�� ��ġ�� �Ҽ����� ����ó��)
        PlayerManager.instance.CurrentMp += Mathf.CeilToInt(PlayerManager.instance.MaxMp * intermediateMP.RecoveryPercentage / 100.0f);

        // ������ ���� ����
        intermediateMP.CurrentCount--;

        // �ش� ������ �������� ������ 0�� �Ǿ��� ���, �κ��丮 ������ ����
        if (intermediateMP.CurrentCount == 0) InventoryManager.instance.FindAndDeleteSlotItem();
    }

    // ���� �������� ��� �������� Ȯ��
    public override bool UsePossible()
    {
        // ���� ������ 1�� �̻��̰�, ���� ���ð��� �ƴϸ�, ������ ������� �÷��̾ ȸ�� �� �� �ִ� ��ġ�� �������� �� (�ִ� MP�� �ƴ� ��)
        if (intermediateMP.CurrentCount >= 1 && !isCooldownTime && PlayerManager.instance.CurrentMp < PlayerManager.instance.MaxMp) return true;
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