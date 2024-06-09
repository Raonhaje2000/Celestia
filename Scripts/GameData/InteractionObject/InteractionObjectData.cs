using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjectData : ScriptableObject
{
    [SerializeField] string objectName;        // ��ȣ�ۿ� ������Ʈ �̸�                                                      

    [SerializeField] bool interactionPossible; // ��ȣ�ۿ� ���� ����

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
