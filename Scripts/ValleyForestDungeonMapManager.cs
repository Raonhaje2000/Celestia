using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyForestDungeonMapManager : MonoBehaviour
{
    public static ValleyForestDungeonMapManager instance;

    GameObject portal;

    [SerializeField]
    GameObject dundeonClearUIObject;

    private void Awake()
    {
        if (instance == null) instance = this;

        portal = GameObject.Find("ValleyForestPortal");
        dundeonClearUIObject = GameObject.Find("DundeonClearUIObject");
    }

    void Start()
    {
        portal.SetActive(false);
        dundeonClearUIObject.SetActive(false);
    }

    public void ClearDungeon()
    {
        portal.SetActive(true);
        dundeonClearUIObject.SetActive(true);
        dundeonClearUIObject.GetComponent<DundeonClearUI>().SetRewardRandom();
    }
}
