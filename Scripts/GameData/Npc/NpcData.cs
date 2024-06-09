using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Npc", menuName = "GameData/Npc/Npc")]                                                      
public class NpcData : ScriptableObject
{
    public enum NpcLocation
    {
        Village = 0,      // ¸¶À»
        SacredTreeForest, // ½Å¼ö ½£
        TreesForest,      // ³ª¹« ½£
        ValleyForest,     // °è°î ½£
        CaveForest        // µ¿±¼ ½£
    }

    [SerializeField] int npcId;             // NPC ID

    [SerializeField] string npcAppellation; // NPC È£Äª
    [SerializeField] string npcName;        // NPC ÀÌ¸§

    [SerializeField] NpcLocation location;  // NPC À§Ä¡
    string loationString;                   // NPC À§Ä¡(¹®ÀÚ¿­)

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

    // È£Äª°ú ÀÌ¸§À» ÇÕÄ£ ¸íÄª
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

    // NPC À§Ä¡¸¦ ¹®ÀÚ¿­·Î ¹ÝÈ¯
    string LocationToString()
    {
        switch (location)
        {
            case NpcLocation.Village:          return "¸¶À»";
            case NpcLocation.SacredTreeForest: return "½Å¼ö ½£";
            case NpcLocation.TreesForest:      return "³ª¹« ½£";
            case NpcLocation.ValleyForest:     return "°è°î ½£";
            case NpcLocation.CaveForest:       return "µ¿±¼ ½£";
            default:                           return "";
        }
    }
}
