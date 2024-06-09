using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoodsItme", menuName = "GameData/Item/GoodsItme")]                              
public class GoodsItmeData : ItemData
{
    [SerializeField] int maxAmount;     // 최대 보유 수량
    [SerializeField] int currentAmount; // 보유 수량

    public int MaxAmount
    {
        get { return maxAmount; }
    }

    public int CurrentAmount
    { 
        get { return currentAmount; }
        set
        { 
            // 최대 보유량을 넘어선 경우 넘어간 수량은 소멸 처리
            currentAmount = value;

            if (currentAmount > maxAmount) currentAmount = maxAmount;                                                  
        }
    }
}