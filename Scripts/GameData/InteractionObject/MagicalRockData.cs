using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicalRockData", menuName = "GameData/InteractionObject/MagicalRockData")]                  
public class MagicalRockData : InteractionObjectData
{
    [SerializeField] int skillPoint; // 상호작용 시 얻을 수 있는 스킬 포인트

    public int SkillPoint
    {
        get { return skillPoint; }
    }
}
