using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyForestPortal : Portal
{
    // ��Ż�� ������(���ڿ�)�� ��ȯ
    public override string GetDestination()
    {
        return "��� ��";
    }

    // ��Ż�� �������� �̵�
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.ValleyForest); // ��� �� ������ ����                               
    }
}