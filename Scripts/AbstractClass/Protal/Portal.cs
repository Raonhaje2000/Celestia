using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    /// <summary>
    /// ��Ż�� �������� ���ڿ��� �޾ƿ��� ó�� </br>
    /// ��Ż�� ��ȣ�ۿ��� �� ���� UI ������ �� ȣ��
    /// </summary>
    /// <returns>��Ż�� ������(�ѱ�)</returns>
    public abstract string GetDestination();

    /// <summary>
    /// ��Ż�� �������� ���� �̵��ϴ� ó�� </br>
    /// �÷��̾ ��ȣ�ۿ� Ű�� ������ �� ȣ����
    /// </summary>
    public abstract void MoveDestination();                                                                                      
}
