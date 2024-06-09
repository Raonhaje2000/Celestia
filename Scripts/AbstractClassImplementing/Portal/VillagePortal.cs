using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePortal : Portal
{
    // 포탈의 목적지(문자열)를 반환
    public override string GetDestination()
    {
        return "마을";
    }

    // 포탈의 목적지로 이동
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.Village); // 마을 씬으로 변경                                      
    }
}