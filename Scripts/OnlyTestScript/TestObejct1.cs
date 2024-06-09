using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObejct1 : InteractionObject
{
    string objectName = "테스트 오브젝트 1";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void InteroperateWithPlayer()
    {
        Debug.Log(objectName);
    }

}
