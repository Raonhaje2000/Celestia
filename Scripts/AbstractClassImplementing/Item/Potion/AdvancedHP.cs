using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedHP : PotionItem
{
    PotionItemData advancedHP;      // 고급 체력 회복 물약 아이템 데이터

    static float cooldownTimeMax;   // 재사용 대기시간 최대값 (아이템 사용 직후의 시간)
    static float cooldownTime;      // 재사용 대기시간
    static bool isCooldownTime;     // 재사용 대기시간인지 확인하는 플래그

    private void Awake()
    {
        // 해당 인벤토리 아이템의 데이터 세팅
        SetInventoryItemData();
    }

    // 해당 인벤토리 아이템의 데이터 세팅
    public override void SetInventoryItemData()
    {
        advancedHP = ScriptableObject.Instantiate(Resources.Load<PotionItemData>("GameData/Item/Potion/AdvancedHPData"));
    }

    // 해당 인벤토리 아이템의 데이터 반환
    public override InventoryItemData GetInventoryItemData()
    {
        return advancedHP;
    }

    // 해당 인벤토리 아이템의 새로운 묶음(컴포넌트) 생성 후 반환
    public override InventoryItem CreateNewBunddle()
    {
        // 묶음 생성 후 아이템 데이터 세팅
        AdvancedHP newBunndle = gameObject.AddComponent<AdvancedHP>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // 포션 아이템 사용
    public override void UseProtionItem()
    {
        if (!UsePossible()) throw new System.Exception(advancedHP.ItemName + " 사용 불가");

        Debug.Log(advancedHP.ItemName + " 아이템 사용");

        // 재사용 대기시간 초기화 및 세팅
        cooldownTimeMax = advancedHP.CooldownTime;
        cooldownTime = cooldownTimeMax;
        isCooldownTime = true;

        // 아이템 효과 처리
        // 아이템의 회복 퍼센트 수치만큼 플레이어의 현재 체력 회복 (회복 수치의 소수점은 버림처리)
        PlayerManager.instance.CurrentHp += Mathf.CeilToInt(PlayerManager.instance.MaxHp * advancedHP.RecoveryPercentage / 100.0f);

        // 아이템 개수 감소
        advancedHP.CurrentCount--;

        // 해당 슬롯의 아이템의 개수가 0이 되었을 경우, 인벤토리 내에서 삭제
        if (advancedHP.CurrentCount == 0) InventoryManager.instance.FindAndDeleteSlotItem();
    }

    // 포션 아이템이 사용 가능한지 확인
    public override bool UsePossible()
    {
        // 보유 개수가 1개 이상이고, 재사용 대기시간이 아니며, 아이템 사용으로 플레이어가 회복 될 수 있는 수치가 남아있을 때 (최대 HP가 아닐 때)
        if (advancedHP.CurrentCount >= 1 && !isCooldownTime && PlayerManager.instance.CurrentHp < PlayerManager.instance.MaxHp) return true;
        else return false;
    }

    // 재사용 대기시간 업데이트
    public override void UpdatePotionCoolDownTime(float deltaTime)
    {
        cooldownTime -= deltaTime;

        if (cooldownTime < 0) isCooldownTime = false;
    }

    // 해당 포션 아이템 종류의 재사용 대기시간 반환
    public override float GetPotionCoolDownTime()
    {
        return cooldownTime;
    }

    public override bool IsPotionCoolDownTime()
    {
        return isCooldownTime;
    }
}