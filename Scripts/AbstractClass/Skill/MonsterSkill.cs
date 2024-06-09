using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSkill : Skill
{
    /// <summary>
    /// 해당 몬스터 스킬 데이터 반환<br/>
    /// 몬스터 스킬 데이터가 필요할 때 호출
    /// </summary>
    /// <returns>해당 몬스터 스킬 데이터 반환</returns>
    public abstract MonsterSkillData GetMonsterSkillData();                                                             

    /// <summary>
    /// 몬스터가 해당 스킬을 사용할 수 있는지<br/>
    /// 몬스터가 스킬을 사용하기 전 스킬 사용 가능 여부 판단할 때 호출
    /// </summary>
    /// <returns>해당 스킬 사용 가능 여부</returns>
    public abstract bool UseSkillPossible();
}
