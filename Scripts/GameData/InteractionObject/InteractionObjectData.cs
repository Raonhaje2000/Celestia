using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjectData : ScriptableObject
{
    [SerializeField] string objectName;        // 상호작용 오브젝트 이름                                                      

    [SerializeField] bool interactionPossible; // 상호작용 가능 여부

    public string ObjectName
    {
        get { return objectName; }
    }

    public bool InteractionPossible
    {
        get { return interactionPossible; }
        set { interactionPossible = value; }
    }
}
