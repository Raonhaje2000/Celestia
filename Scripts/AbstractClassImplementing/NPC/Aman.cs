using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aman : Npc
{
    GameObject uiManager;
    GameObject shop;

    private void Awake()
    {
        uiManager = GameObject.Find("UiManager");
        shop = uiManager.transform.Find("ShopUI").gameObject;

        SetNpcData();
        LoadResources();
    }

    void Start()
    {
        SetNpcNameTag(npcData);

        ActiveQuestMinimapMark(false);
    }

    void Update()
    {
        RotateNameTag();
    }

    protected override void SetNpcData()
    {
        npcData = ScriptableObject.Instantiate(Resources.Load<NpcData>("GameData/Npc/Main/AmanData"));                    
    }

    public override void InteroperateWithPlayer()
    {
        shop.SetActive(true);                                                                                                       
    }
}
