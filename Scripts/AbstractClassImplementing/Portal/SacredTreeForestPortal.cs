using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredTreeForestPortal : Portal
{
    // ��Ż�� ������(���ڿ�)�� ��ȯ
    public override string GetDestination()
    {
        return "�ż� ��";
    }

    // ��Ż�� �������� �̵�
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.SacredTreeForest); // �ż� �� ������ ����                           
    } 
}