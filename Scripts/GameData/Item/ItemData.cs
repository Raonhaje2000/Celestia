using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : ScriptableObject
{
    [SerializeField] Sprite icon;        // ������ ������                                                

    [SerializeField] int itemId;         // ������ ID
    [SerializeField] string itemName;    // ������ �̸�

    [SerializeField] string tooltip;     // ������ ����

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
