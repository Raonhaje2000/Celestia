using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredTreeForestPortal : Portal
{
    // 포탈의 목적지(문자열)를 반환
    public override string GetDestination()
    {
        return "신수 숲";
    }

    // 포탈의 목적지로 이동
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.SacredTreeForest); // 신수 숲 씬으로 변경                           
    } 
}