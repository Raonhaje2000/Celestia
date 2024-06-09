using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 상태 관련(씬 전환, 플레이어 위치 등) 처리
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum State
    {
        Title = 0, // 타이틀
        InGame,    // 인게임
        Ending,    // 엔딩
    }

    public enum Map
    {
        Village                 = 0,  // 마을
        MagicSchool             = 1,  // 학교
        Training                = 2,  // 수련장
        SacredTreeForest        = 10, // 신수 숲
        SacredTreeForestDungeon = 11, // 신수 숲 던전
        TreesForest             = 20, // 나무 숲
        TreesForestDungeon      = 21, // 나무 숲 던전
        ValleyForest            = 30, // 계곡 숲
        ValleyForestDungeon     = 31, // 계곡 숲 던전
        CaveForest              = 40, // 동굴 숲
        CaveForestDungeon       = 41  // 동굴 숲 던전
    }                                    

    [SerializeField] State gameState;             // 게임 상태

    [SerializeField] Map currentMap;              // 현재 위치
    [SerializeField] Map previousMap;             // 이전 위치

    bool isPlayerInteractionWithNpc;              // 플레이어가 NPC와 상호작용 중인지 확인하는 플래그                                 

    #region [Property]

    public State GameState
    {
        get { return gameState; }
    }

    public Map CurrentMap
    {
        get { return currentMap; }
    }

    public Map PreviousMap
    { 
        get { return previousMap; }
    }

    public bool IsPlayerInteractionWithNpc
    {
        get { return isPlayerInteractionWithNpc; }
        set { isPlayerInteractionWithNpc = value; }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 다음 씬으로 넘어가도 오브젝트 파괴되지 않고 유지
            // 게임 내에서 공통으로 쓰이는 데이터 유지
            DontDestroyOnLoad(gameObject);

            // 상호작용 오브젝트의 상호작용 여부 초기화
            // 처음 게임 실행 시 딱 1번만 초기화 (씬 변경되어도 다시 초기화 되지 않음)
            InitializeInteractionObjects();
        }
        else if (instance != this)
        {
            // 이미 기존 씬에 해당 오브젝트가 남아있을 경우 제거
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 초기화
        Initialize();
    }

    // 초기화
    void Initialize()
    {
        gameState = State.Title;
        currentMap = Map.Village;
        previousMap = Map.Village;
    }

    // 상호작용 오브젝트의 상호작용 여부 초기화 (상호작용 가능으로 초기화)
    void InitializeInteractionObjects()
    {
        InteractionObjectData[] interactionObjectDataArray = Resources.LoadAll<InteractionObjectData>("GameData/InteractionObject");

        for (int i = 0; i < interactionObjectDataArray.Length; i++)
        {
            interactionObjectDataArray[i].InteractionPossible = true;
        }
    }

    // 타이틀 불러오기 (씬 변경)
    public void LoadTitle()
    {
        // 타이틀 씬 로드 처리
        gameState = State.Title;
        SceneManager.LoadScene("Title");
    }

    // 인게임 불러오기 (씬 변경)
    public void LoadInGame()
    {
        // 인게임 씬 로드 처리
        gameState = State.InGame;

        LoadDestinationMap(currentMap);
    }

    // 엔딩 불러오기 (씬 변경)
    public void LoadEnding()
    {
        // 엔딩 씬 로드 처리
        gameState = State.Ending;
    }

    // 이동하려는 맵으로 이동 (씬 로드)
    public void LoadDestinationMap(Map destinationMapName)
    {
        // 현재 위치를 이전 위치로 저장하고, 현재 위치를 이동한 곳으로 변경                                                       
        previousMap = currentMap;
        currentMap = destinationMapName;

        // 해당되는 씬 로드
        SceneManager.LoadScene((currentMap.ToString()));
    }
}
