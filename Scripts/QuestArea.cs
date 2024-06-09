using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestArea : MonoBehaviour
{
    [SerializeField]
    List<QuestData> questData;

    [Min(1)]
    [SerializeField] float range;

    LayerMask playerMask;

    void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if(IsPlayerInArea()) CheckProgressQuest();
    }

    bool IsPlayerInArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, playerMask);

        if (colliders.Length != 0) return true;
        return false;
    }

    void CheckProgressQuest()
    {
        QuestData mainQuest = QuestManager.instance.GetProgressMainQuest();
        List<QuestData> subQuests = QuestManager.instance.GetProgressSubQuests();

        for (int i = 0; i < questData.Count; i++)
        {
            if (mainQuest != null)
            {
                if (questData[i].QuestID == mainQuest.QuestID) mainQuest.IsConditionComplete = true;                                
            }

            for (int j = 0; j < subQuests.Count; j++)
            {
                if (questData[i].QuestID == subQuests[j].QuestID) subQuests[j].IsConditionComplete = true;                       
            }
        }
    }

    // 상호작용 범위 기즈모 그리기
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;

        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, range);
    }
}
