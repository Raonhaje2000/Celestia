using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTable", menuName = "GameData/Table/LevelTable")]
public class LevelTableData : ScriptableObject
{
    public const int MAX_LEVEL = 20;                                 // �ִ� ����

    [Min(0)]
    [SerializeField] float[] maxExpTable = new float[MAX_LEVEL + 1]; // �ִ� ����ġ ���̺� (���� ���� ����)

    public float[] MaxExpTable
    {
        get { return maxExpTable; }
    }
}
