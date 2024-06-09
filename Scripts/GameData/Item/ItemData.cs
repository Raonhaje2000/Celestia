using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : ScriptableObject
{
    [SerializeField] Sprite icon;        // 아이템 아이콘                                                

    [SerializeField] int itemId;         // 아이템 ID
    [SerializeField] string itemName;    // 아이템 이름

    [SerializeField] string tooltip;     // 아이템 툴팁

    public Sprite Icon
    {
        get { return icon; }
    }

    public int ItemId
    { 
        get { return itemId; }
    }

    public string ItemName
    { 
        get { return itemName; }
    }

    public string Tooltip
    {
        get { return tooltip; }
    }
}
