using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionItem : InventoryItem
{
    /// <summary>
    /// 포션 아이템을 사용했을 때의 효과 처리<br/>
    /// 포션 아이템을 클릭했을 때 호출함
    /// </summary>
    public abstract void UseProtionItem();                                                              

    /// <summary>
    /// 포션 아이템이 사용 가능한지 확인</br>
    /// 포션 아이템을 클릭해 사용하기 전에 호출함</br>
    /// 보유 개수가 1개 이상이고, 재사용 대기시간이 아니며,
    /// 아이템 사용으로 플레이어가 회복 될 수 있는 수치가 남아있을 때 사용 가능                                               
    /// </summary>
    /// <returns>사용 가능한지 확인하는 플래그</returns>
    public abstract bool UsePossible();

    /// <summary>
    /// 재사용 대기시간 업데이트 처리</br>
    /// 재사용 대기시간 중인 아이템의 재사용 대기시간을 업데이트 할 때 호출함
    /// </summary>
    /// <param name="time">흐른 시간</param>
    /// <returns>변경된 재사용 대기시간</returns>
    public abstract void UpdatePotionCoolDownTime(float deltaTime);

    /// <summary>
    /// 해당 포션 아이템 종류의 재사용 대기시간 반환 처리</br>
    /// 해당 포션 아이템 종류의 재사용 대기시간을 확인할 때 호출함</br>
    /// (특히 슬롯 재사용 대기시간 텍스트 표기할 때)
    /// </summary>
    /// <returns>재사용 대기시간(초단위)</returns>
    public abstract float GetPotionCoolDownTime();

    /// <summary>
    /// 해당 포션 아이템 종류의 재사용 대기시간인지 확인
    /// 해당 포션 아이템 종류가 있는 슬롯들의 활성화 여부를 결정할 때 호출함
    /// </summary>
    /// <returns>재사용 대기시간인지 확인하는 플래그</returns>
    public abstract bool IsPotionCoolDownTime();
}
