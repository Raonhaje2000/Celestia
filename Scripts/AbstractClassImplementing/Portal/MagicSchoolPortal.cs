using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSchoolPortal : Portal
{
    // ��Ż�� ������(���ڿ�)�� ��ȯ
    public override string GetDestination()
    {
        return "���� �б�";
    }

    // ��Ż�� �������� �̵�
    public override void MoveDestination()
    {
        GameManager.instance.LoadDestinationMap(GameManager.Map.MagicSchool); // ���� �б� ������ ����                              
    }
}