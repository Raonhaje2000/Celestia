using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMP : PotionItem
{
    PotionItemData advancedMP;      // 고급 마나 회복 물약 아이템 데이터

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
        advancedMP = ScriptableObject.Instantiate(Resources.Load<PotionItemData>("GameData/Item/Potion/AdvancedMPData"));
    }

    // 해당 인벤토리 아이템의 데이터 반환
    public override InventoryItemData GetInventoryItemData()
    {
        return advancedMP;
    }

    // 해당 인벤토리 아이템의 새로운 묶음(컴포넌트) 생성 후 반환
    public override InventoryItem CreateNewBunddle()
    {
        // 묶음 생성 후 아이템 데이터 세팅
        AdvancedMP newBunndle = gameObject.AddComponent<AdvancedMP>();

        newBunndle.SetInventoryItemData();

        return newBunndle;
    }

    // 포션 아이템 사용
    public override void UseProtionItem()
    {
        if (!UsePossible()) throw new System.Exception(advancedMP.ItemName + " 사용 불가");

        Debug.Log(advancedMP.ItemName + " 아이템 사용");

        // 재사용 대기시간 초기화 및 세팅
        cooldownTimeMax = advancedMP.CooldownTime;
        cooldownTime = cooldownTimeMax;
        isCooldownTime = true;

        // 아이템 효과 처리
        // 아이템의 회복 퍼센트 수치만큼 플레이어의 현재 마나 회복 (회복 수치의 소수점은 버림처리)
        PlayerManager.instance.CurrentMp += Mathf.CeilToInt(PlayerManager.instance.MaxMp * advancedMP.RecoveryPercentage / 100.0f);

        // 아이템 개수 감소
        advancedMP.CurrentCount--;

        // 해당 슬롯의 아이템의 개수가 0이 되었을 경우, 인벤토리 내에서 삭제
        if (advancedMP.CurrentCount == 0) InventoryManager.instance.FindAndDeleteSlotItem();
    }

    // 포션 아이템이 사용 가능한지 확인
    public override bool UsePossible()
    {
        // 보유 개수가 1개 이상이고, 재사용 대기시간이 아니며, 아이템 사용으로 플레이어가 회복 될 수 있는 수치가 남아있을 때 (최대 MP가 아닐 때)
        if (advancedMP.CurrentCount >= 1 && !isCooldownTime && PlayerManager.instance.CurrentMp < PlayerManager.instance.MaxMp) return true;
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