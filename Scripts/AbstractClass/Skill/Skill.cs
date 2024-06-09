using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour                                                                               
{
    /// <summary>
    /// 스킬을 사용할 때의 동작 처리
    /// </summary>
    public abstract void UseSkill();
}
