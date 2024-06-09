using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSchoolPortal : Portal
{
    // 포탈의 목적지(문자열)를 반환
    public override string GetDestination()
    {
        return "마법 학교";
    }

    // 포탈의 목적지로 이동
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.MagicSchool); // 마법 학교 씬으로 변경                              
    }
}