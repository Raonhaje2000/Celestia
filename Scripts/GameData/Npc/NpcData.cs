using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Npc", menuName = "GameData/Npc/Npc")]                                                      
public class NpcData : ScriptableObject
{
    public enum NpcLocation
    {
        Village = 0,      // ����
        SacredTreeForest, // �ż� ��
        TreesForest,      // ���� ��
        ValleyForest,     // ��� ��
        CaveForest        // ���� ��
    }

    [SerializeField] int npcId;             // NPC ID

    [SerializeField] string npcAppellation; // NPC ȣĪ
    [SerializeField] string npcName;        // NPC �̸�

    [SerializeField] NpcLocation location;  // NPC ��ġ
    string loationString;                   // NPC ��ġ(���ڿ�)

    public int NpcId
    {
        get { return npcId; }
    }

    public string NpcAppellation
    {
        get { return npcAppellation; }
    }

    public string NpcName
    {
        get { return npcName; }
    }

    // ȣĪ�� �̸��� ��ģ ��Ī
    public string NpcFullName
    {
        get { return npcAppellation + " " + npcName; }
    }

    public NpcLocation Location
    {
        get { return location; }
    }

    public string LoationString
    {
        get
        {
            if (loationString == "") loationString = LocationToString();

            return loationString;
        }
    }

    // NPC ��ġ�� ���ڿ��� ��ȯ
    string LocationToString()
    {
        switch (location)
        {
            case NpcLocation.Village:          return "����";
            case NpcLocation.SacredTreeForest: return "�ż� ��";
            case NpcLocation.TreesForest:      return "���� ��";
            case NpcLocation.ValleyForest:     return "��� ��";
            case NpcLocation.CaveForest:       return "���� ��";
            default:                           return "";
        }
    }
}
