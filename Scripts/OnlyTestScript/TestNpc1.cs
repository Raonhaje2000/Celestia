using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNpc1 : Npc
{
    string npcName = "Å×½ºÆ® NPC 1";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InteroperateWithPlayer()
    {
        Debug.Log(npcName);
    }

    protected override void SetNpcData()
    {
        throw new System.NotImplementedException();
    }
}
