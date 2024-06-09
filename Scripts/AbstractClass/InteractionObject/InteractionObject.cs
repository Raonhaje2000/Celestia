using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour                                                                   
{
    /// <summary>
    /// 플레이어와의 상호작용 동작 처리<br/>
    /// 플레이어가 상호작용 키를 눌렀을 때 호출함
    /// </summary>
    public abstract void InteroperateWithPlayer();
}
