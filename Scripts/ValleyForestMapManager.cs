using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyForestMapManager : MonoBehaviour
{
    public static ValleyForestMapManager instance;

    GameObject lupus;
    float lupusReGenerationTime;

    GameObject[] lupusGenPoints;

    [SerializeField] int lupusMaxGenCount;
    [SerializeField] int lupusCurrentGenCount;

    [SerializeField] float genRangeMax;

    bool isGenDelay;

    [SerializeField] Transform villagePortal;
    [SerializeField] Transform sacredTreeForestPortal;
    [SerializeField] Transform treesForestPortal;
    [SerializeField] Transform caveForestPortal;
    [SerializeField] Transform valleyForestDungeonPortal;

    Transform player;

    public int LupusCurrentGenCount
    {
        get { return lupusCurrentGenCount; }
        set { lupusCurrentGenCount = value; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;

        lupus = Resources.Load<GameObject>("Prefabs/Monster/Field03_ValleyForest/Lupus");
        lupusReGenerationTime = Resources.Load<MonsterData>("GameData/Monster/Field03_ValleyForest/LupusData").ReGenerationTime;

        lupusGenPoints = GameObject.FindGameObjectsWithTag("MonsterGenPoint");

        villagePortal = GameObject.Find("VillagePortal").transform;
        sacredTreeForestPortal = GameObject.Find("SacredTreeForestPortal").transform;
        treesForestPortal = GameObject.Find("TreesForestPortal").transform;
        caveForestPortal = GameObject.Find("CaveForestPortal").transform;
        valleyForestDungeonPortal = GameObject.Find("ValleyForestDungeonPortal").transform;

        player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        lupusMaxGenCount = 40;
        lupusCurrentGenCount = 0;

        genRangeMax = 40.0f;

        isGenDelay = false;

        for (int i = 0; i < lupusMaxGenCount; i++)
        {
            GenerateLupus();
        }

        SetPlayerPosition();
    }

    void Update()
    {
        if(lupusCurrentGenCount < lupusMaxGenCount)
        {
            StartCoroutine(WaitGenDelay());
        }
    }

    void GenerateLupus()
    {
        int index = Random.Range(0, lupusGenPoints.Length);

        float x = Random.Range(-genRangeMax, genRangeMax);
        float z = Random.Range(-genRangeMax, genRangeMax);

        GameObject clone = Instantiate(lupus, lupusGenPoints[index].transform);
        clone.transform.position += new Vector3(x, 0, z);

        lupusCurrentGenCount++;
    }

    IEnumerator WaitGenDelay()
    {
        if(!isGenDelay)
        {
            isGenDelay = true;

            GenerateLupus();

            yield return new WaitForSeconds(lupusReGenerationTime);

            isGenDelay = false;
        }
    }

    void SetPlayerPosition()
    {
        switch (GameManager.instance.PreviousMap)
        {
            case GameManager.Map.Village:
                {
                    player.position = villagePortal.position;
                    player.rotation = villagePortal.rotation;
                    break;
                }
            case GameManager.Map.SacredTreeForest:
                {
                    player.position = sacredTreeForestPortal.position;
                    player.rotation = sacredTreeForestPortal.rotation;
                    break;
                }
            case GameManager.Map.TreesForest:
                {
                    player.position = treesForestPortal.position;
                    player.rotation = treesForestPortal.rotation;
                    break;
                }
            case GameManager.Map.CaveForest:
                {
                    player.position = caveForestPortal.position;
                    player.rotation = caveForestPortal.rotation;
                    break;
                }
            case GameManager.Map.ValleyForestDungeon:
                {
                    player.position = valleyForestDungeonPortal.position;
                    player.rotation = valleyForestDungeonPortal.rotation;
                    break;
                }
            default:
                {
                    player.position = villagePortal.position;
                    player.rotation = villagePortal.rotation;
                    break;
                }
        }
    }
}
