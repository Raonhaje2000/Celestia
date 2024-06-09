using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesForest_ToDungeon : Portal
{
    public override string GetDestination()
    {
        return "나무 숲 던전";
    }

    // 포탈의 목적지로 이동
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.TreesForestDungeon); // 나무 숲 씬으로 변경
    }
}
