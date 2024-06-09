using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("QuestDetailObject").GetComponent<QuestDetailUI>().SetQuestDetail(QuestManager.instance.GetProgressMainQuest(), false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
