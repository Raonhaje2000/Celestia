using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossMonster", menuName = "GameData/Monster/BossMonster")]                                   
public class BossMonsterData : MonsterData
{
    public BossMonsterData()
    {
        base.type = MonsterType.Boss;
        base.sensingMaxRange = 50.0f;
    }
}
