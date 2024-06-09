using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    /// <summary>
    /// 몬스터가 플레이어 스킬로 입은 피해 처리<br/>
    /// 스킬과의 충돌 처리를 할 때 호출함
    /// </summary>
    /// <param name="skillDamageValue"> 몬스터가 맞은 스킬의 데미지 </param>                                              
    public abstract void DamagedPlayerSkill(float skillDamageValue);

    /// <summary>
    /// 몬스터가 주는 경험치 및 재화 드랍 처리<br/>
    /// 몬스터가 죽었을 때 호출함
    /// </summary>
    protected abstract void DropExpAndGold();

    /// <summary>
    /// 몬스터의 아이템 드랍(드랍 아이템 오브젝트 생성) 처리<br/>
    /// 정해진 드랍 확률에 따라 드랍 아이템을 생성<br/>
    /// 몬스터가 죽었을 때 호출함
    /// </summary>
    protected abstract void DropItems();

    
    /// <summary>
    /// 몬스터 데이터 반환
    /// </summary>
    /// <returns>해당 몬스터의 데이터</returns>
    public abstract MonsterData GetMonsterData();
}
