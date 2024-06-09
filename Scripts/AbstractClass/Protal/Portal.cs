using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    /// <summary>
    /// 포탈의 목적지를 문자열로 받아오는 처리 </br>
    /// 포탈과 상호작용을 할 때의 UI 세팅할 때 호출
    /// </summary>
    /// <returns>포탈의 목적지(한글)</returns>
    public abstract string GetDestination();

    /// <summary>
    /// 포탈의 목적지로 씬을 이동하는 처리 </br>
    /// 플레이어가 상호작용 키를 눌렀을 때 호출함
    /// </summary>
    public abstract void MoveDestination();                                                                                      
}
