using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodsItme", menuName = "GameData/Item/GoodsItme")]                              
public class GoodsItmeData : ItemData
{
    [SerializeField] int maxAmount;     // �ִ� ���� ����
    [SerializeField] int currentAmount; // ���� ����

    public int MaxAmount
    {
        get { return maxAmount; }
    }

    public int CurrentAmount
    { 
        get { return currentAmount; }
        set
        { 
            // �ִ� �������� �Ѿ ��� �Ѿ ������ �Ҹ� ó��
            currentAmount = value;

            if (currentAmount > maxAmount) currentAmount = maxAmount;                                                  
        }
    }
}