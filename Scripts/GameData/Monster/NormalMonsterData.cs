using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalMonster", menuName = "GameData/Monster/NormalMonster")]                              
public class NormalMonsterData : MonsterData
{
    public NormalMonsterData()
    {
        base.type = MonsterType.Normal;
    }
}
