using UnityEngine;
using UnityEngine.SceneManagement;

// ���� ���� ����(�� ��ȯ, �÷��̾� ��ġ ��) ó��
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum State
    {
        Title = 0, // Ÿ��Ʋ
        InGame,    // �ΰ���
        Ending,    // ����
    }

    public enum Map
    {
        Village                 = 0,  // ����
        MagicSchool             = 1,  // �б�
        Training                = 2,  // ������
        SacredTreeForest        = 10, // �ż� ��
        SacredTreeForestDungeon = 11, // �ż� �� ����
        TreesForest             = 20, // ���� ��
        TreesForestDungeon      = 21, // ���� �� ����
        ValleyForest            = 30, // ��� ��
        ValleyForestDungeon     = 31, // ��� �� ����
        CaveForest              = 40, // ���� ��
        CaveForestDungeon       = 41  // ���� �� ����
    }                                    

    [SerializeField] State gameState;             // ���� ����

    [SerializeField] Map currentMap;              // ���� ��ġ
    [SerializeField] Map previousMap;             // ���� ��ġ

    bool isPlayerInteractionWithNpc;              // �÷��̾ NPC�� ��ȣ�ۿ� ������ Ȯ���ϴ� �÷���                                 

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

            // ���� ������ �Ѿ�� ������Ʈ �ı����� �ʰ� ����
            // ���� ������ �������� ���̴� ������ ����
            DontDestroyOnLoad(gameObject);

            // ��ȣ�ۿ� ������Ʈ�� ��ȣ�ۿ� ���� �ʱ�ȭ
            // ó�� ���� ���� �� �� 1���� �ʱ�ȭ (�� ����Ǿ �ٽ� �ʱ�ȭ ���� ����)
            InitializeInteractionObjects();
        }
        else if (instance != this)
        {
            // �̹� ���� ���� �ش� ������Ʈ�� �������� ��� ����
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // �ʱ�ȭ
        Initialize();
    }

    // �ʱ�ȭ
    void Initialize()
    {
        gameState = State.Title;
        currentMap = Map.Village;
        previousMap = Map.Village;
    }

    // ��ȣ�ۿ� ������Ʈ�� ��ȣ�ۿ� ���� �ʱ�ȭ (��ȣ�ۿ� �������� �ʱ�ȭ)
    void InitializeInteractionObjects()
    {
        InteractionObjectData[] interactionObjectDataArray = Resources.LoadAll<InteractionObjectData>("GameData/InteractionObject");

        for (int i = 0; i < interactionObjectDataArray.Length; i++)
        {
            interactionObjectDataArray[i].InteractionPossible = true;
        }
    }

    // Ÿ��Ʋ �ҷ����� (�� ����)
    public void LoadTitle()
    {
        // Ÿ��Ʋ �� �ε� ó��
        gameState = State.Title;
        SceneManager.LoadScene("Title");
    }

    // �ΰ��� �ҷ����� (�� ����)
    public void LoadInGame()
    {
        // �ΰ��� �� �ε� ó��
        gameState = State.InGame;

        LoadDestinationMap(currentMap);
    }

    // ���� �ҷ����� (�� ����)
    public void LoadEnding()
    {
        // ���� �� �ε� ó��
        gameState = State.Ending;
    }

    // �̵��Ϸ��� ������ �̵� (�� �ε�)
    public void LoadDestinationMap(Map destinationMapName)
    {
        // ���� ��ġ�� ���� ��ġ�� �����ϰ�, ���� ��ġ�� �̵��� ������ ����                                                       
        previousMap = currentMap;
        currentMap = destinationMapName;

        // �ش�Ǵ� �� �ε�
        SceneManager.LoadScene((currentMap.ToString()));
    }
}
