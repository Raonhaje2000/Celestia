using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesForest_ToDungeon : Portal
{
    public override string GetDestination()
    {
        return "���� �� ����";
    }

    // ��Ż�� �������� �̵�
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.TreesForestDungeon); // ���� �� ������ ����
    }
}
