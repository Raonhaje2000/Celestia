using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTable", menuName = "GameData/Table/LevelTable")]
public class LevelTableData : ScriptableObject
{
    public const int MAX_LEVEL = 20;                                 // 최대 레벨

    [Min(0)]
    [SerializeField] float[] maxExpTable = new float[MAX_LEVEL + 1]; // 최대 경험치 테이블 (현재 레벨 기준)

    public float[] MaxExpTable
    {
        get { return maxExpTable; }
    }
}
