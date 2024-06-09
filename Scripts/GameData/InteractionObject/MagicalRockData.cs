using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicalRockData", menuName = "GameData/InteractionObject/MagicalRockData")]                  
public class MagicalRockData : InteractionObjectData
{
    [SerializeField] int skillPoint; // ��ȣ�ۿ� �� ���� �� �ִ� ��ų ����Ʈ

    public int SkillPoint
    {
        get { return skillPoint; }
    }
}
